using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    GridManager grid;

    public Vector2 gridPosition = new(0, 0);
    public Vector2 requestedGridPosition = new(0, 0);
    public Sprite[] sprites;
    SpriteRenderer sr;

    public float jumpLength = 1;

    public int currentDirectionID = 0;
    int integer;

    GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Sprite")
                body = child.gameObject;
        }
        
        sr = body.GetComponent<SpriteRenderer>();

        Move(gameObject, 1, 0);
    }

    public void Move(GameObject character, int dataID, int increment)
    {
        // increment should not change when moving
        if (dataID != 0)
            currentDirectionID += increment;

        if (currentDirectionID <= -4 || currentDirectionID >= 4)
        {
            currentDirectionID = 0;
        }

        
        // dataID 0 concerns transform position
        // dataID 1 concerns character rotation

        // Set transform position
        if (dataID == 0)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                if (grid.IsSquareEmpty(RequestGridPosition(currentDirectionID)) == 1)
                {
                    integer = 1;

                    if (increment < 0)
                    {
                        integer *= -1;
                    }
                    switch (currentDirectionID)
                    {
                        case 0:
                            character.transform.position += new Vector3(jumpLength, jumpLength / 2, 0) * integer;
                            grid.MoveInGridMatrix(this.gameObject, RequestGridPosition(currentDirectionID));
                            break;
                        case 1 or -3:
                            character.transform.position += new Vector3(jumpLength, -jumpLength / 2, 0) * integer;
                            grid.MoveInGridMatrix(this.gameObject, RequestGridPosition(currentDirectionID));
                            break;
                        case 2 or -2:
                            character.transform.position += new Vector3(-jumpLength, -jumpLength / 2, 0) * integer;
                            grid.MoveInGridMatrix(this.gameObject, RequestGridPosition(currentDirectionID));
                            break;
                        case 3 or -1:
                            character.transform.position += new Vector3(-jumpLength, jumpLength / 2, 0) * integer;
                            grid.MoveInGridMatrix(this.gameObject, RequestGridPosition(currentDirectionID));
                            break;
                    }
                }
            }

        }

        // Set character rotation
        if (dataID == 1)
        {
            switch (currentDirectionID)
            {
                case 0:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1 or -3:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2 or -2:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3 or -1:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }

    public Vector2 RequestGridPosition(int currentDirectionID)
    {
        switch (currentDirectionID)
        {
            case 0:
                return new Vector2(0, 1);
              
            case 1 or -3:
                return new Vector2(1, 0);
               
            case 2 or -2:
                return new Vector2(0, -1);
              
            case 3 or -1:
                return new Vector2(-1, 0);

        }
        return new Vector2(0, 0);
    }

}
