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

    private void Update()
    {
        
    }
    private  IEnumerator ExecuteTurn()
    {
        for (int currentTurn = 0; currentTurn < amountOfTurns; currentTurn++) //keeps track of turns
        {
            for(int playerInList = 0; playerInList < players.Count; playerInList++) // keeps track of which player is currently doing something
            {
                for (int steps = 0; steps < (int)players[playerInList].myActions[currentTurn].y; steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                {                                                                             // början på turnmanager.
                    yield return new WaitForSeconds(turnLenght);
                    players[playerInList].TryMove(players[playerInList].gameObject, (int)players[playerInList].myActions[currentTurn].x, 1); 

                }                                                    
            }
        }
    }
}
