using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility  {

    public const int ObstacleLayer = 1<<8;
    public const int AllTeamLayer = 1 << 9 | 1 << 10 | 1 << 11;
    public const int RigidPropLayer = 1<<13;
    public const int DefaultLayer = 1;
    public const int ColliderLayer = ObstacleLayer | AllTeamLayer | RigidPropLayer | DefaultLayer;

    public static void SetLayerRecursively(Transform target, int layer)
    {
        target.gameObject.layer = layer;
        foreach (Transform go in target)
        {
            SetLayerRecursively(go, layer);
        }
    }


    public static bool IsApproch(this float a, float target, float threshold = 0.0001f)
    {
        if (Mathf.Abs(a - target) <= threshold)
            return true;
        else
            return false;
    }


    public static Vector3 SnapToZero( Vector3 vec,float threshold = 0.001f)
    {
        if (Mathf.Abs(vec.x) < threshold)
            vec.x = 0;

        if (Mathf.Abs(vec.y) < threshold)
            vec.y = 0;

        if (Mathf.Abs(vec.z) < threshold)
            vec.z = 0;

        return vec;
    }

    public static float SnapToZero(float value,float threshold = 0.001f)
    {
        if (Mathf.Abs(value) < threshold)
        {
            value = 0;
        }

        return value;
    }



    public static bool ZoneCheck(this Vector3 vec,Vector3 pos_1,Vector3 pos_2)
    {
        float max, min;

        if (pos_1.x > pos_2.x)
        {
            max = pos_1.x;
            min = pos_2.x;
        }
        else
        {
            max = pos_2.x;
            min = pos_1.x;
        }

        if (vec.x > max || vec.x < min)
            return false;

        if (pos_1.y > pos_2.y)
        {
            max = pos_1.y;
            min = pos_2.y;
        }
        else
        {
            max = pos_2.y;
            min = pos_1.y;
        }

        if (vec.y > max || vec.y < min)
            return false;

        if (pos_1.z > pos_2.z)
        {
            max = pos_1.z;
            min = pos_2.z;
        }
        else
        {
            max = pos_2.z;
            min = pos_1.z;
        }

        if (vec.z > max || vec.z < min)
            return false;

        return true;
    }

    public static Transform GetRootTransform(Transform target)
    {
        
        while (target.parent != null)
        {
            target = target.parent;
        }
        return target;
    }

    public static Vector3 GetAveragePoint(this Vector3[] vectors)
    {
        Vector3 result = new Vector3();
        for (int i = 0; i < vectors.Length; i++)
        {
            result += vectors[i];
        }
        return result;
    }

    public static Vector2 ProjectToVector2(this Vector3 vector3)
    {
        Vector2 go = new Vector2(vector3.x, vector3.z);
        return go;
    }

    public static bool IsPrefab(this GameObject go)
    {
        return go.scene.rootCount == 0;
    }

    public static void Replay(this ParticleSystem ps)
    {
        ps.Clear();
        ps.Play();
    }

    public static Vector3 XOZ(this Vector3 pos)
    {
        Vector3 xoy = pos;
        xoy.y = 0;
        return xoy;
    }

    public static Vector3 ZOY(this Vector3 pos)
    {
        Vector3 zoy = pos;
        zoy.x = 0;
        return zoy;
    }

    public static Vector2 RandomUV()
    {
        return new Vector2(Random.value, Random.value);
    }

    public static int ToLayerMask(this Team team)
    {
        return 1 << (int)team;
    }

    public static int HostileTeamLayerMask(this Team team)
    {
        return team.ToLayerMask() ^ (Team.TeamA.ToLayerMask() | Team.TeamB.ToLayerMask() | Team.TeamC.ToLayerMask());
    }

    public static Vector3 RandomVec()
    {
        Vector3 vec = new Vector3(Random.value, Random.value, Random.value);
        vec = vec * 2 - Vector3.one;
        return vec;
    }

    public static void Reset(this Transform target)
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
        target.localScale = Vector3.one;
    }

}

