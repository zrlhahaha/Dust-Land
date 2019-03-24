using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CustomNetworkManager : NetworkBehaviour {

    public static CustomNetworkManager _instance;

    public Dictionary<uint, NetworkIdentity> playerDict = new Dictionary<uint, NetworkIdentity>();
    public List<GameObject> spawnablePrefabs = new List<GameObject>(); 

    public uint localPlayerNetid;
    public NetworkIdentity localPlayer;
    public PlayerNetworking localPlayerNetworking;
    public bool localPlayerActive = true;
    public bool firstRegisterLocalPlayer = true;
    public int localPlayerLayer;


    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("There has another networkManager");

        _instance = this;
    }

    private void Update()
    {
        if (!localPlayerActive && Input.GetKeyDown(KeyCode.Space))
        {
            localPlayer.GetComponent<PlayerNetworking>().CmdRespawn(localPlayerNetid);
        }
    }

    public void RegisterPlayer(NetworkIdentity playerNetworkIdentity, bool isLocalPlayer = false)
    {
        if (isLocalPlayer)
        {
            localPlayer = playerNetworkIdentity;
            localPlayerNetworking = playerNetworkIdentity.GetComponent<PlayerNetworking>();
            localPlayerNetid = playerNetworkIdentity.netId.Value;
            playerNetworkIdentity.transform.name = "LocalPlayer";

            Utility.SetLayerRecursively(localPlayer.transform, localPlayerLayer);

            OnRegisterLocalPlayer(playerNetworkIdentity);
            if (firstRegisterLocalPlayer == true)
            {
                firstRegisterLocalPlayer = false;
                OnFirstRegisterLocalPlayer(playerNetworkIdentity);
            }
        }
        
        playerDict.Add(playerNetworkIdentity.netId.Value, playerNetworkIdentity);
    }

    public void OnRegisterLocalPlayer(NetworkIdentity localPlayer)
    {
        Garage._instance.RestartEditVehicle(localPlayer.transform);
        CameraSetup._instance.SetTpCameraTarget(localPlayer.transform.Find("TPCameraFocus"));
    }

    /// <summary>
    /// 待修改
    /// </summary>
    /// <param name="localPlayer"></param>
    public void OnFirstRegisterLocalPlayer(NetworkIdentity localPlayer)
    {
        CameraSetup._instance.ActiveTPCamera(localPlayer.transform.Find("TPCameraFocus"));
    }

    public  void LogOutPlayer(uint netId)
    {
        playerDict.Remove(netId);
    }

    public  NetworkIdentity GetPlayer(uint netId)
    {
        return playerDict[netId];
    }

    

}
