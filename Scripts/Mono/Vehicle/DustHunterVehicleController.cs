using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class DustHunterSuspension:Suspension
{
    public Transform hanger_L;
    public Transform bracket_L;
    public Transform spring_L;
    public Transform springJoint_L_Hanger;
    public Transform springJoint_L_Bracket;

    public Transform hanger_R;
    public Transform bracket_R;
    public Transform spring_R;
    public Transform springJoint_R_Hanger;
    public Transform springJoint_R_Bracket;
}

public class DustHunterVehicleController : VehicleController {

    public DustHunterSuspension[] suspensions;

    //public override void ApplyReplaceableWheelInfo(int wheelIndex)
    //{
    //    Wheel wheel = wheels[wheelIndex];
    //    DustHunterSuspension suspension = suspensions[wheelIndex];

    //    //wheel center
    //    suspension.bracket_L.localPosition = Vector3.up * wheel.junctionInfo_L.offset_Axis_Y*0.5f;
    //    suspension.bracket_R.localPosition = Vector3.up * wheel.junctionInfo_R.offset_Axis_Y*0.5f;

    //    base.ApplyReplaceableWheelInfo(wheelIndex);
    //}

    //public override void SyncSuspensionGraphic(int wheelIndex)
    //{
    //    //hanger
    //    Quaternion rot;
    //    Vector3 pos;
    //    Vector3 desireHangerPos;
    //    Wheel wheel = wheels[wheelIndex];
    //    DustHunterSuspension suspension = suspensions[wheelIndex];

    //    if (wheel.wheelCollider_L.enabled == false)
    //    {
    //        pos = Vector3.zero;
    //        rot = Quaternion.identity;
    //        desireHangerPos = suspension.hanger_L.localPosition;
    //        desireHangerPos.y = 0;
    //    }
    //    else
    //    {
    //        wheel.wheelCollider_L.GetWorldPose(out pos, out rot);
    //        desireHangerPos = suspension.hanger_L.localPosition;
    //        desireHangerPos.y = wheel.wheelGraphic_L.localPosition.y;
    //    }

    //    suspension.hanger_L.localPosition = desireHangerPos;


    //    if (wheel.wheelCollider_R.enabled == false)
    //    {
    //        pos = Vector3.zero;
    //        rot = Quaternion.identity;
    //        desireHangerPos = suspension.hanger_R.localPosition;
    //        desireHangerPos.y = 0;
    //    }
    //    else
    //    {
    //        wheel.wheelCollider_R.GetWorldPose(out pos, out rot);
    //        desireHangerPos = suspension.hanger_R.localPosition;
    //        desireHangerPos.y = wheel.wheelGraphic_R.localPosition.y;
    //    }
    //    suspension.hanger_R.localPosition = desireHangerPos;

    //    //spring
    //    suspension.springJoint_L_Bracket.LookAt(suspension.springJoint_L_Hanger);
    //    suspension.springJoint_L_Hanger.LookAt(suspension.springJoint_L_Bracket);
    //    suspension.springJoint_R_Bracket.LookAt(suspension.springJoint_R_Hanger);
    //    suspension.springJoint_R_Hanger.LookAt(suspension.springJoint_R_Bracket);

    //    suspension.spring_L.localScale = Vector3.Distance
    //        (suspension.springJoint_L_Hanger.position, suspension.springJoint_L_Bracket.position) * SpringLength_Reciprocal * Vector3.forward
    //        + new Vector3(1, 1, 0);

    //    suspension.spring_R.localScale = Vector3.Distance
    //        (suspension.springJoint_R_Hanger.position, suspension.springJoint_R_Bracket.position) * SpringLength_Reciprocal * Vector3.forward
    //        + new Vector3(1, 1, 0);

    //    base.SyncSuspensionGraphic(wheelIndex);
    //}

}
