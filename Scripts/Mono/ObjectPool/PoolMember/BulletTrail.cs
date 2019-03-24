using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BulletTrail : PoolMember
{
    [Range(1,2000)]
    public float speed;
    public Vector3 end;
    public Vector3 start;
    public float delay = 0.2f;

    Vector3 dir;
    float sqrProgress;
    Coroutine returnPool;
    bool dirty = false;

    public TrailRenderer trailRenderer;

    private void OnEnable()
    {
        dirty = true;
    }

    void Update()
    {
        if (returnPool != null)
            return;

        if (dirty)
        {
            dirty = false;
            return;
        }

        transform.position = transform.position + dir * speed * Time.deltaTime;
        if ((transform.position - start).sqrMagnitude > sqrProgress)
        {
            transform.position = end;
            returnPool = StartCoroutine(DelayReturnPool());
        }
    }

    public void SetTrail(Vector3 start, Vector3 end)
    {
        trailRenderer.Clear();
        this.start = start;
        this.end = end;
        sqrProgress = (end - start).sqrMagnitude;
        transform.position = start;
        transform.rotation = Quaternion.LookRotation(end - start);
        dir = Vector3.Normalize(end - start);
    }

    public override void Reset()
    {
        returnPool = null;
        sqrProgress = 0;
    }

    public IEnumerator DelayReturnPool()
    {
        yield return new WaitForSeconds(delay);
        ReturnPool();
    }
}
