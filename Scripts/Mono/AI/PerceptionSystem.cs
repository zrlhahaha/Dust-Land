using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlertState
{
    Grey,
    Red
}

[System.Serializable]
public class EnemyInfo
{
    public Collider collider;
    public Vector3 lastSpottedPos;
    public float escapeTimer;
    public float visibility;
    public bool spottedThisFrame;
    public bool accessable;

    public EnemyInfo(Vector3 pos,Collider collider)
    {
        lastSpottedPos = pos;
        spottedThisFrame = accessable = false;
        this.collider = collider;
    }
}



[RequireComponent(typeof(Property))]
public class PerceptionSystem : MonoBehaviour {

    public EnemyInfo[] display;
    public float enemyCheckRange = 100;

    public Property property;
    public Transform eye;
    public float eyeRotateSpeed = 80f;
    public float fov = 150;
    public Vector3 eyeFocusPos;
    public AlertState alertState;

    [Header("Gray")]
    public float alertness;
    public float alertnessRiseSpeed = 400f;
    public float alertnessDropSpeed = 0.5f;

    [Header("Red")]
    public Collider primaryTarget;
    public Dictionary<Collider, EnemyInfo> memory = new Dictionary<Collider, EnemyInfo>();
    public float escapeTime = 3;
    [Range(0.1f, 5f)]
    public float visibilityRiseSpeed = 2.5f;
    [Range(0.1f, 5f)]
    public float visibilityDropSpeed = 3.5f;

    [Header("Sound")]
    public Vector3 nearestSound;

    public void Update()
    {
        display = new EnemyInfo[memory.Values.Count];

        int i = 0;
        foreach (var val in memory.Values)
        {
            display[i++] = val;
        }

        eyeFocusPos = transform.position + transform.forward * enemyCheckRange;

        switch (alertState)
        {
            case (AlertState.Grey):
                State_Grey();
                break;

            case (AlertState.Red):
                State_Red();
                break;
        }

        eye.transform.rotation = Quaternion.RotateTowards(eye.transform.rotation, Quaternion.LookRotation(eyeFocusPos - eye.transform.position), eyeRotateSpeed * Time.deltaTime);

    }

    public Collider[] CheckNearbyEnemy()
    {
        return Physics.OverlapSphere(transform.position, enemyCheckRange, property.team.HostileTeamLayerMask());
    }

    //测试是否能看见这个物体
    public bool VisiblityTest(Collider target)
    {
        //fov test
        float dot = Vector3.Dot(eye.forward, (target.transform.position - eye.position).normalized);
        if (dot < Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad))
            return false;

        RaycastHit hitInfo;
        Ray ray = new Ray(eye.position, target.transform.position - eye.position);
        if (Physics.Raycast(ray, out hitInfo, enemyCheckRange, Utility.ColliderLayer))
            return hitInfo.collider == target;

