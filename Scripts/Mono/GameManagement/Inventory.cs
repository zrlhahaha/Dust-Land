using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<ItemInfo> itemInfos = new List<ItemInfo>();
    public Dictionary<int, ItemInfo> gobalItemInfoDict = new Dictionary<int, ItemInfo>();


    public static Inventory _instance;

    public void Awake()
    {
        _instance = this;

        for (int i = 0; i < itemInfos.Count; i++)
            gobalItemInfoDict.Add(i,itemInfos[i]);
    }

    public ItemInfo GetItemInfo(int itemIndex)
    {
        if (itemIndex == -1)
            return null;

        return gobalItemInfoDict[itemIndex];
    }

    public int GetItemInfoIndex(ItemInfo itemInfo)
    {
        if (itemInfo == null)
            return -1;
        return itemInfos.IndexOf(itemInfo);
    }

}
