using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public static List<Vector2> myMoves = new List<Vector2>();
  
    int act = 1;
    float myTime = 1f;
    int index = 0;
    bool enemyMove = false;
    
    void Update()
    {
        if (myMoves.Count >= 5)
        {
            //myMoves.Count >= 5
            myTime -= Time.deltaTime; // dancing rats
            if (myTime < 0f && enemyMove == false)
            {
                TryMove(gameObject, (int)myMoves[index].x, (int)myMoves[index].y);
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
                myMoves.Clear();
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
    }
    public override char ChangeTag()
    {
        return 'P';
    }

    public override void DoAMove(int inc)
    {
        throw new System.NotImplementedException();
    }
    private bool Waste()
    {
        return true;
    }
}
