using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoutePoint
{
    public Vector3 position;
    public Vector3 direction;

    public RoutePoint(Vector3 positon, Vector3 direction)
    {
        this.position = positon;
        this.direction = direction;
    }
}

public class WayPoint : MonoBehaviour
{
    public Transform[] wayPoints;
    public int WayPointsCount { get { return wayPoints.Length; } }
    public float TotalDistance
    {
        get
        {
            return loop ? distances[distances.Length - 1] : distances[distances.Length - 2];
        }
    }
    Vector3[] positions;
    float[] distances;
    Vector3[] sub_positions;             //曲线通过subDevision细分出的各个点存储在这儿
    float[] sub_distances;                 //曲线通过subDevision细分出的距离存储在这儿

    [Range(1f,10f)]
    public float subDevisionDistance = 4f;
    public bool smooth;
    public bool loop;

    void Start()
    {
        CachePositionsAndDistances();

    }

    public RoutePoint GetRoutePoint(float dist)
    {
        if (wayPoints == null || wayPoints.Length < 2)
        {
            print("wayPoints not valid");
            return new RoutePoint();
        }

        Vector3 v1 = GetPosition(dist);
        Vector3 v2 = GetPosition(dist + 0.1f);
        return new RoutePoint(v1, (v2 - v1).normalized);

    }


    public float SampleProgress(Vector3 position)
    {
        if (sub_distances == null)
            return 0;

        float nearestProcessDist = 0f;
        float nearestSqrDist = float.MaxValue;
        for (int i = 0; i < sub_distances.Length; i++)
        {
            float sqrDist = (position - sub_positions[i]).sqrMagnitude;
            if (sqrDist < nearestSqrDist)
            {
                nearestSqrDist = sqrDist;
                nearestProcessDist = sub_distances[i];
            }
        }
        return nearestProcessDist;

    }

    public Vector3 GetPosition(float progress)
    {
        if (loop)
            return GetPositionLoop(progress);
        else
            return GetPositionClamp(progress);
    }

    public Vector3 GetPositionLoop(float progress)
    {
        if (wayPoints == null || wayPoints.Length < 2)
        {
            print("wayPoints not valid");
            return Vector3.zero;
        }

        progress = Mathf.Repeat(progress, TotalDistance);

        int p0, p1, p2, p3;
        p0 = p1 = p2 = p3 = 0;
        while (distances[p2] < progress)
            ++p2;

        p1 = (p2 - 1 + WayPointsCount) % WayPointsCount;

        float i = Mathf.InverseLerp(distances[p1], distances[p2], progress);

        if (smooth)
        {
            p3 = (p2 + 1) % WayPointsCount;
            p0 = (p1 - 1 + WayPointsCount) % WayPointsCount;

            return CatmullRom(positions[p0], positions[p1], positions[p2], positions[p3], i);
        }
        else
            return Vector3.Lerp(positions[p1], positions[p2], i);
    }

    public Vector3 GetPositionClamp(float progress)
    {
        if (wayPoints == null || wayPoints.Length < 2)
        {
            print("wayPoints not valid");
            return Vector3.zero;
        }

        progress = Mathf.Clamp(progress, 0, TotalDistance);

        int p0, p1, p2, p3;
        p0 = p1 = p2 = p3 = 0;

        while (distances[p2] < progress)
            ++p2;

        //p2 = Mathf.Clamp(p2, 0, WayPointsCount - 1);
        p1 = Mathf.Clamp(p2 - 1, 0, WayPointsCount - 1);

        float i = Mathf.InverseLerp(distances[p1], distances[p2], progress);

        if (smooth)
        {
            p3 = Mathf.Clamp(p2 + 1, 0, WayPointsCount - 1);
            p0 = Mathf.Clamp(p1 - 1, 0, WayPointsCount - 1);

            return CatmullRom(positions[p0], positions[p1], positions[p2], positions[p3], i);
        }
        else
        {
            return Vector3.Lerp(positions[p1], positions[p2], i);
        }
    }


    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        return 0.5f *
               ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }


    public void CachePositionsAndDistances()
    {
        if (wayPoints == null || WayPointsCount < 2)
            return;

        positions = new Vector3[WayPointsCount + 1];
        distances = new float[WayPointsCount + 1];                //distance从零开始计数
        float accumulateDistance = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 p1 = wayPoints[i % WayPointsCount].position;
            Vector3 p2 = wayPoints[(i + 1) % WayPointsCount].position;

            positions[i] = p1;
            distances[i] = accumulateDistance;
            accumulateDistance += (p2 - p1).magnitude;
        }


        int count = (int)(TotalDistance / subDevisionDistance);
        sub_distances = new float[count+1];
        sub_positions = new Vector3[count+1];

        for (int i = 0; i < count + 1; i++)
        {
            sub_distances[i] = subDevisionDistance * i;
            sub_positions[i] = GetPosition(sub_distances[i]);
        }
    }

    public void SetWayPoint(Vector3[] newWayPoints)
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Destroy(wayPoints[i].gameObject);
        }

        wayPoints = new Transform[newWayPoints.Length];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = string.Format("waypoint_{0}", i);
            go.transform.position = newWayPoints[i];
            wayPoints[i] = go.transform;
        }

        CachePositionsAndDistances();
    }

    private void OnDrawGizmos()
    {
        if (wayPoints == null || wayPoints.Length < 2)
            return;

        CachePositionsAndDistances();

        Gizmos.color = Color.yellow;

        Vector3 pre = positions[0];
        float length = TotalDistance;
        for (float i = 0; i < length; i += 0.1f)
        {
            Vector3 next = GetPosition(i);
            Gizmos.DrawLine(pre, next);
            pre = next;
        }

        for (int i = 0; i < sub_positions.Length; i++)
        {
            Gizmos.DrawSphere(sub_positions[i], 0.5f);
        }
    }
}
