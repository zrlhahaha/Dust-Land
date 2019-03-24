using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemGridInfo : ItemUI {

    public Text itemName;
    public Image icon;
    public Image serie;
    public Image type;
    public Image bg;                //只有bg是raycastTarget

    public Vector3 originalPos;
    public Transform originalParent;
    public override void ChangeItem(ItemInfo newItem)
    {
        if (newItem == null)
            return;
        itemInfo = newItem;

        Refresh();
    }

    public override void Refresh()
    {
        itemName.text = itemInfo.itemName;

        if (itemInfo.sprite_icon != null)
            icon.sprite = itemInfo.sprite_icon;
        else
            icon.gameObject.SetActive(false);

        if (itemInfo.sprite_serie != null)
            serie.sprite = itemInfo.sprite_serie;
        else
            serie.gameObject.SetActive(false);

        if (itemInfo.sprite_type != null)
            type.sprite = itemInfo.sprite_type;
        else
            type.gameObject.SetActive(false);
    }

    public void OnDrag_MoveGrid()
    {
        transform.position = Input.mousePosition;
    }

    public void OnBeginDrag_SavePos()
    {
        originalParent = transform.parent;
        transform.SetParent(Garage._instance.garagePanel);
        originalPos = transform.position;
        BlockRaycast(false);
    }

    public void OnEndDrag_Apply()
    {
        if (Garage._instance.pointerOnModuleUI != null)
            Garage._instance.pointerOnModuleUI.ChangeItem(itemInfo);

        transform.position = originalPos;
        transform.SetParent(originalParent);
        BlockRaycast(true);
    }

    public void OnPointerEnter()
    {
        Garage._instance.pointerOnItemGridInfo = this;
    }

    public void OnPointerOut()
    {
        Garage._instance.pointerOnItemGridInfo = null;
    }

    public void BlockRaycast(bool state)
    {
        bg.raycastTarget = state;
    }
}
