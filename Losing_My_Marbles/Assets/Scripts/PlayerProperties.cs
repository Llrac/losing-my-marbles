using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public int playerId = 0; // playerID of (0) is null

    public static List<int> ids = new();
    public static List<int> myActions = new();
    public List<int> playerMarbles = new();
    public List <Vector2> marbleEffect = new();

    GridGenerator gridGen;

    int act = 1;
  
    private void Awake()
    {
        TurnManager.players.Add(gameObject.GetComponent<PlayerProperties>());
        gridGen = FindObjectOfType<GridGenerator>();

        UpdateAnimation();
        UpdateSkinBasedOnPlayerID();
    }

    void Update()
    {
        if (playerId == 1)
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
                Pushed(-1);
            }
        }

        if (playerId == 2)
        {
            if (Input.GetButtonDown("Jump"))
            {
                TryMove(gameObject, 0, act);
            }
        }

        if (hasKey)
            gridGen.UpdateGlitter();
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
            }
           // Debug.Log(myActions[0]);
            playerMarbles.Add(myActions[0]);
            myActions.RemoveAt(0);
        }
    }

    public void ResetMarbles()
    {
        marbleEffect.Clear();
    }

    public bool Pushed(int dir)
    {
        int savedDir = currentDirectionID;
        currentDirectionID = dir;

        if (TryMove(gameObject, 0, 1) == true)
        {
            currentDirectionID = savedDir;
            UpdateAnimation();
            
            return true;
        }
        currentDirectionID = savedDir;
        UpdateAnimation();
        return false;
    }
}

