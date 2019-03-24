using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem: MonoBehaviour{

    public Transform holder;
    public Transform holderPivot;
    public Transform weapon;
    public Transform weaponPivot;
    public Transform chamber;
    float chamber_holder_r;
    float chamber_weapon_r;

    public float maxDegree_Weapon = 45f;
    public float minDegree_Weapon = -45f;

    public bool clampHolderRot = false;
    public float maxDegree_Holder;
    public float minDegree_Holder;

    public float rotateSpeed_Weapon = 60f;
    public float rotateSpeed_Holder = 30f;
    float angle_Weapon;
    float angle_Holder;
    float desireAngle_Weapon;
    float desireAngle_Holder;
    public Vector3 target;
    private void Awake()
    {
        chamber_holder_r = holder.InverseTransformPoint(chamber.position).x;
        chamber_weapon_r = chamber.localPosition.y;
    }

    private void FixedUpdate()
    {
        UpdateDesireAngle(target);
        UpdateRotation();
        ApplyRotation();
    }



    void UpdateDesireAngle(Vector3 pos)
    {
        Vector3 pos_holder = holderPivot.InverseTransformPoint(pos);
        Vector3 pos_weapon = weaponPivot.InverseTransformPoint(pos);

        //层级如下
        //-HolderPivot
        //-Holder
        //  -WeaponPivot
        //  -Weapon
        //    -Chamber
        //weaponPivot,holderPivot,basePivot都是参考点，不能改变
        //设chamber为C,base为O,W为weponPivot,目标点pos为M,XYZ分别为轴末端
        //假设bc垂直base.forward,既chamber在base的x轴上
        //这段计算是在holderPivot坐标系中
        Vector3 pos_xoz = pos_holder.XOZ();
        float MOZ = Vector3.Angle(pos_xoz, Vector3.forward) * Mathf.Sign(pos_xoz.x);
        float CMO = Mathf.Asin(chamber_holder_r / pos_xoz.magnitude) * Mathf.Rad2Deg;

        desireAngle_Holder = MOZ - CMO;
        //desireAngle_Holder = angle_holder;
        //这段计算是在weaponPivot坐标系中
        Vector3 pos_zoy = pos_weapon.ZOY();
        float length_pos_zoy_world = (pos - weaponPivot.position).magnitude;            //pos_zoy.magnitude会随着holderPiovt的转动而变化,所以要用worldSpace的
        float ZOM = Mathf.Asin(pos_zoy.y / length_pos_zoy_world) * Mathf.Rad2Deg;
        float OMC = Mathf.Asin(chamber_weapon_r / length_pos_zoy_world) * Mathf.Rad2Deg;

        desireAngle_Weapon = -(ZOM - OMC);
    } 

    void UpdateRotation()
    {
        angle_Holder = Mathf.MoveTowardsAngle(angle_Holder, desireAngle_Holder, rotateSpeed_Holder*Time.deltaTime);
        angle_Weapon = Mathf.MoveTowardsAngle(angle_Weapon, desireAngle_Weapon, rotateSpeed_Weapon*Time.deltaTime);

        ClampAngle();
    }

    void ClampAngle()
    {
        angle_Weapon = Mathf.Clamp(angle_Weapon, minDegree_Weapon, maxDegree_Weapon);

        if (clampHolderRot)
        {
            angle_Holder = Mathf.Clamp(angle_Holder, minDegree_Holder, maxDegree_Holder);
        }
    }

    void ApplyRotation()
    {
        weapon.localRotation = Quaternion.Euler(Vector3.right * angle_Weapon);
        holder.localRotation = Quaternion.Euler(Vector3.up * angle_Holder);
    }

    public virtual void SetTarget(Vector3 pos)
    {
        target = pos;
    }

    public float AngleToTarget()
    {
        return Vector3.Angle(chamber.forward, target - chamber.position);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(chamber.position, chamber.position + chamber.forward * 100);
    //}
}
