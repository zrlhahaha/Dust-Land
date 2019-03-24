using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Raycaster : PoolMember {

    public float speed;
    public float damege;
    public float impactForce = 20;
    public float delay = 0.2f;
    public Vector3 end;
    public Vector3 start;
    public Vector3 lastPosition;
    public TrailRenderer trailRenderer;
    public Property source;

    float sqrProgress;
    Coroutine returnPool;

    private void Awake()
    {
        trailRenderer.Clear();
    }

    void Update()
    {
        if (returnPool != null)
            return;

        if ((transform.position - start).sqrMagnitude > sqrProgress)
            returnPool = StartCoroutine(DelayReturnPool());

        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        RaycastHit hitInfo;
        float deltaDist = (transform.position - lastPosition).magnitude;
        if (Physics.Raycast(transform.position, transform.position - lastPosition, out hitInfo, deltaDist, Utility.ColliderLayer))
        {
            transform.position = hitInfo.point;

            //伤害
             Property property = hitInfo.transform.GetComponent<Property>();
            if (property != null)
                property.TakeDamege(damege,source);

            //特效
            IHitFX hitFX = property;
            hitFX = hitFX ?? hitInfo.transform.GetComponent<IHitFX>();

            HitEffect hitEffect = HitEffect.Defualt;
            if (hitFX != null) 
                hitEffect = hitFX.GetHitFXType();

            FX fx = ResourceManager._instance.GetHitFX(hitEffect);
            fx.Play(hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            //物理
            if(hitInfo.rigidbody!=null)
                hitInfo.rigidbody.AddForceAtPosition(impactForce * transform.forward, hitInfo.point);

            returnPool = StartCoroutine(DelayReturnPool());
        }
        lastPosition = transform.position;
    }

    public override void Reset()
    {
        trailRenderer.Clear();
        returnPool = null;
        sqrProgress = 0;
    }

    public void SetTrail(Vector3 start,Vector3 end,float speed,float damege,Property source)
    {
        this.start = start;
        this.end = end;
        this.speed = speed;
        this.damege = damege;
        sqrProgress = (end - start).sqrMagnitude;
        this.source = source;

        transform.position = start;
        lastPosition = start;
        transform.rotation = Quaternion.LookRotation(end-start);
    }

    public IEnumerator DelayReturnPool()
    {
        yield return new WaitForSeconds(delay);
        ReturnPool();
    }
}
