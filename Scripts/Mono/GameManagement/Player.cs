using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player _instance;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogWarning("another Gobal instance exist");

        _instance = this;

        SetPlayerVehicle(player);
        SetMainCameraToPlayerVehicle();
    }

    public Camera mainCamera;
    public Transform cameraRoot;
    public ShakeTransform cameraShake;
    public TPCamera tpCamera;

    public Transform player;
    [HideInInspector] public VehicleController playerVehicle;
    [HideInInspector] public WeaponSystem playerWeaponSystem;
    [HideInInspector] public PlayerVehicleControl playerVehicleControl;
    [HideInInspector] public Collider playerTag;
    [HideInInspector] public Property playerProperty;

    public void SetMainCamera(Transform parent)
    {
        cameraRoot.SetParent(parent);
        cameraRoot.localPosition = Vector3.zero;
        cameraRoot.localRotation = Quaternion.identity;
    }

    public void SetMainCameraToPlayerVehicle()
    {
        tpCamera.followTarget = playerVehicleControl.transform;
        SetMainCamera(tpCamera.cameraPos);
    }


    public void SetPlayerVehicle(Transform  newPlayer)
    {
        player = newPlayer;
        playerVehicle = newPlayer.GetComponent<VehicleController>();
        playerWeaponSystem = newPlayer.GetComponent<WeaponSystem>();
        playerVehicleControl = newPlayer.GetComponent<PlayerVehicleControl>();
        playerProperty = newPlayer.GetComponent<Property>();
        playerTag = playerProperty.TeamTagCollider;
    }

    private void Update()
    {
        //mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, Vector3.zero, 7f);
        //mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.identity, 7f);
    }


}
