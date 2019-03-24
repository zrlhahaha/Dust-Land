using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : WheelBase {

    public TrackedVehicleController owner;

    public WheelCollider[] wheelColliders;
    public Transform[] wheelGraphics;
    public Transform[] trackBones;
    public int direction;
    public AnimationCurve AC_SteerTorque;

    public float trackOffset_X;
    public float textureOffsetSpeedTweaker;
    public Material mat;
    public Renderer trackRenderer;
    public float trackOffsetTweaker = 1;
    int mainTexId;

    private void Start()
    {
        mat = new Material(trackRenderer.material);
        trackRenderer.material = mat;
        mainTexId = Shader.PropertyToID("_MainTex");
    }

    void SyncWheelGraphic()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Quaternion rot;
            Vector3 pos;

            if (wheelColliders[i].enabled == false)
            {
                pos = Vector3.zero;
                rot = Quaternion.identity;
            }
            else
            {
                wheelColliders[i].GetWorldPose(out pos, out rot);
            }
            wheelGraphics[i].rotation = rot;
            wheelGraphics[i].position = pos;
            trackBones[i].localPosition = trackBones[i].parent.InverseTransformPoint(pos) + trackOffset_X * Vector3.right;
        }
        mat.SetTextureOffset(mainTexId, wheelColliders[0].rpm  * wheelColliders[0].radius* trackOffsetTweaker * Vector2.up);

    }

    public override void Motor(float torque)
    {

        if (owner.rigid.velocity.sqrMagnitude > owner.maxSpeed * owner.maxSpeed)
        {
            torque = 0;
        }

        float steerTorque = AC_SteerTorque.Evaluate(owner.rigid.velocity.magnitude/owner.maxSpeed)*owner.motorTorque;

        foreach (var go in wheelColliders)
        {
            go.motorTorque = _Input.inputVec.z *torque - direction * _Input.inputVec.x * steerTorque;
        }

        base.Motor(torque);
    }

    public override void HandBrack(float torque)
    {
        foreach (var go in wheelColliders)
        {
            go.brakeTorque = torque;
        }

        base.HandBrack(torque);
    }

    public override void FootBrack(float torque)
    {
        foreach (var go in wheelColliders)
        {
            go.brakeTorque = torque;
        }

        base.FootBrack(torque);
    }


    public override void DefaultBrakeTorque(float defaultBrackTorque = 0)
    {
        foreach (var go in wheelColliders)
        {
            go.brakeTorque = defaultBrackTorque;
        }

        base.DefaultBrakeTorque(defaultBrackTorque);
    }

    public override void SyncGraphic()
    {
        SyncWheelGraphic();

        base.SyncGraphic();
    }

}
