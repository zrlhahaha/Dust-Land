using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointProgressTracker : MonoBehaviour
{

    public WayPoint wayPoint;

    public float progressDist;
    public Vector3 lastPos;
    public float speed;

    public float aheadPositionOffset = 4;
    public float aheadPositionSpeedFactor = 1;

    public float aheadDirectionOffset = 8;
    public float aheadDirectionSpeedFactor = 2;

    public float progressTrackSensitivity = 0.5f;
    public float speedSmooth = 1f;

    public RoutePoint progressPoint;
    public RoutePoint targetPoint;
    public RoutePoint directionPoint;

    private void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (wayPoint == null || wayPoint.wayPoints.Length < 2)
            return;

        speed = Mathf.Lerp(speed, (transform.position - lastPos).magnitude / Time.deltaTime, speedSmooth * Time.deltaTime);
        lastPos = transform.position;

        targetPoint = wayPoint.GetRoutePoint(progressDist + aheadPositionOffset + aheadPositionSpeedFactor * speed);
        directionPoint = wayPoint.GetRoutePoint(progressDist + aheadPositionOffset + aheadPositionSpeedFactor * speed);
        progressPoint = wayPoint.GetRoutePoint(progressDist);

        Vector3 toProgressPoint = progressPoint.position - transform.position;

        if (Vector3.Dot(toProgressPoint, progressPoint.direction) < 0)
            progressDist += speed * progressTrackSensitivity;

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (wayPoint == null || wayPoint.wayPoints.Length < 2)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(progressPoint.position, 1f);
        Gizmos.DrawLine(targetPoint.position, transform.position);
        Gizmos.DrawWireSphere(directionPoint.position, 1f);
    }

    public void ResetProgress()
    {
        progressDist = 0;
    }

    public void Reposition()
    {
        progressDist = wayPoint.SampleProgress(transform.position) + aheadPositionOffset;
    }
}

