using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Character
{
    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdateData(gameObject, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UpdateData(gameObject, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateData(gameObject, 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateData(gameObject, 1, 1);
        }
    }
}
