using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static int playerToControl = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerToControl = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerToControl = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerToControl = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerToControl = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerToControl = 5;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Switch between players by number order
        }
    }
}
