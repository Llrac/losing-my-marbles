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
    public static float turnLenght = .5f; // den här kan alltså ändras så att man hinner med en annan corroutine!!!
    public static List <PlayerProperties> players = new List <PlayerProperties> ();
    public static List <PlayerProperties> sortedPlayers = new List <PlayerProperties> ();
    public LogHandler logHandler;
    
    //add a sortet list here
    bool startTurn = true;
    int tracking = 0;
    int ratPathKeeping = 0;
    private void Update()
    {
        if(PlayerProperties.ids.Count > tracking)
        {
            logHandler.InstantiateMessage(tracking);
            Debug.Log("Player " + (PlayerProperties.ids[tracking]) + " has locked in");
            tracking++;
        }
        
        if(PlayerProperties.myActions.Count == players.Count * 5 && PlayerProperties.myActions.Count != 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (PlayerProperties.ids[i] == players[j].playerId)
                    {
                        players[j].AddMarbles();
                        sortedPlayers.Add (players[j]);
                    }
                }
            }
            
            if(startTurn == true)
            { 
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
                for (int steps = 0; steps < Mathf.Abs((int)sortedPlayers[playerInList].marbleEffect[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                {                                                // implement a if player is still alive.
                    switch ((int)sortedPlayers[playerInList].marbleEffect[currentTurn].x)
                    {
                        case 0:
                            sortedPlayers[playerInList].TryMove(players[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, 1);
                            break;
                        case 1:
                            sortedPlayers[playerInList].TryMove(players[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, (int)players[playerInList].marbleEffect[currentTurn].y);
                            break;
                    }
                    yield return new WaitForSeconds(turnLenght);
                }
                yield return new WaitForSeconds(turnLenght);
            }
            // enemy
            if(Movement.enemies.Count > 0)
            {
                for (int enemyCounter = 0; enemyCounter < Movement.enemies.Count; enemyCounter++)
                {
                    float pathForRatx = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].x;
                    float pathForRaty = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].y;
                    yield return new WaitForSeconds(turnLenght);
                    Movement.enemies[enemyCounter].DoAMove((int)pathForRatx, (int)pathForRaty, Movement.enemies[enemyCounter].currentDirectionID);
                }
            }

            ratPathKeeping++;
            Debug.Log(ratPathKeeping.ToString());
            if(ratPathKeeping >= 7)
            {
                ratPathKeeping = 0;
            }
            yield return new WaitForSeconds(turnLenght);
           
            Environment.Turn();
        }
        startTurn = true;
        for(int i = 0; i < players.Count; i++)
        {
            players[i].ResetMarbles();
        }
        PlayerProperties.ids.Clear();
        tracking = 0;
        sortedPlayers.Clear();
    }
    
    
    
}
