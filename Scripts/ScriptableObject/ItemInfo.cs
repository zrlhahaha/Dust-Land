using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType : int
{
    Vehicle =0,
    PrimaryWeapon =1,
    SecondaryWeapon = 2,
    Wheel =3,
    Bumper = 4,
}

[CreateAssetMenu(fileName ="new ItemInfo",menuName = "ScriptableObject/ItemInfo")]
public class ItemInfo : ScriptableObject {

    public string itemName;
    public GameObject prefab;
    public Sprite sprite_icon;
    public Sprite sprite_serie;
    public Sprite sprite_type;
    public ItemType itemType;
}
