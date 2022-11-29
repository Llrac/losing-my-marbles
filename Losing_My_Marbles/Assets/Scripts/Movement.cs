using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public Vector2 gridPosition = new(0, 0);
    public List<Movement> enemies = new List<Movement>();

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
        enemies.Add(FindObjectOfType<RåttaProperties>());

        grid = FindObjectOfType<GridManager>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Sprite")
                body = child.gameObject;
        }
        
        sr = body.GetComponent<SpriteRenderer>();

    }

    public void TryMove(GameObject character, int dataID, int increment, int callersDirectionID = 5)
    {
        // If this TryMove was called from another character's TryMove with their currentDirectionID,
        // use their currentDirectionID as this currentDirectionID
        if (callersDirectionID != 5)
        {
            currentDirectionID = callersDirectionID;
        }

        // Set transform position
        if (dataID == 0)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                switch (grid.IsSquareEmpty(character, RequestGridPosition(currentDirectionID)))
                {
                    case 0: // EMPTY (walls, void, etc)
                        TryMove(character, 1, 2);
                        break;
                    case 1: // WALKABLEGROUND
                        Move(character, increment);
                        break;
                    case 2: // PLAYER
                       
                        // Move(character, increment);
                        break;
                    case 3: // ENEMY
                        GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID) + gridPosition, enemies);
                        if (grid.IsSquareEmpty(enemy, RequestGridPosition(currentDirectionID)) == 1)
                        {
                            Move(enemy, increment);
                            Move(character, increment);
                        }
                           
                        
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
        return currentDirectionID switch
        {
            0 => new Vector2(0, 1),
            1 or -3 => new Vector2(1, 0),
            2 or -2 => new Vector2(0, -1),
            3 or -1 => new Vector2(-1, 0),
            _ => new Vector2(0, 0),
        };
    }

    void Move(GameObject character, int increment)
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