        return false;
    }

    //计算附近敌人引起的注意度
    public float AttentionTest(float riseSpeed, float dropSpeed)
    {
        Collider[] targets = CheckNearbyEnemy();
        float val = 0f;

        bool spotEnemy = false;
        for (int i = 0; i < targets.Length; i++)
        {
            if (!VisiblityTest(targets[i]))
                continue;

            spotEnemy = true;
            float sqrDistance = (targets[i].transform.position - transform.position).sqrMagnitude;
            float deltaVal = Mathf.Pow(Mathf.Clamp01(1 / sqrDistance), 0.4f) * riseSpeed * Time.deltaTime;

            val += deltaVal;
        }

        if (!spotEnemy)
            val -= dropSpeed * Time.deltaTime;

        return val;
    }

    //获取附近能被看见的敌人
    public Collider[] VisibleEnemies()
    {
        Collider[] targets = CheckNearbyEnemy();
        List<Collider> result = new List<Collider>();
        for (int i = 0; i < targets.Length; i++)
        {
            if (!VisiblityTest(targets[i]))
                continue;

            result.Add(targets[i]);
        }

        return result.ToArray();
    }

    void UpdateMemory(Collider[] spottedTargets)
    {
        //所有记忆中的单位
        List<Collider> destroyedCollider = new List<Collider>();
        foreach (var key in memory.Keys)
        {
            EnemyInfo info = memory[key];

            if (info.collider == null)
            {
                destroyedCollider.Add(key);
                continue;
            }

            info.spottedThisFrame = false;
            if (info.accessable)
                info.lastSpottedPos = info.collider.transform.position;
        }

        //移除已经摧毁的单位
        foreach (var key in destroyedCollider)
            memory.Remove(key);
       
        //这一帧被看见的单位
        for (int i = 0; i < spottedTargets.Length; i++)
        {
            Collider go = spottedTargets[i];
            EnemyInfo info;
            if (memory.ContainsKey(go))
                info = memory[go];
            else
                memory.Add(go, info = new EnemyInfo(go.transform.position, go));

            info.spottedThisFrame = true;

            info.visibility = Mathf.Clamp01(info.visibility + visibilityRiseSpeed * Time.deltaTime);
            info.escapeTimer = 0;
            if (info.visibility > 0.95)
                info.accessable = true;

        }

        List<Collider> removeList = new List<Collider>();

        //这一帧没被看见的单位
        foreach (var key in memory.Keys)
        {
            EnemyInfo info = memory[key];
            if (info.spottedThisFrame)
                continue;

            //消失后短时间仍然可以获取位置
            if (info.visibility < 0.05)
                info.accessable = false;

            info.escapeTimer += Time.deltaTime;
            info.visibility = Mathf.Clamp01(info.visibility - visibilityDropSpeed * Time.deltaTime);

            if (info.escapeTimer > escapeTime)
                removeList.Add(key);
        }

        foreach (var key in removeList)
            memory.Remove(key);
    }

    //选取最有威胁的敌人
    //暂时选取最近的敌人
    Collider GetPrimaryTarget()
    {
        float minSqrDist = float.MaxValue;
        Collider closestEnemy = null;

        foreach (var key in memory.Keys)
        {
            EnemyInfo info = memory[key];
            if (info.visibility > 0.95f)
            {
                float sqrDist = (info.lastSpottedPos - transform.position).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    closestEnemy = key;
                }
            }
        }

        return closestEnemy;
    }

    public bool LastEnemySpottedPos(out Vector3 pos)
    {
        float lowestTimer = float.MaxValue;
        EnemyInfo target = null;

        foreach (var go in memory.Values)
        {
            if (go.escapeTimer < lowestTimer)
            {
                lowestTimer = go.escapeTimer;
                target = go;
            }
        }

        if (target == null)
        {
            pos = Vector3.zero;
            return false;
        }
        else
        {
            pos = target.lastSpottedPos;
            return true;
        }
    }


    void State_Grey()
    {
        alertness += AttentionTest(alertnessRiseSpeed, alertnessDropSpeed);
        alertness = Mathf.Clamp01(alertness);

        if (Battle._instance.GetNearestSound(transform.position, out nearestSound))
            eyeFocusPos = nearestSound;

        if (alertness > 0.99)
        {
            alertState = AlertState.Red;
            alertness = 0;
        }
    }

    void State_Red()
    {
        Collider[] visibleEnemies = VisibleEnemies();

        UpdateMemory(visibleEnemies);

        primaryTarget = GetPrimaryTarget();

        if (primaryTarget == null)
        {
            Vector3 enemyPos;
            if (LastEnemySpottedPos(out enemyPos))
                eyeFocusPos = enemyPos;
        }
        else
            eyeFocusPos = primaryTarget.transform.position;


        if (memory.Count == 0)
            alertState = AlertState.Grey;
    }

    public void Msg_TakeDamege(Property source)
    {
        if (alertState == AlertState.Grey)
        {
            alertState = AlertState.Red;
            alertness = 0;
        }

        if(!memory.ContainsKey(source.TeamTagCollider))
            memory.Add(source.TeamTagCollider, new EnemyInfo(source.transform.position,source.TeamTagCollider));
    }

    private void OnDestroy()
    {
        Battle._instance.BotDestroyed(this);
    }

    private void OnDrawGizmosSelected()
    {
        //视野区域
        Gizmos.color = alertState == AlertState.Grey ? Color.white : Color.red;
        //UnityEditor.Handles.color = Gizmos.color;
        //UnityEditor.Handles.DrawWireDisc(eye.position, Vector3.up, enemyCheckRange);
        Quaternion rotation = Quaternion.AngleAxis(fov * 0.5f, Vector3.up);
        Gizmos.DrawLine(eye.position, eye.position + rotation * eye.forward * enemyCheckRange);
        Gizmos.DrawLine(eye.position, eye.position + Quaternion.Inverse(rotation) * eye.forward * enemyCheckRange);


        Gizmos.color = Color.red;
        Vector3 pos;
        if (primaryTarget == null && LastEnemySpottedPos(out pos))
        {
            Gizmos.DrawWireCube(pos,Vector3.one * 2);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyeFocusPos, 2);
    }

}
