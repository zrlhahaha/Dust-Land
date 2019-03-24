using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ShellLaucher))]
[RequireComponent(typeof(TargetingSystem))]
public class RaycasterShotgun : WeaponBase {

    public float fireDuration = 1f;
    public float spread = 1;
    public float maxDistance = 25f;
    public float damege = 20f;
    public float bulletSpeed = 50f;
    public int raysAmount = 7;
    public LayerMask layer;
    public Transform firePoint;
    public Raycaster raycaster;
    public FX firePraticlePrefab;
    public ShellLaucher shellLauncher;
    public Animator anim;
    public TargetingSystem targetingSystem;

    float fireTimer;

    public override bool Trigger()
    {
        if (Time.time < fireTimer + fireDuration)
            return false;

        fireTimer = Time.time;

        for (int i = 0; i < raysAmount; i++)
        {
            Vector2 randomVector2 = Random.insideUnitCircle * spread;
            Vector3 dir_world= firePoint.TransformDirection(new Vector3(randomVector2.x, randomVector2.y, 1));
            Raycaster go = ResourceManager._instance.GetPoolMember(raycaster);
            go.SetTrail(firePoint.position, firePoint.position + dir_world * maxDistance,bulletSpeed,damege,owner);
        }

        FX fire_fx = ResourceManager._instance.GetPoolMember(firePraticlePrefab);
        fire_fx.transform.position = firePoint.position;
        fire_fx.transform.rotation = firePoint.rotation;
        fire_fx.Play(firePoint.position, firePoint.rotation);
        fire_fx.transform.SetParent(firePoint);

        shellLauncher.Trigger();
        anim.SetTrigger(AnimPara.Fire);

        return true;
    }

    public override void SetTarget(Vector3 target)
    {
        targetingSystem.SetTarget(target);
    }

    public override void Enable(bool state)
    {
        targetingSystem.enabled = state;
    }
}
