using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle : MonoBehaviour {

    public WeaponBase weapon;
    public ItemInfo currentItem;
    public ItemType moduleRequestType = ItemType.PrimaryWeapon;
    public KeyCode triggerKey = KeyCode.Mouse0;

    public WeaponBase ChangeWeapon(ItemInfo weaponInfo)
    {
        if (weaponInfo == null)
        {
            DestroyWeapon();
            return null;
        }

        if (weaponInfo == currentItem)
            return weapon;

        if (weaponInfo.itemType != moduleRequestType)
            return null;

        if (weapon!=null)
            Destroy(weapon.gameObject);

        currentItem = weaponInfo;

        GameObject go = Instantiate(weaponInfo.prefab,transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        WeaponBase newModule = go.GetComponent<WeaponBase>();
        weapon = newModule;

        if (newModule == null)
            Debug.LogError("New Module Invalid");

        return weapon;
    }

    public void DestroyWeapon()
    {
        if (weapon == null)
            return;

        Destroy(weapon.gameObject);
        weapon = null;
        currentItem = null;
    }
}
