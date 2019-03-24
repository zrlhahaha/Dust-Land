using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Garage : MonoBehaviour {
    public VehicleController vehicle;
    public WeaponSystem weaponSystem;
    public PlayerVehicleControl playerVehicleInput;

    public RectTransform[] typePannel;
    public List<ItemGridInfo> itemGridInfoList;
    public List<WeaponHandleGridInfo> weaponHandleGridInfo;
    public VehicleUI vehicleUI;
    public WheelUI wheelUI;

    public RectTransform itemGridContent;
    public RectTransform WeaponHandleGridContent;
    public RectTransform garagePanel;
    public RectTransform selectOutLine;
    public RectTransform PressFireKey;
    public ItemGridInfo itemGridPrefab;
    public WeaponHandleGridInfo weaponHandleGridPrefab;

    public ModuleUI pointerOnModuleUI;
    public ItemGridInfo pointerOnItemGridInfo;

    public Transform zoneCheck_1;
    public Transform zoneCheck_2;
    public Transform carStopPos;
    public Transform cameraPos;

    public bool editingVehicle = false;

    public static Garage _instance;
    void Start()
    {
        _instance = this;

        ReadInventory();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))// && Utility.ZoneCheck(Gobal._instance.player.position,zoneCheck_1.position, zoneCheck_2.position))
        {
            StartEditVehicle();
        }
        if (pointerOnItemGridInfo != null)
        {
            selectOutLine.position = pointerOnItemGridInfo.transform.position;
            selectOutLine.gameObject.SetActive(true);
        }
        else
            selectOutLine.gameObject.SetActive(false);

        if(editingVehicle)
            weaponSystem.SetTarget(vehicle.transform.position + vehicle.transform.forward * 100);
    }

    void ReadInventory()
    {
        foreach (ItemInfo item in Inventory._instance.itemInfos)
            AddItemInfo(item);
    }

    public void StartEditVehicle()
    {
        editingVehicle = true;
        LockCursor.UnlockCoursor();
        Player._instance.playerProperty.hp = Player._instance.playerProperty.maxHp;

        vehicle = Player._instance.playerVehicle;
        weaponSystem = Player._instance.playerWeaponSystem;
        playerVehicleInput = Player._instance.playerVehicleControl;

        vehicle.transform.position = carStopPos.position;
        vehicle.transform.rotation = carStopPos.rotation;
        vehicle.StopCar();

        playerVehicleInput.enabled = false;

        Player._instance.SetMainCamera(cameraPos);

        garagePanel.gameObject.SetActive(true);
        InitModuleUI();
    }

    public void StopEditVehicle()
    {
        editingVehicle = false;
        playerVehicleInput.enabled = true;


        Player._instance.SetMainCameraToPlayerVehicle();
        LockCursor.LockCorsor();

        garagePanel.gameObject.SetActive(false);
    }

    public void InitModuleUI()
    {
        InitWeaponHandleUI();
        vehicleUI.SetUI(vehicle.vehicleItemInfo);
        wheelUI.SetUI(vehicle.wheelItemInfo);
    }

    public void InitWeaponHandleUI()
    {
        int offset =weaponSystem.weaponHandles.Length - weaponHandleGridInfo.Count;
        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                WeaponHandleGridInfo go = Instantiate(weaponHandleGridPrefab, WeaponHandleGridContent);
                weaponHandleGridInfo.Add(go);
            }
        }
        else if(offset<0)
        {
            offset = -offset;
            for (int i = 0; i < offset; i++)
            {
                int index = weaponHandleGridInfo.Count - 1;
                Destroy(weaponHandleGridInfo[index].gameObject);
                weaponHandleGridInfo.RemoveAt(index);
            }
        }

        for (int i = 0; i < weaponSystem.weaponHandles.Length; i++)
        {
            weaponHandleGridInfo[i].weaponHandle = weaponSystem.weaponHandles[i];
            weaponHandleGridInfo[i].SetUI(weaponSystem.weaponHandles[i].currentItem);
        }
    }

    public void OnClicked_ChangePanel(GameObject target)
    {
        foreach (RectTransform go in typePannel)
        {
            if (go.gameObject == target)
                go.gameObject.SetActive(true);
            else
                go.gameObject.SetActive(false);
        }
    }

    public void AddItemInfo(ItemInfo newItem)
    {
        ItemGridInfo go = Instantiate(itemGridPrefab, typePannel[(int)newItem.itemType]);
        itemGridInfoList.Add(go);
        go.ChangeItem(newItem);
    }
    

    public void StartDragVehicle()
    {
        garagePanel.gameObject.SetActive(false);
    }

    public void StopDrageVehicle()
    {
        garagePanel.gameObject.SetActive(true);
    }

    public void ChangeWeaponHandleTriggerKey(WeaponHandle weaponHandle)
    {
        PressFireKey.gameObject.SetActive(true);
        StartCoroutine(Corountine_ChangeWeaponKey(weaponHandle));
    }

    public IEnumerator Corountine_ChangeWeaponKey(WeaponHandle weaponHandle)
    {
        while (true)
        {
            if (Input.anyKeyDown)
                break;

            yield return null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressFireKey.gameObject.SetActive(false);
            yield break;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                weaponHandle.triggerKey = key;
                PressFireKey.gameObject.SetActive(false);
                yield break;
            }
        }
    }

    public void ChangeVehicle(ItemInfo newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("target item is not suppoest to be null");
            return;
        }

        if (newItem.itemType != ItemType.Vehicle)
        {
            Debug.LogError("target item is not a vehicle");
            return;
        }

        if (newItem == vehicle.vehicleItemInfo)
            return;

        Destroy(vehicle.gameObject);
        GameObject go = Instantiate(newItem.prefab);
        Player._instance.SetPlayerVehicle(go.transform);
        Player._instance.SetMainCameraToPlayerVehicle();
        StartEditVehicle();
    }
}
