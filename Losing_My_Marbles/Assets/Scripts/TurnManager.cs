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
    public static List<PlayerProperties> sortedPlayers = new List <PlayerProperties> ();
    bool startTurn = true;

    private void Update()
    {
        
        if(PlayerProperties.myActions.Count == players.Count * 5 && players.Count != 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (PlayerProperties.ids[i] == players[j].playerId)
                    {
                        players[j].AddMarbles();
                        sortedPlayers.Add(players[j]);
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
            for (int playerInList = 0; playerInList < sortedPlayers.Count; playerInList++) // keeps track of which player is currently doing something
            {
                for (int steps = 0; steps < Mathf.Abs((int)sortedPlayers[playerInList].marbleEffect[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                    // implement a if player is still alive.
                Debug.Log(sortedPlayers[playerInList].playerId);
                { 
                    switch ((int)sortedPlayers[playerInList].marbleEffect[currentTurn].x)
                    {
                        case 0:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, 1);
                            break;
                        case 1:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y);
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
                    yield return new WaitForSeconds(turnLenght);
                    Movement.enemies[enemyCounter].DoAMove(1, Movement.enemies[enemyCounter].currentDirectionID);
                }
            }
            
            yield return new WaitForSeconds(turnLenght);
           
            Environment.Turn();
        }
        startTurn = true;
        for(int i = 0; i < sortedPlayers.Count; i++)
        {
            sortedPlayers[i].ResetMarbles();
        }
        PlayerProperties.ids.Clear();
    }
}
