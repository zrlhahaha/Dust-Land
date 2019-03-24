using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHandleGridInfo : ModuleUI {

    public WeaponHandle weaponHandle;
    public Image icon;
    public Image serie;
    public Image type;
    public Image quality;
    public Image upperBar;
    public Button btn_Unequip;
    public Button btn_Configure;
    public Text upperbarText;


    public override void ChangeItem(ItemInfo newItem)
    {
        if (newItem!=null&&newItem.itemType != weaponHandle.moduleRequestType)
            return;

        itemInfo = newItem;
        weaponHandle.ChangeWeapon(newItem);
        Refresh();
    }

    public override void SetUI(ItemInfo item)
    {
        itemInfo = item;

        Refresh();
    }

    public override void Refresh()
    {
        upperbarText.text = weaponHandle.moduleRequestType.ToString();

        if (itemInfo == null)
        {
            DisableUI();
            return;
        }

        if (itemInfo.sprite_icon != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = itemInfo.sprite_icon;
        }
        else
            icon.gameObject.SetActive(false);

        if (itemInfo.sprite_serie != null)
        {
            serie.sprite = itemInfo.sprite_serie;
            serie.gameObject.SetActive(true);
        }
        else
            serie.gameObject.SetActive(false);

        if (itemInfo.sprite_type != null)
        {
            type.gameObject.SetActive(true);
            type.sprite = itemInfo.sprite_type;
        }
        else
            type.gameObject.SetActive(false);

        upperBar.gameObject.SetActive(true);

        btn_Unequip.gameObject.SetActive(true);
        btn_Configure.gameObject.SetActive(true);

    }

    void DisableUI()
    {
        icon.gameObject.SetActive(false);
        serie.gameObject.SetActive(false);
        type.gameObject.SetActive(false);
        quality.gameObject.SetActive(false);
        btn_Unequip.gameObject.SetActive(false);
        btn_Configure.gameObject.SetActive(false);
    }

    public void OnClicked_Unequip()
    {
        ChangeItem(null);
    }

    public void OnClicked_Configure()
    {
        Garage._instance.ChangeWeaponHandleTriggerKey(weaponHandle);
    }

}
