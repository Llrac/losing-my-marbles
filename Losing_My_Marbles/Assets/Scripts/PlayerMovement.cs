using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdatePlayerProperties(gameObject, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            UpdatePlayerProperties(gameObject, -1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdatePlayerProperties(gameObject, 1, 1);
        }
    }
}
