using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponSystem))]
[RequireComponent(typeof(PerceptionSystem))]
public class AIWeaponControl : MonoBehaviour {

    public WeaponSystem weaponSystem;
    public PerceptionSystem perception;
    public Vector3 weaponTarget;
    public float aimingDisturb;
    public float maxAimingDisturb;
    public float aimingDisturbRiseSpeed;
    public float aimingDistrubDropSpeed;
    private void Update()
    {
        Vector3 targetPos;
        bool visible = GetWeaponTarget(out targetPos);
        weaponTarget = targetPos;

        Vector3 disturbedPos = targetPos + aimingDisturb * Utility.RandomVec();
        weaponSystem.SetTarget(disturbedPos);

        aimingDisturb -= aimingDistrubDropSpeed * Time.deltaTime;
        if (visible)
        {
            foreach (var go in weaponSystem.weaponHandles)
            {
                if (go.weapon == null)
                    continue ;

                if (go.weapon.ApprochingTarget())
                {
                    if (go.weapon.Trigger())
                        aimingDisturb += aimingDisturbRiseSpeed * Time.deltaTime;
                }
            }
        }
        aimingDisturb = Mathf.Clamp(aimingDisturb, 0, maxAimingDisturb);
    }

    bool GetWeaponTarget(out Vector3 pos)
    {
        if (perception.primaryTarget != null)
        {
            pos = perception.primaryTarget.transform.position;
            return true;
        }

        Vector3 temp;
        if (perception.LastEnemySpottedPos(out temp))
        {
            pos = temp;
            return false;
        }

        pos = perception.eyeFocusPos;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(weaponTarget, Vector3.one * 3);
    }
}
