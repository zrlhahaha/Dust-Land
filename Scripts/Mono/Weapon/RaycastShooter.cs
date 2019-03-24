using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShellLaucher))]
[RequireComponent(typeof(TargetingSystem))]
[RequireComponent(typeof(Animator))]
public class RaycastShooter : WeaponBase
{
    public float fireDuration = 0.2f;
    public float effectDuration = 0.08f;
    public float raycastMaxDistance = 200f;
    public float damege = 10f;
    public float impactForce = 20f;
    float fireTimer;

    public Transform firePoint;
    public GameObject fireLight;
    public FX fireParticlePrefab;
    public BulletTrail bulletTrailPrefab;

    public ShellLaucher shellLauncher;
    public TargetingSystem targetingSystem;
    public Animator anim;

    public override void SetTarget(Vector3 target)
    {
        targetingSystem.SetTarget(target);
    }

    public void Update()
    {
        if (Time.time > fireTimer + effectDuration)
            fireLight.SetActive(false);
    }

    public override bool Trigger()
    {
        if (Time.time < fireTimer + fireDuration)
            return false;

        fireTimer = Time.time;

        RaycastHit hitInfo;
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        Vector3 end;
        if (Physics.Raycast(ray, out hitInfo, raycastMaxDistance, Utility.ColliderLayer))
        {
            Property property = hitInfo.transform.GetComponent<Property>();
            if (property != null)
                property.TakeDamege(damege, owner);

            IHitFX hitFX = property;
            hitFX = hitFX ?? hitInfo.transform.GetComponent<IHitFX>();

            HitEffect hitEffect = HitEffect.Defualt;
            if (hitFX != null)
                hitEffect = hitFX.GetHitFXType();

            end = hitInfo.point;
            FX hit_Fx = ResourceManager._instance.GetHitFX(hitEffect);
            hit_Fx.Play(hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            if(hitInfo.rigidbody!=null)
                hitInfo.rigidbody.AddForceAtPosition(impactForce * transform.forward, hitInfo.point);
        }
        else
            end = firePoint.position + firePoint.forward * raycastMaxDistance;

        shellLauncher.Trigger();
        fireLight.SetActive(true);
        anim.SetTrigger(AnimPara.Fire);

        FX fireFX = ResourceManager._instance.GetPoolMember(fireParticlePrefab);
        fireFX.transform.SetParent(firePoint);
        fireFX.Play(firePoint.position, firePoint.rotation);

        BulletTrail trail = ResourceManager._instance.GetPoolMember(bulletTrailPrefab);
        trail.SetTrail(firePoint.position, end);

        return true;
    }

    public override void Enable(bool state)
    {
        targetingSystem.enabled = state;
    }

}
