using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworking : NetworkBehaviour {

    public NetworkTransformChild[] networkTransformChilds;


    [Command]
    public void CmdInstantiatePoolMember_Rigid(uint netId, int index, Vector3 pos, Vector3 velocity)
    {
        RpcInstantiatePoolMember_Rigid(netId, index, pos, velocity);
    }

    [Command]
    public void CmdInstantiatePoolMember_RigidWithAngularVelocity(uint netId, int index, Vector3 pos, Vector3 desireVelocity, Vector3 desireAngularVelocity)
    {
        RpcInstantiatePoolMember_RigidWithVelocity(netId, index, pos, desireVelocity, desireAngularVelocity);
    }

    [ClientRpc]
    public void RpcInstantiatePoolMember_Rigid(uint netId, int index, Vector3 pos, Vector3 velocity)
    {
        if (netId == CustomNetworkManager._instance.localPlayerNetid)
            return;

        GameObject go = ResourceManager._instance.GetPoolMember(index);
        go.transform.position = pos;
        Rigidbody rigid = go.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = velocity;
        }
    }

    [ClientRpc]
    public void RpcInstantiatePoolMember_RigidWithVelocity(uint netId, int index, Vector3 pos, Vector3 desireVelocity, Vector3 desireAngularVelocity)
    {
        if (netId == CustomNetworkManager._instance.localPlayerNetid)
            return;

        GameObject go = ResourceManager._instance.GetPoolMember(index);
        go.transform.position = pos;
        Rigidbody rigid = go.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            print(rigid.velocity);

            rigid.velocity = Vector3.right;
            rigid.velocity = desireAngularVelocity;

            print(desireVelocity);
            print(rigid.velocity);
        }


    }


    [Command]
    public void CmdSyncModule( int wheelIndex,int[] equipmentIndexs)
    {
        RpcSyncModule( equipmentIndexs);
        RpcSyncWheel(wheelIndex);
    }

    [ClientRpc]
    public void RpcSyncWheel(int wheelIndex)
    {
        ItemInfo target = Inventory._instance.GetItemInfo(wheelIndex);


        GetComponent<VehicleController>().ChangeWheel(target);
    }

    [ClientRpc]
    public void RpcSyncModule( int[] equipmentIndexs)
    {
        //AutoWeapon autoWeapon;

        WeaponHandle[] playerWeaponHandles = GetComponent<WeaponSystem>().weaponHandles;
        for (int i = 0; i < equipmentIndexs.Length; i++)
        {
            //同步Module
            if (transform != CustomNetworkManager._instance.localPlayer.transform)
            {
                if (equipmentIndexs[i] == -1)
                    continue;

                WeaponBase module = playerWeaponHandles[i].ChangeModule(Inventory._instance.GetItemInfo(equipmentIndexs[i]));
                module.Disable();
            }

            //更新同步组件
            //autoWeapon = playerWeaponHandles[i].weapon as AutoWeapon;
            NetworkTransformChild sync_1 = networkTransformChilds[2 * i];
            NetworkTransformChild sync_2 = networkTransformChilds[2 * i + 1];

            //if (autoWeapon != null)
            //{
            //    sync_1.enabled = true;
            //    sync_2.enabled = true;
            //    sync_1 = networkTransformChilds[2 * i];
            //    sync_2 = networkTransformChilds[2 * i + 1];
            //    sync_1.target = autoWeapon.holderPivot;
            //    sync_1.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;
            //    sync_2.target = autoWeapon.weaponPivot;
            //    sync_2.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisX;
            //}
            //else
            //{
            //    //if (weaponSync[2 * i] != null)
            //    //    Destroy(weaponSync[2 * i]);

            //    //if (weaponSync[2 * i + 1] != null)
            //    //    Destroy(weaponSync[2 * i + 1]);
            //    sync_1.enabled = false;
            //    sync_2.enabled = false;
            //}
        }
    }

    [Command]
    public void CmdTakeDamage(uint netId, float damage)
    {
        Property go = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<Property>();
        float desireHp = go.hp - damage;

        if (desireHp > 0)
            RpcTakeDamage(netId, damage);
        else
            RpcDisablePlayer(netId);
    }

    [ClientRpc]
    public void RpcTakeDamage(uint netId, float damage)
    {
        Property go = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<Property>();
        go.TakeDamage(damage);
    }

    [ClientRpc]
    public void RpcDisablePlayer(uint netId)
    {
        Property go = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<Property>();
        go.Disable();
    }

    [Command]
    public void CmdRespawn(uint netId)
    {
        Property go = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<Property>();

        if (!go.active)
        RpcRespawn(netId);

    }

    [ClientRpc]
    public void RpcRespawn(uint netId)
    {
        CustomNetworkManager._instance.GetPlayer(netId).GetComponent<Property>().Respawn();
    }

    [Command]
    public void CmdSyncRacastShooterGraphic(uint netId,int moduleIndex,Vector3 start,Vector3 end, Vector3 normal, bool isHit,HitEffect hitEffect)
    {
        RpcSyncRacastShootGraphic(netId,moduleIndex,start,end,normal,isHit,hitEffect);
    }

    [ClientRpc]
    public void RpcSyncRacastShootGraphic(uint netId, int weaponHandleIndex,Vector3 start,Vector3 end,Vector3 normal,bool isHit,HitEffect hitEffect)
    {
        if (netId == CustomNetworkManager._instance.localPlayerNetid)
            return;

        WeaponSystem weaponSystem = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<WeaponSystem>();
        RaycastShooter weapon = weaponSystem.weaponHandles[weaponHandleIndex].module as RaycastShooter;
        StartCoroutine(weapon.DisplayGraphic(start, end,normal,isHit,hitEffect));
    }

    [Command]
    public void CmdSyncShootgunGraphic(uint netId, int moduleIndex, Vector3 start, Vector3[] end, Vector3[] normal, bool[] isHits,  HitEffect[] hitEffects)
    {
        RpcSyncShootgunGraphic(netId, moduleIndex, start, end, normal,isHits, hitEffects);
    }

    [ClientRpc]
    public void RpcSyncShootgunGraphic(uint netId, int weaponHandleIndex, Vector3 start, Vector3[] end, Vector3[] normal, bool[] isHits,HitEffect[] hitEffects)
    {
        if (netId == CustomNetworkManager._instance.localPlayerNetid)
            return;

        WeaponSystem weaponSystem = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<WeaponSystem>();
        Shotgun weapon = weaponSystem.weaponHandles[weaponHandleIndex].module as Shotgun;
        weapon.DisplayGraphic(start, end, normal, isHits,hitEffects);
    }

    [Command]
    public void CmdSyncColliderShooterGraphic(uint netId, int weaponHandleIndex, Vector3 colliderPos, Quaternion colliderRot,Vector3 velocity)
    {
        RpcSyncColliderShooterGraphic(netId, weaponHandleIndex, colliderPos, colliderRot,velocity);
    }

    [ClientRpc]
    public void RpcSyncColliderShooterGraphic(uint netId, int weaponHandleIndex,Vector3 colliderPos,Quaternion colliderRot,Vector3 velocity)
    {
        if (netId == CustomNetworkManager._instance.localPlayerNetid)
            return;

        WeaponSystem weaponSystem = CustomNetworkManager._instance.GetPlayer(netId).GetComponent<WeaponSystem>();
        ColliderShooter weapon = weaponSystem.weaponHandles[weaponHandleIndex].module as ColliderShooter;
        weapon.DisplayGraphic(colliderPos, colliderRot,velocity);
    }

    public void SwitchVehicle(int itemId, Vector3 pos, Quaternion rot)
    {
        CmdSwitchVehicle(itemId, pos, rot);
    }

    [Command]
    public void CmdSwitchVehicle(int itemId, Vector3 pos, Quaternion rot)
    {
        ItemInfo target = Inventory._instance.GetItemInfo(itemId);
        if (target.itemType != ItemType.Vehicle)
        {
            Debug.LogError("gived item is not vehicle");
            return;
        }
        GameObject go = Instantiate(target.prefab, pos, rot);
        NetworkServer.DestroyPlayersForConnection(connectionToClient);
        NetworkServer.AddPlayerForConnection(connectionToClient, go, 0);
    }




}
