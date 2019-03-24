using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {

    public WeaponHandle[] weaponHandles;

    public void SetTarget(Vector3 target)
    {
        for (int i = 0; i < weaponHandles.Length; i++)
        {
            if (weaponHandles[i].weapon != null)
            {
                weaponHandles[i].weapon.SetTarget(target);
            }
        }
    }

    public bool Trigger()
    {
        bool triggerd = false;
        for (int i = 0; i < weaponHandles.Length; i++)
        {
            if (weaponHandles[i].weapon == null)
                continue;

            if (weaponHandles[i].weapon.Trigger())
                triggerd = true;
        }
        return triggerd;
    }

    public void Trigger(int i)
    {
        weaponHandles[i].weapon.Trigger();
    }

    public void Enable(bool state)
    {
        for (int i = 0; i < weaponHandles.Length; i++)
        {
            weaponHandles[i].weapon.Enable(state);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Property target = Utility.GetRootTransform(collision.collider.transform).GetComponent<Property>();
        if (target == null)
            return;

        foreach (var go in weaponHandles)
            if (go.weapon != null)
                go.weapon.OnImpactOtherEntity(collision, target);
    }


}
