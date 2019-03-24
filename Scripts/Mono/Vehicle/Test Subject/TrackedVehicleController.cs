using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackedVehicleController : MonoBehaviour {

    [Header("References Setup")]
    public Track[] tracks;
    public Rigidbody rigid;
    public Transform centerOfMass;
    public ItemInfo vehicleItemInfo;
    public ItemInfo wheelItemInfo;


    [Header("Vehicle Physics")]
    public float motorTorque = 2500f;
    public float brakeTorque = 5000f;
    public float maxSpeed = 30f;
    public float IdleAutoBrackTorque;

    void Start () {
        rigid.centerOfMass = centerOfMass.localPosition;
    }

    void Update () {
        WheelControl();
	}

    void WheelControl()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            Track track = tracks[i];

            track.DefaultBrakeTorque(IdleAutoBrackTorque);

            track.Motor(motorTorque);

            if(_Input.handBrack)
                track.HandBrack(brakeTorque);

            Vector3 relativeVelocity = transform.InverseTransformVector(rigid.velocity);

            if (Utility.SnapToZero(_Input.inputVec.z * relativeVelocity.z) < 0)
            {
                track.FootBrack(brakeTorque);
            }

            if (_Input.handBrack)
                track.HandBrack(brakeTorque);

            track.SyncGraphic();
        }
    }

}
