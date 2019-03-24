using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test :MonoBehaviour{

    public VehicleController vc;
    public ItemInfo wheel;
    public bool check = false;
    void Update()
    {
        if (check)
        {
            check = false;
            vc.ChangeWheel(wheel);
        }
    }

}
