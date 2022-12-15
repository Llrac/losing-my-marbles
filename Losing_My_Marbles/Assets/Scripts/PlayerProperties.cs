using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public int playerID = 0; // playerID of (0) is null
    private Vector2 startingGridPosition = Vector2.zero;
    private Vector2 startingWorldPosition = Vector2.zero;

    public static List<int> ids = new();
    public static List<int> myActions = new();
    public List<int> playerMarbles = new();
    public List <Vector2> marbleEffect = new();

    GridGenerator gridGen;
    SpriteRenderer FindIntentShower;
    SetIntent intent;
    int act = 1;
  
    private void Awake()
    {
        TurnManager.players.Add(gameObject.GetComponent<PlayerProperties>());

        UpdateSkeleton();
        UpdateSkinBasedOnPlayerID();
        
        FindIntentShower = transform.GetComponentInChildren<SpriteRenderer>(); //only works intentshower is the first spriterenderer in children 
        intent = FindIntentShower.GetComponent<SetIntent>();
        startingGridPosition = gridPosition;
        startingWorldPosition = transform.position;
    }

    void Update()
    {
        if (playerID == 1)
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
            
        }

        if (playerID == 1)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //gameObject.GetComponent<Movement>().Blink(3);
                TryMove(gameObject, 2, 3);
            }
        }
    }

    public override char ChangeTag()
    {
        return 'P';
    }

    public override void DoAMove(int id, int inc, int dir)
    {
        throw new System.NotImplementedException();
    }
    
    public void AddMarbles()
    {
        for (int i = 0; i < 5; i++)
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

        if (TryMove(gameObject, 0, 1) == true)
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
        intent.ShowIntent(Intent.GiveIntent(marbleID));
    }
    public void HideMyIntent()
    {
        intent.HideIntent();
    }
    public void Death()
    {
        marbleEffect.Clear();
        for (int i = 0; i < 5; i++)
        {
            marbleEffect.Add(new Vector2(1, 0));
        }
        transform.position = startingWorldPosition;
        gridPosition = startingGridPosition;
    }
}

