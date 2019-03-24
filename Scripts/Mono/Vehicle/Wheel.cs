using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wheel : MonoBehaviour 
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelModels;

    WheelFrictionCurve[] defaultFrictionCurve;

    private void Awake()
    {
        defaultFrictionCurve = new WheelFrictionCurve[wheelColliders.Length];
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            defaultFrictionCurve[i] = wheelColliders[i].sidewaysFriction;
        }
    }

    public void SyncWheelModel()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider collider = wheelColliders[i];
            Transform model = wheelModels[i];

            Vector3 pos;
            Quaternion rot;

            if (collider.enabled == false)
            {
                pos = Vector3.zero;
                rot = Quaternion.identity;
            }
            else
                collider.GetWorldPose(out pos, out rot);

            model.position = pos;
            model.rotation = rot;
        }
    }

    public void Drive(float motortorque, float brakeTorque, float steeringAngle)
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider collider = wheelColliders[i];

            collider.motorTorque = motortorque;
            collider.brakeTorque = brakeTorque;
            collider.steerAngle = steeringAngle;
        }
    }

    public void SetFrictionFactor(float sidewayFrictionFactor)
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider collider = wheelColliders[i];
            WheelFrictionCurve curve = defaultFrictionCurve[i];
            curve.stiffness *= sidewayFrictionFactor;
            collider.sidewaysFriction = curve;
        }
    }


}
