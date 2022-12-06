using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerProperties : Movement
{

    public int playerId = 0;
    public static List <int> ids = new List <int> ();
    public static List<Vector2> myActions = new List<Vector2>();
  
    public List <Vector2> marbleEffect = new List<Vector2> ();
   
    int act = 1;
  
    float timeBetween = 0.5f;


    private void Start()
    {
        TurnManager.players.Add(this.gameObject.GetComponent<PlayerProperties>());
    }
    void Update()
    {
        if(playerId == 1)
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
        TryMove(gameObject, 0, 1);
        currentDirectionID = savedDir;

    }
}

