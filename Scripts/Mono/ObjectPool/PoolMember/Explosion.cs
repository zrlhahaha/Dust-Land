using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Explosion : PoolMember {
    public ShakeData shakeData;
    public float damege;
    public float impactForce;
    public float impactRange;

    public FXContainer hitFX;
    public Property source;
    public Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (hitFX != null)
            hitFX.Play(transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

        int hostileLayer = source.team.HostileTeamLayerMask();
        int effectLayer = hostileLayer | Utility.RigidPropLayer;
        int hitLayer = Utility.ColliderLayer;

        //只对TeamTag做检测
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRange, effectLayer);
        foreach (var go in colliders)
        {
            Transform rootTransform = Utility.GetRootTransform(go.transform);
            if (rootTransform == transform)
                continue;

            RaycastHit hitInfo;
            if (!Physics.Raycast(transform.position , go.transform.position - transform.position, out hitInfo, impactRange, hitLayer) 
                || Utility.GetRootTransform(hitInfo.transform) != rootTransform)
                continue;

            Vector3 impactPoint = hitInfo.point;
            Vector3 dir = impactPoint - transform.position;
            float sqrDist = dir.sqrMagnitude;

            if ((go.gameObject.layer | hostileLayer) != 0)
            {
                Transform root = Utility.GetRootTransform(go.transform);
                Property property = root.GetComponent<Property>();

                float atten = Mathf.Clamp(Mathf.Pow(1 / sqrDist, 0.2f), 0.5f, 1f);
                if (property != null)
                    property.TakeDamege(atten * damege, source);

                if (root == Player._instance.player)
                    Player._instance.cameraShake.AddShakeEvent(shakeData);
            }

            if (go.attachedRigidbody != null)
                go.attachedRigidbody.AddForce(dir.normalized * impactForce / Mathf.Max( sqrDist,0.1f),ForceMode.Impulse);
        }

        ReturnPool();
    }

    private void Update()
    {
        if (transform.position.y < -10)
            ReturnPool();
    }

    public void Trigger(Vector3 position,Quaternion rotation,Vector3 velocity,float damege,float range,Property source)
    {
        this.source = source;
        transform.position = position;
        transform.rotation = rotation;
        rigid.velocity = velocity;
        this.damege = damege;
        impactRange = range;
    }

    public void SetHitFX(FXContainer fxs)
    {
        hitFX = fxs;
    }

    public override void Reset()
    {
        hitFX = null;
    }

}
