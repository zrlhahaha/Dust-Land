using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Suspension
{
}

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour {



    [Header("Vehicle Physics")]
    public float wheelMaxAngle = 35f;
    public float steerAngle;
    public float steerAngleSmooth;
    public float motorTorque = 2500f;
    public float footBrakeTorque = 3000f;
    public float handBrakeTorque = 2000f;
    public float maxSpeed = 30f;
    public float reverseAdditonalTorque = 1000f;
    public float driftFrictionFactor = 0.5f;
    public float downForcePerSpeed = 50;
    public float baseDownForce = 3000;
    public Vector3 relativeVelocity;

    [Header("Dependence")]
    public WheelHolder[] holders;
    public Rigidbody rigid;
    public Transform centerOfMass;
    public ItemInfo vehicleItemInfo;
    public ItemInfo wheelItemInfo;
    void Awake () {
        rigid.centerOfMass = centerOfMass.localPosition;

    }

    private void FixedUpdate()
    {
        relativeVelocity =Utility.SnapToZero(transform.InverseTransformVector(rigid.velocity),0.1f);

        Vector3 desireDownForce = Vector3.down * (downForcePerSpeed * rigid.velocity.magnitude + baseDownForce);
        rigid.AddForce(desireDownForce);

        for (int i = 0; i < holders.Length; i++)
        {
            WheelHolder holder = holders[i];
            if (holder.empty)
                return;

            holder.SyncWheelModel();
            SyncSuspensionGraphic(i);
        }
        
        if (rigid.velocity.sqrMagnitude >= maxSpeed * maxSpeed)
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
    }

    public void Move(float steering, float accel, bool footBrake, bool handBrake)
    {
        if (enabled == false)
            return;

        steering = Mathf.Clamp(steering, -1, 1);
        accel = Mathf.Clamp(accel, -1, 1);

        float motorTorque = DriveTorque(accel);
        float steeringAngle =SteeringRotate(steering);
        float footBrakeTorque = FootBrakeTorque(footBrake);
        float handbrakeTorque = HandBrakeTorque(handBrake);
        float fricionFactor = FrictionFactor(handBrake);

        for (int i = 0; i < holders.Length; i++)
        {
            WheelHolder holder = holders[i];
            
            //根据holder的属性来决定是否应用
            float _driveTorque = holder.motor ? motorTorque : 0;
            float _steeringAngle = holder.steerRotate ? steeringAngle : 0;

            float _brakeTorque = 0;
            if (holder.handBrake)
                _brakeTorque += handbrakeTorque;

            if (holder.footBrack)
                _brakeTorque += footBrakeTorque;

            if(holder.handBrake)
                holder.SetFrictionFactor(fricionFactor);
            holder.Drive(_driveTorque, _brakeTorque, _steeringAngle);

        }
    }

    float DriveTorque(float accel)
    {
        float reverseTorque = Vector3.Dot(Vector3.forward*accel, relativeVelocity) < 0 ? reverseAdditonalTorque : 0;
        float desireTorque = accel * ( motorTorque +reverseTorque);
        return desireTorque;
    }

    float SteeringRotate(float steering)
    {
        return steerAngle = Mathf.Lerp(steerAngle, steering * wheelMaxAngle, steerAngleSmooth * Time.deltaTime); ;
    }

    float HandBrakeTorque(bool handBrake)
    {
        return handBrake ? handBrakeTorque : 0;
    }

    float FootBrakeTorque(bool footBrake)
    {
        return footBrake ? footBrakeTorque : 0;
    }

    float FrictionFactor(bool handBrake)
    {
        return handBrake ? driftFrictionFactor : 1;
    }

    public void ChangeWheel(ItemInfo newItem)
    {
        if (newItem == null || newItem.itemType != ItemType.Wheel)
            return;

        for (int i = 0; i < holders.Length; i++)
        {
            WheelHolder holder = holders[i];
            holder.ChangeNewWheel(newItem);
        }

        wheelItemInfo = newItem;
    }

    public void StopCar()
    {
        foreach (var holder in holders)
        {
            holder.Drive(0, 100000000, 0);
            holder.SyncWheelModel();
        }
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 inverseForce = Vector3.Dot(Vector3.up, collision.impulse) * Vector3.down;
        rigid.AddForce(inverseForce);
    }

    public virtual void SyncSuspensionGraphic(int wheelIndex)
    {

    }
}
