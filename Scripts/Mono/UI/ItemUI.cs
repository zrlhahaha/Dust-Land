using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour {
    public ItemInfo itemInfo;

    public virtual void  ChangeItem(ItemInfo newItem)
    {
    }

    public virtual void Refresh()
    {

    }
}
