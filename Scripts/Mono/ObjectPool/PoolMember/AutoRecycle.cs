using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecycle : PoolMember {

    public float lifeTime = 10f;
    float timer;

    public override void Reset()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time > timer + lifeTime)
        {
            ReturnPool();
            timer = 0;
        }
    }

}
