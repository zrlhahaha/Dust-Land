using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    public NetworkIdentity networkIdentity;


    void Start () {
        if (isLocalPlayer)
        {
            CustomNetworkManager._instance.RegisterPlayer(networkIdentity,true);
        }
        else
        {
            CustomNetworkManager._instance.RegisterPlayer(networkIdentity);
        }
    }

    private void OnDestroy()
    {
        CustomNetworkManager._instance.LogOutPlayer(networkIdentity.netId.Value);
    }
}
