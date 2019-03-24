using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detachable : PoolMember {

    public float lifeTime = 5f;

    public Rigidbody[] rigids;
    Vector3[] localPositions;
    Quaternion[] localRotations;
    float timer;

    private void Awake()
    {
        localPositions = new Vector3[rigids.Length];
        localRotations = new Quaternion[rigids.Length];
        for (int i = 0; i < rigids.Length; i++)
        {
            localPositions[i] = rigids[i].transform.localPosition;
            localRotations[i] = rigids[i].transform.localRotation;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
            ReturnPool();
    }

    

    public override void Reset()
    {
        for (int i = 0; i < rigids.Length; i++)
        {
            rigids[i].transform.localPosition = localPositions[i];
            rigids[i].transform.localRotation = localRotations[i];
            rigids[i].velocity = Vector3.zero;
        }
        timer = 0;
    }
}
