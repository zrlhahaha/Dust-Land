using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {

    protected Property owner;
    protected Rigidbody rootRigidbody;
    protected Transform rootTransform;

    public ShakeData cameraShakingData;
    public void Awake()
    {
        rootTransform = Utility.GetRootTransform(transform);
        owner = rootTransform.GetComponent<Property>();
        if (owner == null)
            Debug.Log("Didn't Find Property In Root Transform");

        rootRigidbody = rootTransform.GetComponent<Rigidbody>();
        if (rootRigidbody == null)
            Debug.Log("Didn't Find RigidBody In RootTransform");
    }

    public virtual bool Trigger()
    {
        return false;
    }

    public virtual void SetTarget(Vector3 target)
    {

    }

    public virtual bool ApprochingTarget()
    {
        return true;
    }

    public virtual void Enable(bool state)
    {

    }

    public virtual void OnImpactOtherEntity(Collision collision,Property other)
    {

    }
}
