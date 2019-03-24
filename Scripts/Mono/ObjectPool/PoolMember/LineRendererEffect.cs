
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererEffect : PoolMember {

    public LineRenderer lineRenderer;
    public float delay = 0.1f;
    float expireTime;


    private void Update()
    {
        if (Time.time > expireTime)
        {
            ReturnPool();
        }
    }

    public override void Reset()
    {
        expireTime = 0;
    }

    public void SetLine(Vector3 start,Vector3 end)
    {
        expireTime = Time.time + delay;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

}
