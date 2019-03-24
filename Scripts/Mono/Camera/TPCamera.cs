using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour {

    public Transform camera_X;
    public Transform camera_Y;
    public Transform followTarget;
    public Transform cameraPos;

    [Header("Rotate")]
    public float rotateSpeed_X = 120f;
    public float rotateSpeed_Y = 120f;
    public float rotateSmooth = 5f;
    public bool inverseAxis_Y =false;

    public float maxRotateAngle_X = 60f;
    public float minRotateAngle_X = -30f;

    [Header("Obstacle Offset")]
    [Range(0.1f,10)]
    public float localCameraMaxDistance_Z = 4f;
    public float obstacleOffset;
    public float localCameraDistanceSmooth = 7f;
    //public float localCameraOvershootDistanceSmooth = 20f;
    public bool stopRotate = false;

    [Header("Follow")]
    public float followSmooth = 5f;
    public Vector3 followOffset = Vector3.up * 4;


    float mouseSmooth_X;
    float mouseSmooth_Y;
    float angle_X;
    float angle_Y;

    void Start () {
		cameraPos.transform.localPosition = localCameraMaxDistance_Z*Vector3.back;
    }

    void FixedUpdate () {

        if (!stopRotate)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            int inverse_Y = inverseAxis_Y ? -1 : 1;

            mouseSmooth_X = Mathf.Lerp(mouseSmooth_X, x, rotateSmooth);
            mouseSmooth_Y = inverse_Y * Mathf.Lerp(mouseSmooth_Y, y, rotateSmooth);

            angle_X += mouseSmooth_Y * rotateSpeed_Y * Time.deltaTime;
            angle_Y += mouseSmooth_X * rotateSpeed_Y * Time.deltaTime;

            angle_X = Mathf.Clamp(angle_X, minRotateAngle_X, maxRotateAngle_X);
            camera_X.localRotation = Quaternion.AngleAxis(angle_X,Vector3.right);
            camera_Y.localRotation = Quaternion.AngleAxis(angle_Y, Vector3.up);
        }

        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, cameraPos.position - transform.position);
        float maxCameraLocal_Z = localCameraMaxDistance_Z,z_abs;
        if (Physics.SphereCast(ray, 0.5f, out hitInfo, localCameraMaxDistance_Z, Utility.ObstacleLayer))
        {
            float projectionLength = Vector3.Dot (hitInfo.point - ray.origin,ray.direction.normalized);
            maxCameraLocal_Z = projectionLength;
            cameraPos.localPosition = Vector3.back * maxCameraLocal_Z;
        }

        z_abs = -cameraPos.localPosition.z;

        z_abs =Mathf.Lerp(z_abs,maxCameraLocal_Z , localCameraDistanceSmooth* Time.deltaTime);
        cameraPos.localPosition = z_abs * Vector3.back;

        if (followTarget != null)
            transform.position = Vector3.Lerp(transform.position, followTarget.position + followOffset, followSmooth * Time.deltaTime);
	}


}
