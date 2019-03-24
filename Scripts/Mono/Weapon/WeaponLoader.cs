using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponSystem))]
public class WeaponLoader : MonoBehaviour {

    public WeaponSystem weaponSystem;
    public ItemInfo[] weapon;
    public bool check = false;
	
	void Update () {
        if (check)
        {
            check = false;

            int length = Mathf.Min(weaponSystem.weaponHandles.Length, weapon.Length);
            for (int i = 0; i < length; i++)
            {
                weaponSystem.weaponHandles[i].ChangeWeapon( weapon[i]);
            }
        }
	}
}
