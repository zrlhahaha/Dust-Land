using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelUI : ModuleUI {

    public Image icon;

    public override void ChangeItem(ItemInfo newItem)
    {
        if (newItem.itemType != ItemType.Wheel)
            return;

        itemInfo = newItem;
        Garage._instance.vehicle.ChangeWheel(newItem);
        Refresh();
    }

    public override void SetUI(ItemInfo item)
    {
        itemInfo = item;
        Refresh();
    }

    public override void Refresh()
    {

        if (itemInfo == null)
            return;

        if (itemInfo.sprite_icon != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = itemInfo.sprite_icon;
        }
        else
            icon.gameObject.SetActive(false);
        
    }


}
