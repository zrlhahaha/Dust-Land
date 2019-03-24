using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraSetup :  MonoBehaviour{

    public TPCamera mainCamera;
    public Camera defaultCamera;

    public static CameraSetup _instance;

    //Awakw -->Start
    private void Start()
    {
        _instance = this;

        defaultCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);

    }

    public void ActiveTPCamera(Transform target) { 
        defaultCamera.gameObject.SetActive(false);
        mainCamera.followTarget = target;
        mainCamera.gameObject.SetActive(true);
    }

    public void SetTpCameraTarget(Transform target)
    {
        mainCamera.followTarget = target;
    }

}
