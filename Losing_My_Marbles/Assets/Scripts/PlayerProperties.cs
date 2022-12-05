using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerProperties : Movement
{
    public static List<Vector2> myMoves = new List<Vector2>()
    {
        new Vector2 (0, 1 ), new Vector2 (0, 2), new Vector2 (0, 2), new Vector2 (1,1), new Vector2 (1,1)
    };
  
    int act = 1;
    float timeBetween = 0.5f;
   
    bool enemyMove = false;
    
    void Update()
    {
        if(enemyMove == false)
        {
            StartCoroutine(Turn()); 
            enemyMove = true;
        }
            
        if (myMoves.Count >= 5)
        {

         
            //myMoves.Count >= 5
            //myTime -= Time.deltaTime; // den kommer inte ut här i tid till sequencing.
            //if (myTime < 0f && enemyMove == false)
            //{   
            //    for (int i = 0; i < Mathf.Abs((int)myMoves[index].y); i++)
            //    {
            //        TryMove(gameObject, (int)myMoves[index].x, 1);
            //        myTime = 1f;
            //    }

            //    myTime -= Time.deltaTime;
            //    enemyMove = true;
            //}
            //if (myTime <=-1f)
            //{
            //    enemies[0].DoAMove(1, enemies[0].GetComponent<RåttaProperties>().currentDirectionID);
            //    enemyMove = false;
            //    myTime = 1f;
            //    index++;
            //}
            //if (index >= 5)
            //{
            //    myMoves.Clear();
            //    index = 0;
            //}

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

    public override void DoAMove(int inc, int dir)
    {
        throw new System.NotImplementedException();
    }
    private IEnumerator Turn()
    {
        for (int i = 0; i < myMoves.Count; i++)
        {
            for (int j = 0; j < (int)myMoves[i].y; j++) // början på turnmanager.
            {
                yield return new WaitForSeconds(timeBetween);
                TryMove(gameObject, (int)myMoves[i].x, 1);

            }
            yield return new WaitForSeconds(timeBetween);
            enemies[0].DoAMove(1, enemies[0].GetComponent<RåttaProperties>().currentDirectionID);
        }
        
        
    }
}
