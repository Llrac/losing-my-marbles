using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public Vector2 gridPosition = new(0, 0);

    public static List <Movement> enemies = new ();

    public int currentDirectionID = 0;

    public Sprite[] sprites = new Sprite[2];
    SpriteRenderer sr;
    GridManager grid;

    public float jumpLength = 1;
 
    int multiplier;

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
        

    }


    public void TryMove(GameObject character, int dataID, int increment)
    {
       
        //if (callersDirectionID != 5)
        //{
        //    currentDirectionID = callersDirectionID;
        //}
        

        // Set transform position
        if (dataID == 0)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                if(grid == null)
                {
                    grid = FindObjectOfType<GridManager>();
                }
                switch (grid.IsSquareEmpty(character, RequestGridPosition(currentDirectionID)))
                {
                    case 0: // EMPTY (walls, void, etc)
                        TryMove(character, 1, 2); // l�gg till recursion h�r
                        break;
                    case 1: // WALKABLEGROUND
                        Move(character, 1);
                        break;
                    case 2: // PLAYER
                       
                        // Move(character, increment);
                        break;
                    case 3: // ENEMY
                        GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID) + gridPosition, enemies);
                        TryMove(enemy,0, 1);
                        TryMove(character, 0, 1);
                        break;
                }
            }
        }

        // Set character rotation
        if (dataID == 1)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                multiplier = 1;
                if (increment < 0)
                {
                    multiplier *= -1;
                }
                currentDirectionID += multiplier;
                if (currentDirectionID <= -4 || currentDirectionID >= 4)
                {
                    currentDirectionID = 0;
                }
            }

            foreach (Transform child in transform)
            {
                if (child.gameObject.name == "Sprite")
                    body = child.gameObject;
                sr = body.GetComponent<SpriteRenderer>();
            }

            switch (currentDirectionID)
            {
                case 0:
                   
                    character.GetComponent<Movement>().sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1 or -3:
                   
                    character.GetComponent<Movement>().sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2 or -2:
                    
                    character.GetComponent<Movement>().sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3 or -1:
                    
                    character.GetComponent<Movement>().sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }

    public Vector2 RequestGridPosition(int currentDirectionID)
    {
        return currentDirectionID switch
        {
            0 => new Vector2(0, 1),
            1 or -3 => new Vector2(1, 0),
            2 or -2 => new Vector2(0, -1),
            3 or -1 => new Vector2(-1, 0),
            _ => new Vector2(0, 0),
        };
    }

    public void Move(GameObject character, int increment)
    {
        multiplier = 1;
        if (increment < 0)
        {
            multiplier *= -1;
        }
        switch (currentDirectionID)
        {
            case 0:
                character.transform.position += new Vector3(jumpLength, jumpLength / 2, 0) * multiplier;
                grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
                break;
            case 1 or -3:
                character.transform.position += new Vector3(jumpLength, -jumpLength / 2, 0) * multiplier;
                grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
                break;
            case 2 or -2:
                character.transform.position += new Vector3(-jumpLength, -jumpLength / 2, 0) * multiplier;
                grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
                break;
            case 3 or -1:
                character.transform.position += new Vector3(-jumpLength, jumpLength / 2, 0) * multiplier;
                grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
                break;
        }
    }
    public abstract char ChangeTag();
}
