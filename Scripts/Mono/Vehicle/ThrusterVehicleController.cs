using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThrusterSuspension : Suspension
{
    public Transform wheelHolder_L;
    public Transform hanger_L;
    public Transform springJoint_Hanger_L;
    public Transform sprintJoint_Bracket_L;
    public Transform spring_L;

    public Transform wheelHolder_R;
    public Transform hanger_R;
    public Transform springJoint_Hanger_R;
    public Transform sprintJoint_Bracket_R;
    public Transform spring_R;
}

public class ThrusterVehicleController : VehicleController {

    public ThrusterSuspension[] suspensions;
    [UnityEngine.Serialization.FormerlySerializedAs("wheelHoldersParent")]
    public Transform wheelHolders;

    //public override void SyncSuspensionGraphic(int wheelIndex)
    //{
    //    //hanger
    //    Quaternion rot;
    //    Vector3 pos;
    //    Vector3 desireHingePos;
    //    Vector3 wheelHolderDirection;
    //    Wheel wheel = wheels[wheelIndex];
    //    ThrusterSuspension suspension = suspensions[wheelIndex];
    //    if (wheel.wheelCollider_L.enabled == false)
    //    {
    //        pos = Vector3.zero;
    //        rot = Quaternion.identity;

    //        desireHingePos = suspension.wheelHolder_L.localPosition;
    //        desireHingePos.y = 0;
    //    }
    //    else
    //    {
    //        wheel.wheelCollider_L.GetWorldPose(out pos, out rot);

    //        desireHingePos = suspension.wheelHolder_L.localPosition;
    //        desireHingePos.y = wheelHolders.InverseTransformPoint(pos).y;
    //    }


    //    suspension.wheelHolder_L.transform.localPosition = desireHingePos;

    //    wheelHolderDirection = suspension.wheelHolder_L.position - suspension.hanger_L.position + Vector3.up * 0.1f;
    //    suspension.hanger_L.rotation = Quaternion.LookRotation(wheelHolderDirection);

    //    if (wheel.wheelCollider_R.enabled == false)
    //    {
    //        pos = Vector3.zero;
    //        rot = Quaternion.identity;
    //        desireHingePos = suspension.wheelHolder_R.localPosition;
    //        desireHingePos.y = 0;
    //    }
    //    else
    //    {
    //        wheel.wheelCollider_R.GetWorldPose(out pos, out rot);
    //        desireHingePos = suspension.wheelHolder_R.localPosition;
    //        desireHingePos.y = wheelHolders.InverseTransformPoint(pos).y;
    //    }

    //    suspension.wheelHolder_R.transform.localPosition = desireHingePos;

    //    //这里添加了一个模拟的数值以达到合适的视觉效果
    //    wheelHolderDirection = suspension.wheelHolder_R.position - suspension.hanger_R.position+Vector3.up*0.1f;
    //    suspension.hanger_R.rotation = Quaternion.LookRotation(wheelHolderDirection);

    //    //spring
    //    suspension.sprintJoint_Bracket_L.LookAt(suspension.springJoint_Hanger_L);
    //    suspension.springJoint_Hanger_L.LookAt(suspension.sprintJoint_Bracket_L);
    //    suspension.sprintJoint_Bracket_R.LookAt(suspension.springJoint_Hanger_R);
    //    suspension.springJoint_Hanger_R.LookAt(suspension.sprintJoint_Bracket_R);

    //    suspension.spring_L.localScale = Vector3.Distance
    //        (suspension.springJoint_Hanger_L.position, suspension.sprintJoint_Bracket_L.position) * SpringLength_Reciprocal * Vector3.forward
    //        + new Vector3(1, 1, 0);

    //    suspension.spring_R.localScale = Vector3.Distance
    //        (suspension.springJoint_Hanger_R.position, suspension.sprintJoint_Bracket_R.position) * SpringLength_Reciprocal * Vector3.forward
    //        + new Vector3(1, 1, 0);


    //    base.SyncSuspensionGraphic(wheelIndex);
    //}

}
