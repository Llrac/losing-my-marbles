using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerProperties : Movement
{

    public int playerId = 0;
    public static List <int> ids = new List <int> ();
    public static List<Vector2> myActions = new List<Vector2>()
    {
       
    };
    public List <Vector2> actions = new List<Vector2> ();
    public static List<Vector2> myActions = new();
    public static List<int> ids = new();

    int act = 1;
    float myTime = 1f;
    int index = 0;
    bool enemyMove = false;
    
    int act = 1;
    float timeBetween = 0.5f;
   
    bool enemyMove = false;

    private void Start()
    {
        TurnManager.players.Add(this.gameObject.GetComponent<PlayerProperties>());
    }
    void Update()
    {
       
            
        if (myActions.Count >= 5)
        {
            
         
            //myMoves.Count >= 5
            //myTime -= Time.deltaTime; // den kommer inte ut h�r i tid till sequencing.
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
            //    enemies[0].DoAMove(1, enemies[0].GetComponent<R�ttaProperties>().currentDirectionID);
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
   
}
