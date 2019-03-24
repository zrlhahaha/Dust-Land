using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour {
    public static bool hideCursor = false;

    private void Awake()
    {
        LockCorsor();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!hideCursor)
            {
                hideCursor = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                hideCursor = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
	}

    public static void LockCorsor()
    {
        hideCursor = true;

        hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void UnlockCoursor()
    {
        hideCursor = false;

        hideCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
