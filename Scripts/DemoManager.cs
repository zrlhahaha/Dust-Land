using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour {

    public GameObject hint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            hint.SetActive(!hint.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();                
    }

}
