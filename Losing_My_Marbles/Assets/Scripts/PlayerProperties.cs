using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerProperties : Movement
{
    public AnimationCurve jumpProgress;
    [HideInInspector] public GameObject characterToAnimate;
    [HideInInspector] public Vector2 destination;
    [HideInInspector] public float animTimer = 10f;

    public int playerID = 0; // playerID of (0) is null

    public static List <int> ids = new List <int> ();
    public static List<Vector2> myActions = new List<Vector2>();
  
    public List <Vector2> marbleEffect = new List<Vector2> ();
   
    int act = 1;
    float myTime = 1f;
    int index = 0;
    bool enemyMove = false;
  
    float timeBetween = 0.5f;


    private void Start()
    {
        TurnManager.players.Add(this.gameObject.GetComponent<PlayerProperties>());
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
        }
    }
    public override char ChangeTag()
    {
        return 'P';
    }

    public override void DoAMove(int inc, int dir)
    {
        throw new System.NotImplementedException();
    }
    private IEnumerator Turn()
    {
        for (int i = 0; i < myActions.Count; i++)
        {
            for (int j = 0; j < (int)myActions[i].y; j++) // b�rjan p� turnmanager.
            {
                yield return new WaitForSeconds(timeBetween);
                TryMove(gameObject, (int)myActions[i].x, 1);

            }
            yield return new WaitForSeconds(timeBetween);
            enemies[0].DoAMove(1, enemies[0].GetComponent<RatProperties>().currentDirectionID);
        }
    }

    public void AddMarbles()
    {
        for (int i = 0; i < 5; i++)
        {
            this.marbleEffect.Add(myActions[0]);
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
        if (TryMove(gameObject, 0, 1) == true)
        {
            gameObject.GetComponent<PlayerProperties>().currentDirectionID = savedDir;
        }
        if(hasKey == true)
        {

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

