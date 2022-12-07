using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public char savedTile = 'X';

    public Vector2 gridPosition = new(0, 0);
    public bool hasKey = false;
    public static List <Movement> enemies = new ();

    public int currentDirectionID = 0;

    public Sprite[] sprites = new Sprite[2];
    SpriteRenderer childRenderer;
    GridManager grid;

    public float jumpLength = 1;
 
    int multiplier;

    float timer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 1f;
            //enemies[0].DoAMove(1);
        }
    }
    public bool TryMove(GameObject character, int dataID, int increment) // into bool?
    {
        // Set transform position
        if (dataID == 0)
        {
            if (grid == null)
            {
                grid = FindObjectOfType<GridManager>();
            }
            switch (grid.GetNexTile(character, RequestGridPosition(currentDirectionID)))
            {
                case GridManager.EMPTY: // EMPTY (walls, void, etc)
                    TryMove(character, 1, 2);
                    return false;// lägg till recursion här'


                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Move(character, 1);
                    savedTile = 'X';
                    break;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);
                    player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID);
                    // Move(character, increment);
                    break;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    enemy.GetComponent<RatProperties>().DoAMove(1, currentDirectionID);
                    
                    break;

                case GridManager.DOOR:
                    if (character.GetComponent<Movement>().hasKey == true)
                    {
                        Move(character, 1);
                        character.SetActive(false);
                        FindObjectOfType<ResetManager>().ResetLevel();
                    }
                    break;

                case GridManager.KEY:
                    character.GetComponent<Movement>().hasKey = true;
                    GameObject.FindGameObjectWithTag("Key").SetActive(false);
                    Move(character, 1);
                    break;

                case GridManager.WATER:
                    // do water stuff
                    Move(character, 1);
                    savedTile = GridManager.WATER;
                    break;

                case GridManager.HOLE:
                    character.SetActive(false);
                    grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0, 0));
                    if(gameObject.tag == "Player")
                    {
                        TurnManager.players.Remove(character.GetComponent<PlayerProperties>());
                    }
                    else
                    {
                        enemies.Clear();

                    }
                    break;
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

            foreach (Transform child in character.transform)
            {
                if (child.gameObject.name == "Sprite")
                    childRenderer = child.gameObject.GetComponent<SpriteRenderer>();
            }

            switch (currentDirectionID)
            {
                case 0:
                    character.GetComponent<Movement>().childRenderer.sprite = sprites[0];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1 or -3:
                    character.GetComponent<Movement>().childRenderer.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2 or -2:
                    character.GetComponent<Movement>().childRenderer.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3 or -1:
                    character.GetComponent<Movement>().childRenderer.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
            
        }
        
        return true;
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
                break;
            case 1 or -3:
                character.transform.position += new Vector3(jumpLength, -jumpLength / 2, 0) * multiplier;
                break;
            case 2 or -2:
                character.transform.position += new Vector3(-jumpLength, -jumpLength / 2, 0) * multiplier;
                break;
            case 3 or -1:
                character.transform.position += new Vector3(-jumpLength, jumpLength / 2, 0) * multiplier;
                break;
        }
        //
        grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
       
    }
    public abstract char ChangeTag();
    public abstract void DoAMove(int inc, int dir);
}
