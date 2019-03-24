using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBase : MonoBehaviour {

    public GameObject detachPrefab;

    public virtual void Motor(float torque)
    {

    }

    public virtual void HandBrack(float torque)
    {

    }

    public virtual void FootBrack(float torque)
    {

    }

    public virtual void Steer(float torque)
    {

    }

    public virtual void DefaultBrakeTorque(float defaultBrackTorque = 0)
    {

    }

    public virtual void SyncGraphic()
    {

    }

}
