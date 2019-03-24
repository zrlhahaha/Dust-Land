using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class _Input : MonoBehaviour {

    public static Vector3 inputVec;
    public static bool isMove;
    public static bool handBrack;
    public static bool triggerWeapon;

    private void Update()
    {
        InputVec();
        IsMove();
        HandBrack();
        TriggerWeapon();
    }

    public void InputVec()
    {
        inputVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            inputVec.z = 1;
        else if (Input.GetKey(KeyCode.S))
            inputVec.z = -1;

        if (Input.GetKey(KeyCode.D))
            inputVec.x = 1;
        else if (Input.GetKey(KeyCode.A))
            inputVec.x = -1;
    }

    public void IsMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            isMove = true;
        else
            isMove = false;

    }

    public void HandBrack()
    {
        if (Input.GetKey(KeyCode.Space))
            handBrack = true;
        else
            handBrack = false;
    }

    public void TriggerWeapon()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            triggerWeapon = true;
        else
            triggerWeapon = false;
    }
}
