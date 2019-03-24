using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelHolder : MonoBehaviour
{
    //这个用来标记轮胎与支架间的连接处
    public Wheel wheel;
    public ItemInfo itemInfo;
    public bool motor;
    public bool steerRotate;
    public bool handBrake;
    public bool footBrack;
    public bool empty { get { return wheel == null; } }

    public void SyncWheelModel()
    {
        if (empty)
            return;

        wheel.SyncWheelModel();
    }

    public void Drive(float motorTorque, float brakeTorque, float steeringAngle)
    {
        if (empty)
            return;

        wheel.Drive(motorTorque, brakeTorque, steeringAngle);
    }

    public void SetFrictionFactor(float sidewayFrictionFactor)
    {
        if (empty)
            return;

        wheel.SetFrictionFactor(sidewayFrictionFactor);
    }

    public void DestroyWheel()
    {
        if (empty)
            return;

        GameObject.Destroy(wheel.gameObject);
        wheel = null;
        itemInfo = null;
    }

    public void ChangeNewWheel(ItemInfo itemInfo)
    {
        if (!empty)
            DestroyWheel();

        if (itemInfo == null)
            return;

        this.itemInfo = itemInfo;

        GameObject newWheel = Instantiate(itemInfo.prefab, transform);
        newWheel.transform.localPosition = Vector3.zero;
        newWheel.transform.localRotation = Quaternion.identity;
        wheel = newWheel.GetComponent<Wheel>();

        //wheel.SyncWheelModel();
    }
}
