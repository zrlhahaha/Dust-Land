using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : PoolMember {

    public ParticleSystem[] particles;
    private void Update()
    {
        for (int i = 0; i < particles.Length; i++)
            if (particles[i].IsAlive())
                return;

        ReturnPool();
    }

    public void Play(Vector3 position,Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        foreach (ParticleSystem go in particles)
            go.Play();
    }

    public void Play()
    {
        foreach (ParticleSystem go in particles)
            go.Play();
    }

    public override void Reset()
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Clear();
    }
}
