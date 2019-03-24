using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisableComponetOnRemotePlayer : NetworkBehaviour {

    public Behaviour[] disableOnStart;


    void Start () {

        if (isLocalPlayer)
            return;


        for (int i = 0; i < disableOnStart.Length; i++)
        {
            disableOnStart[i].enabled = false;
        }
    }

}
