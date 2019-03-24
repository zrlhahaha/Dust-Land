using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetingSystem))]
[RequireComponent(typeof(ShellLaucher))]
public class ColliderShooter : WeaponBase
{
    public float fireDuration = 0.2f;
    public float damege = 10;
    public float range = 10;
    public float bulletVelocity = 100f;
    float timer;

    public FXContainer hitFX;
    public FXContainer fireFX;
    public Transform firePoint;
    public Explosion bulletPrefab;
    public ShellLaucher shellLauncher;
    public TargetingSystem targetingSystem;

    public override bool Trigger()
    {
        if (timer + fireDuration > Time.time)
            return false;

        timer = Time.time;

        Explosion ball = ResourceManager._instance.GetPoolMember(bulletPrefab);
        Vector3 velocity = firePoint.forward * bulletVelocity + rootRigidbody.velocity;
        ball.Trigger(firePoint.position, firePoint.rotation, velocity, damege,range,owner);
        ball.SetHitFX(hitFX);

        fireFX.Play(firePoint.position, firePoint.rotation);

        if (shellLauncher != null) 
            shellLauncher.Trigger();

        return true;
    }

    public override void Enable(bool state)
    {
        targetingSystem.enabled = state;
    }

    public override void SetTarget(Vector3 target)
    {
        targetingSystem.SetTarget(target);
    }

    public override bool ApprochingTarget()
    {
        return targetingSystem.AngleToTarget()<5;
    }
}
