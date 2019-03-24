using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUI : ModuleUI {

    public Image icon;
    public Text text;

    public override void ChangeItem(ItemInfo newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("null is not expect");
            return;
        }

        if (newItem.itemType != ItemType.Vehicle)
        {
            return;
        }

        itemInfo = newItem;
        Garage._instance.ChangeVehicle(newItem);
        Refresh();
    }

    public override void SetUI(ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
        Refresh();
    }

    //根据自己的itemInfo刷新UI
    public override void Refresh()
    {
        if (itemInfo == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }
        text.text = itemInfo.itemName;

        if (itemInfo.sprite_icon != null)
        {
            icon.sprite = itemInfo.sprite_icon;
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

}
