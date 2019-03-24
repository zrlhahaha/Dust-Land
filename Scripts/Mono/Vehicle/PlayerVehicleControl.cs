using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof( VehicleController))]
[RequireComponent(typeof(WeaponSystem))]
public class PlayerVehicleControl : MonoBehaviour {

    Camera playerCamera;
    public float accel;
    public bool footBrake;
    public bool handBrake;
    public float steering;
    Vector3 hitPos;

    [Header("Dependence")]
    public VehicleController vehicle;
    public WeaponSystem weaponSystem;

    void Start () {
        playerCamera = Player._instance.mainCamera;
    }

    void FixedUpdate () {

        accel = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");
        handBrake = Input.GetKey(KeyCode.Space);

        footBrake = accel > 0 && vehicle.relativeVelocity.z < 0;
        vehicle.Move(steering, accel, footBrake, handBrake);

        RaycastHit hitInfo;
        Vector3 target;
        Ray centerRay;
        centerRay = playerCamera.ScreenPointToRay(new Vector3(playerCamera.pixelWidth * 0.5f, playerCamera.pixelHeight * 0.5f, 0));
        if (Physics.Raycast(centerRay, out hitInfo, 500, Utility.ColliderLayer))
            target = hitInfo.point;
        else
            target = playerCamera.transform.position + playerCamera.transform.forward * 100f;

        hitPos = hitInfo.point;

        weaponSystem.SetTarget(target);

        foreach (var go in weaponSystem.weaponHandles)
        {
            if (go.weapon == null)
                return;

            if (Input.GetKey(go.triggerKey))
                if (go.weapon.Trigger())
                {
                    ShakeData shakeData = go.weapon.cameraShakingData;
                    if(shakeData!=null)
                        Player._instance.cameraShake.AddShakeEvent(shakeData);
                    Battle._instance.AddSound(transform.position);
                }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPos, 0.1f);
    }

}
