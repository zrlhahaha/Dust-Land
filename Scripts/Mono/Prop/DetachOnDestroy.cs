using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachOnDestroy : MonoBehaviour {

    public Detachable detachPrefab;

    public Detachable Detach()
    {
        Detachable go = ResourceManager._instance.GetPoolMember(detachPrefab);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        return go;
    }

}
