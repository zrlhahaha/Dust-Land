using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FXContainer{

    public FX[] prefabs;

    public void Play(Vector3 position, Quaternion rotation)
    {
        if (prefabs == null || prefabs.Length == 0)
            return;

        for (int i = 0; i < prefabs.Length; i++)
        {
            FX go = ResourceManager._instance.GetPoolMember(prefabs[i]);
            go.Play(position, rotation);
        }
    }

}
