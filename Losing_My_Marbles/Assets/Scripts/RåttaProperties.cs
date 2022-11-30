using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(gameObject, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(gameObject, 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(gameObject, 1, 1);
        }
    }
    public override char ChangeTag()
    {
        return 'E';
    }
}