using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static int characterToControl = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            characterToControl = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterToControl = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            characterToControl = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            characterToControl = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            characterToControl = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            characterToControl = 6;
        }
    }
}
