using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : PoolMember {

    public Rigidbody rigid;
    public float lifeTime = 5;
    public float timer;

    public void Trigger(Vector3 position,Quaternion rotation,Vector3 velocity,Vector3 angularVelocity)
    {
        transform.position = position;
        transform.rotation = rotation;
        rigid.velocity = velocity;
        rigid.angularVelocity = angularVelocity;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer>lifeTime)
            ReturnPool();
    }

    public override void Reset()
    {
        timer = 0;
    }

}
