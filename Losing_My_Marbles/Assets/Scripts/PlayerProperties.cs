using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    [Header("Player Properties")]
    public int playerID = 0; // playerID of (0) is null
    private Vector2 startingGridPosition = Vector2.zero;
    private Vector2 startingWorldPosition = Vector2.zero;
    private int startingDirection = 0;
    public int specialMarbleCount = 0;

    public static List<int> ids = new();
    public static List<int> myActions = new();
    public List<int> playerMarbles = new();
    public List <Vector2> marbleEffect = new();

    SpriteRenderer FindIntentShower;
    SetIntent intent;
    GridManager gridManager;
    int act = 1;
    public bool isAlive;

    public static int[] scoreKeeper = new int[4]
    {
        0,0,0,0
    };

    private void Awake()
    {
        TurnManager.players.Add(gameObject.GetComponent<PlayerProperties>());
        gridManager = FindObjectOfType<GridManager>();
        UpdateSkinBasedOnPlayerID();
        GetScore(); // keps your score
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "IntentShower")
            {
                FindIntentShower = child.gameObject.GetComponent<SpriteRenderer>();
            }
        }
        //FindIntentShower = transform.GetComponentInChildren<SpriteRenderer>(); //only works intentshower is the first spriterenderer in children 
        intent = FindIntentShower.GetComponent<SetIntent>();
        startingGridPosition = gridPosition;
        startingWorldPosition = transform.position;
        startingDirection = currentDirectionID;
    }
    private void GetScore()
    {
        specialMarbleCount = scoreKeeper[playerID - 1];
    }
    private void Start()
    { 
        FindObjectOfType<UIDesktop>().UpdatePickupMarbles(this.gameObject);
        UpdateSkeleton();
    }

    private void OnDestroy()
    {
        scoreKeeper[playerID - 1] = specialMarbleCount;
        TurnManager.players.Remove(this);
        TurnManager.sortedPlayers.Remove(this);
    }
    
    void Update()
    {
        if (playerID == DebugManager.characterToControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                TryMove(gameObject, 0, act);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TryMove(gameObject, 1, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TryMove(gameObject, 1, 1);
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                act++;
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                act--;
            }
            
            if (Input.GetButtonDown("Jump"))
            {
                SpecialMarble.Amplifier(this);
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                StartCoroutine(FindObjectOfType<SpecialMarble>().Bomb(this));
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerBomb);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                FindObjectOfType<SpecialMarble>().Swap(this);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerSwap);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                TryMove(gameObject, 2, 3);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ResetManager.PlayerWin(playerID);
            }
        }
    }

    public override char ChangeTag()
    {
        return 'P';
    }

    public override bool DoAMove(int id, int inc, int dir)
    {
        Debug.Log("player DoAMove");
        throw new System.NotImplementedException();
    }
    
    public void AddMarbles()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (myActions[0])
            {
                case 1: // Move 1
                    marbleEffect.Add(new Vector2(0, 1));
                    break;
                case 2: // Move 2
                    marbleEffect.Add(new Vector2(0, 2));
                    break;
                case 3: // Move 3
                    marbleEffect.Add(new Vector2(0, 3));
                    break;
                case 4: // Turn L
                    marbleEffect.Add(new Vector2(1, -1));
                    break;
                case 5: // Turn R
                    marbleEffect.Add(new Vector2(1, 1));
                    break;
                case 6: // Blink
                    marbleEffect.Add(new Vector2(2, 3));
                    break;
                case 7: //Turn 180
                    marbleEffect.Add(new Vector2(1, 2));
                    break;
                case 8:
                    marbleEffect.Add(new Vector2(3, 1));
                    //earthquake
                    break;
                case 9:
                    marbleEffect.Add(new Vector2(4, 3));
                    //bomb
                    break;
                case 10:
                    marbleEffect.Add(new Vector2(5, 1));
                    //Daze
                    break;
                case 11:
                    marbleEffect.Add(new Vector2(6, 1));
                    //drop key
                    break;
                case 12:
                    marbleEffect.Add(new Vector2(7, 1));
                    //amplify
                    break;
                case 13:
                    marbleEffect.Add(new Vector2(8, 1));
                    //BlockMove
                    break;
                case 14:
                    marbleEffect.Add(new Vector2(9, 1));
                    //Swap
                    break;
                case 15:
                    marbleEffect.Add(new Vector2(10, 1));
                    //RollerSkates
                    break;
                

            }
           // Debug.Log(myActions[0]);
            playerMarbles.Add(myActions[0]);
            myActions.RemoveAt(0);
        }
    }
    
    public void ResetMarbles()
    {
        marbleEffect.Clear();
        playerMarbles.Clear();
    }

    public bool Pushed(int dir)
    {
        int savedDir = currentDirectionID;
        currentDirectionID = dir;

        if (TryMove(gameObject, 0, 1, 2) == true)
        {
            currentDirectionID = savedDir;
            UpdateSkeleton();
            
            return true;
        }
        currentDirectionID = savedDir;
        UpdateSkeleton();
        return false;
    }
    public void ShowMyIntent(int marbleID)
    {
        Debug.Log(intent);
        intent.ShowIntent(Intent.GiveIntent(marbleID), !isAlive);
    }
    public void HideMyIntent()
    {
        intent.HideIntent();
    }
    public void Death(Vector2 effect)
    {
        if (deathPoof != null)
        {
            GameObject newPoof = Instantiate(deathPoof, effect, transform.rotation);
            Destroy(newPoof, 1f);
        }

        gridManager.levels[GridManager.currentLevel][(int)gridPosition.x, (int)gridPosition.y] = savedTile;

        marbleEffect.Clear();
        
        for (int i = 0; i < 5; i++)
        {
            marbleEffect.Add(new Vector2(1, 0));
        }
        StartCoroutine(Respawn());
       
    }
    private IEnumerator Respawn()
    {
        transform.position = new Vector2(100,100); // mediocre fix
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if (TurnManager.players[i].gridPosition == startingGridPosition && TurnManager.players[i].playerID != playerID)
            {
                TurnManager.players[i].Pushed(startingDirection);
            }
        }
        currentDirectionID = startingDirection;
        gridPosition = startingGridPosition;
        UpdateSkeleton();
        gridManager.levels[GridManager.currentLevel][(int)gridPosition.x, (int)gridPosition.y] = ChangeTag();
        savedTile = GridManager.WALKABLEGROUND;
        GetComponent<AnimationCurveHandler>().Respawn(gameObject, startingWorldPosition);
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().characterFall);
        isAlive = false;
    }
}

