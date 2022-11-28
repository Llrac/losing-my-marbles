using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    GridManager grid;
    void Update()
    {
        grid = FindObjectOfType<GridManager>();
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.W))
        {
           // if(grid.IsSquareEmpty(new Vector2(0, 1)) == 1)
                Move(gameObject, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(gameObject, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(gameObject, 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(gameObject, 1, 1);
        }
    }
}
