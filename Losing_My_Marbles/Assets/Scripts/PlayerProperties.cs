using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public AnimationCurve jumpProgress;
    public GameObject characterToAnimate;
    public Vector2 destination;
    public float animTimer = 10f;

    public int playerID = 0; // playerID of (0) is null

    public static List<Vector2> myActions = new();
  
    int act = 1;
    float myTime = 1f;
    int index = 0;
    bool enemyMove = false;

    void Update()
    {
        if (myActions.Count >= 5)
        {
            //myMoves.Count >= 5
            myTime -= Time.deltaTime;
            if (myTime < 0f && enemyMove == false)
            {
                TryMove(gameObject, (int)myActions[index].x, (int)myActions[index].y);
                enemyMove = true;
            }
            if (myTime <=-1f)
            {
                enemies[0].DoAMove(1);
                enemyMove = false;
                myTime = 1f;
                index++;
            }
            if (index >= 5)
            {
                myActions.Clear();
                index = 0;
            }
        }

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
        animTimer += Time.deltaTime;

        if (animTimer < jumpProgress.length)
        {
            characterToAnimate.transform.position = new Vector2(Mathf.Lerp(characterToAnimate.transform.position.x, destination.x, jumpProgress.Evaluate(animTimer)),
                Mathf.Lerp(characterToAnimate.transform.position.y, destination.y, jumpProgress.Evaluate(animTimer)));
        }
    }
    public override char ChangeTag()
    {
        return 'P';
    }

    public override void DoAMove(int inc)
    {
        throw new System.NotImplementedException();
    }
    private bool Waste() // currently unassigned to any keyboard input
    {
        return true;
    }

    public void JumpTo(Vector3 position)
    {
        destination = position;
        animTimer = 0;
    }
}
