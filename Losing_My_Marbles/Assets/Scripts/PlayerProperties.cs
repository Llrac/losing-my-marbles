using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(gameObject, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(gameObject, 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(gameObject, 1, 1);
        }
    }
    public override char ChangeTag()
    {
        return 'P';
    }
}
