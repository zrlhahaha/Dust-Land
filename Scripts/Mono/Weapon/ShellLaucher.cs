using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellLaucher : MonoBehaviour {

    public float maxSpeed = 6f;
    public float minSpeed = 4f;
    public float maxAngularSpeed = 30f;
    public float minAngularSpeed = 20f;
    public float launchDirectionOffset = 0.1f;

    Rigidbody rootRigidbody;
    public Shell shellPrefab;
    public Transform launchPoint;

    public void Start()
    {
        Transform rootTransform = Utility.GetRootTransform(transform);
        rootRigidbody = rootTransform.GetComponent<Rigidbody>();

        if (rootRigidbody == null)
            Debug.LogError("Something Wrong With Root GameObject RigidBody");
    }

    public void Trigger()
    {
        Shell shell = ResourceManager._instance.GetPoolMember(shellPrefab);

        Vector2 randomVec2 = Random.insideUnitCircle*launchDirectionOffset;
        Vector3 vec = new Vector3(1, randomVec2.y, randomVec2.x);
        Vector3 offsetVelocity = Random.Range(maxSpeed, minSpeed) * launchPoint.TransformDirection(vec);
        Vector3 offsetAngularVelocity = Random.Range(maxAngularSpeed, minAngularSpeed) * new Vector3(randomVec2.x,1,randomVec2.y);

        if (rootRigidbody != null)
            offsetVelocity += rootRigidbody.velocity;

        shell.Trigger(launchPoint.position, launchPoint.rotation, offsetVelocity, offsetAngularVelocity);
    }
}
