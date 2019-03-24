using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public float posSmooth = 2f;
    public float rotSmooth = 5f;

    public List<Transform> followPoints = new List<Transform>();
    public List<Transform> focusPoints = new List<Transform>();
    public int followPointIndex;


    void Start () {
	}
	
	void FixedUpdate () {
        Vector3 desirePos = Vector3.Lerp(transform.position, followPoints[followPointIndex].position, posSmooth*Time.deltaTime);
        transform.position = desirePos;

        Quaternion desireRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(focusPoints[followPointIndex].position- followPoints[followPointIndex].position), rotSmooth*Time.deltaTime);
        transform.rotation = desireRot;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            followPointIndex++;

            if (followPointIndex >= followPoints.Count)
                followPointIndex = 0;
        }
    }
}
