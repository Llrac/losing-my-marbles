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
    bool startTurn = true;

    private void Update()
    {
        
        if(PlayerProperties.myActions.Count == players.Count * 5)
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if(PlayerProperties.ids[i] == players[j].playerId)
                    {
                        players[j].AddMarbles();
                    }
                }
            }
            if(startTurn == true)
            {
                Debug.Log(PlayerProperties.myActions.Count);
                StartCoroutine(ExecuteTurn()); 
                startTurn = false;
            }
                
        }
        
    }
    private IEnumerator ExecuteTurn()
    {
        for (int currentTurn = 0; currentTurn < amountOfTurns; currentTurn++) //keeps track of turns
        {
            for (int playerInList = 0; playerInList < players.Count; playerInList++) // keeps track of which player is currently doing something
            {
                for (int steps = 0; steps < Mathf.Abs((int)players[playerInList].marbleEffect[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                { 
                    switch ((int)players[playerInList].marbleEffect[currentTurn].x)
                    {
                        case 0:
                            players[playerInList].TryMove(players[playerInList].gameObject, (int)players[playerInList].marbleEffect[currentTurn].x, 1);
                            break;
                        case 1:
                            players[playerInList].TryMove(players[playerInList].gameObject, (int)players[playerInList].marbleEffect[currentTurn].x, (int)players[playerInList].marbleEffect[currentTurn].y);
                            break;
                    }
                    yield return new WaitForSeconds(turnLenght);
                }
                yield return new WaitForSeconds(turnLenght);
            }
            // enemy
            for (int enemyCounter = 0; enemyCounter < Movement.enemies.Count; enemyCounter++)
            {
                yield return new WaitForSeconds(turnLenght);
                Movement.enemies[enemyCounter].DoAMove(1, Movement.enemies[enemyCounter].currentDirectionID);
            }
            
            // environment
        }
        startTurn = true;
        for(int i = 0; i < players.Count; i++)
        {
            players[i].ResetMarbles();
        }  
    }
}
