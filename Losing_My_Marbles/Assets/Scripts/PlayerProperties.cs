using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public static List<Vector2> myMoves = new List<Vector2>();
  
    int act = 1;
   
    void Update()
    {
        //if(myMoves.Count >= 5)
        //{
            
        //    for(int i = 0; i < myMoves.Count; i++)
        //    {
        //        myTime -= Time.deltaTime; // dancing rats
        //        if(myTime < 5f)
        //        {
        //            TryMove(gameObject, (int)myMoves[i].x, (int)myMoves[i].y);
        //            enemies[0].DoAMove(1);
        //            myTime = 5f;
        //        }
               
        //    }
           
        //}
        // Diagonal movement
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
