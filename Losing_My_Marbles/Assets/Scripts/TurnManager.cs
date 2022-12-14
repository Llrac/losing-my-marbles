using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // turn order

    // player 1, 2, 3, 4

    // enemy 1, 2, 3, etc
    // all hazard tiles
    // all environment tiles
    int amountOfTurns = 5;
    public static float turnLength = .5f; // den h�r kan allts� �ndras s� att man hinner med en annan coroutine!!!
    public static List <PlayerProperties> players = new List <PlayerProperties> ();
    public static List <PlayerProperties> sortedPlayers = new List <PlayerProperties> ();
    public UIDesktop uiDesktop;
    public GameObject readyAlert;
    public ActionHandler actionHandler;
    
    //add a sorted list here
    bool startTurn = true;
    int tracking = 0;
    int ratPathKeeping = 0;

    private void Start()
    {
        uiDesktop.TurnOnMarbleBagAnimation();
    }
    private void Update()
    {
        if (PlayerProperties.ids.Count > tracking)
        {
            uiDesktop.TurnOffMarbleBagAnimation(PlayerProperties.ids[tracking]);
            Debug.Log("Player " + (PlayerProperties.ids[tracking]) + " has locked in");
            tracking++;
        }
        
        if (PlayerProperties.myActions.Count == players.Count * 5 && PlayerProperties.myActions.Count != 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (PlayerProperties.ids[i] == players[j].playerID)
                    {
                        players[j].AddMarbles();
                        sortedPlayers.Add(players[j]);
                    }
                }
            }
            
            if (startTurn == true)
            {
                readyAlert.GetComponent<Image>().enabled = false;
                StartCoroutine(ExecuteTurn()); 
                startTurn = false;
            }     
        }
        
        
    }

    private IEnumerator ExecuteTurn()
    {
        
        yield return new WaitForSeconds(2.5f);
        uiDesktop.TogglePlayerBags(false);
        //TODO Show "Round starts in..." here
        
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            uiDesktop.ToggleReadyShine(sortedPlayers[i].playerID, false);
            uiDesktop.InstantiatePlayerOrder(sortedPlayers[i].playerID);
            yield return new WaitForSeconds(0.3f);
        }
        
        for (int currentTurn = 0; currentTurn < amountOfTurns; currentTurn++) //keeps track of turns
        {
            for (int playerInList = 0; playerInList < players.Count; playerInList++) // keeps track of which player is currently doing something
            {
                //show intent
                sortedPlayers[playerInList].ShowMyIntent(sortedPlayers[playerInList].playerMarbles[currentTurn]);
                yield return new WaitForSeconds(turnLength);
                for (int steps = 0; steps < Mathf.Abs((int)sortedPlayers[playerInList].marbleEffect[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                {                                               
                    switch ((int)sortedPlayers[playerInList].marbleEffect[currentTurn].x)
                    {
                        case 0:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, 1);
                            break;
                        case 1:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y);
                            Debug.Log((int)sortedPlayers[playerInList].marbleEffect[currentTurn].y);
                            break;
                    }
                    yield return new WaitForSeconds(turnLength);
                }
                //hide intent
                sortedPlayers[playerInList].HideMyIntent();
                yield return new WaitForSeconds(turnLength);
            }
            // enemy
            if(Movement.enemies.Count > 0)
            {
                for (int enemyCounter = 0; enemyCounter < Movement.enemies.Count; enemyCounter++)
                {
                    //float pathForRatx = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].x;
                    //float pathForRaty = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].y;
                    
                    //Movement.enemies[enemyCounter].DoAMove((int)pathForRatx, (int)pathForRaty, Movement.enemies[enemyCounter].currentDirectionID);
                    yield return new WaitForSeconds(turnLength);
                }
            }
            

            //ratPathKeeping++;
            //Debug.Log(ratPathKeeping.ToString());
            //if(ratPathKeeping >= 7)
            //{
            //    ratPathKeeping = 0;
            //}
            yield return new WaitForSeconds(turnLength);
           
            //Environment.Turn();
        }

        for(int i = 0; i < players.Count; i++)
        {
            players[i].ResetMarbles();
        }

        PlayerProperties.ids.Clear();
        tracking = 0;
        sortedPlayers.Clear();
        
        uiDesktop.ClearPlayerOrder();
        uiDesktop.TurnOnMarbleBagAnimation();
        uiDesktop.TogglePlayerBags(true);

        readyAlert.GetComponent<Image>().enabled = true;
        
        startTurn = true;
        
        //TODO Send message to mobile ui to clear hand and draw new marbles
        actionHandler.DrawNewHand(true);
    }
}
