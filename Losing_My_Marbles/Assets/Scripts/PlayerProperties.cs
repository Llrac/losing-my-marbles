using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public AnimationCurve jumpProgress;
    [HideInInspector] public GameObject characterToAnimate;
    [HideInInspector] public Vector2 destination;
    [HideInInspector] public float animTimer = 10f;
    GridGenerator gridGen;

    public int playerId = 0; // playerID of (0) is null

    public static List<int> ids = new();
    public static List<int> myActions = new();
    public List<int> playerMarbles = new();
    public List <Vector2> marbleEffect = new List<Vector2> ();
   
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
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log(jumpProgress.length);
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

        animTimer += Time.deltaTime;

        if (animTimer <= jumpProgress.length)
        {
            characterToAnimate.transform.position = new Vector2(Mathf.Lerp(characterToAnimate.transform.position.x, destination.x, jumpProgress.Evaluate(animTimer)),
            Mathf.Lerp(characterToAnimate.transform.position.y, destination.y, jumpProgress.Evaluate(animTimer)));
            if (hasKey)
                gridGen.UpdateGlitter();
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

    public void Pushed(int dir)
    {
        int savedDir = currentDirectionID;
        currentDirectionID = dir;
        //TryMove(gameObject, 0, 1);
        if (hasKey == true)
        {
            DroppKey();
        }
        if (TryMove(gameObject, 0, 1) == true)
        {
            currentDirectionID = savedDir;
        }
       
       // currentDirectionID = savedDir;

    }

    public void TransitionFromTo(GameObject character, Vector3 position)
    {
        characterToAnimate = character;
        destination = position;
        animTimer = 0;
    }
}

