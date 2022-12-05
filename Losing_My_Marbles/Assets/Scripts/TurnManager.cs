using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // turn order

    // player 1, 2, 3, 4

    // enemy 1, 2, 3, etc
    // all hazard tiles
    // all environment tiles
    int amountOfTurns = 5;
    float turnLenght = .5f;
    public static List <PlayerProperties> players = new List <PlayerProperties> ();
    bool startTurn = false;

    private void Update()
    {
        
        if(PlayerProperties.myActions.Count == 10)
        {
            
            for(int i = 0; i < players.Count; i++)
            {
                switch (players[i].playerId)
                {
                    case 1:
                        for (int j = 0; j < 5; j++)
                        {
                            players[i].actions.Add(PlayerProperties.myActions[j]);
                            
                        }
                        
                        break;

                    case 2:
                        for (int k = 5; k < 9; k++)
                        {
                            players[i].actions.Add(PlayerProperties.myActions[k]);
                            
                        }
                       

                        break;
                }
            }
            if(startTurn == false)
                StartCoroutine(ExecuteTurn()); startTurn = true;
        }
        if (startTurn == true)
        {
           // StartCoroutine(ExecuteTurn());
        }
    }
    private IEnumerator ExecuteTurn()
    {
        for (int currentTurn = 0; currentTurn < amountOfTurns; currentTurn++) //keeps track of turns
        {
            for (int playerInList = 0; playerInList < players.Count; playerInList++) // keeps track of which player is currently doing something
            {
                Debug.Log(playerInList);
                for (int steps = 0; steps < Mathf.Abs((int)players[playerInList].actions[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                {                                                                             // början på turnmanager.
                    Debug.Log((int)players[playerInList].actions[currentTurn].y);
                    switch ((int)players[playerInList].actions[currentTurn].x)
                    {
                        case 0:
                            players[playerInList].TryMove(players[playerInList].gameObject, (int)players[playerInList].actions[currentTurn].x, 1);
                            break;
                        case 1:
                            players[playerInList].TryMove(players[playerInList].gameObject, (int)players[playerInList].actions[currentTurn].x, (int)players[playerInList].actions[currentTurn].y);
                            break;
                    }
                    yield return new WaitForSeconds(turnLenght);
                   

                }
            }
            // enemy
            // environment
        }
    }
}
