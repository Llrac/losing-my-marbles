using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float dPadx;

    private void Update()
    {
        if (Input.GetButtonDown("PS4_X"))
        {
            Debug.Log("X was pressed");
        }
        if (Input.GetButtonDown("PS4_Square"))
        {
            Debug.Log("Square was pressed");
        }

        dPadx = Input.GetAxisRaw("DPAD_Horizontal");
        if(dPadx != 0)
        {
            Debug.Log(dPadx);
        }
    }
}
