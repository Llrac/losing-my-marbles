using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    // turn order

    // player 1, 2, 3, 4

    // enemy 1, 2, 3, etc
    // all hazard tiles
    // all environment tiles
    public int amountOfTurns = 3;
    public static float turnLength = .5f; // den h�r kan allts� �ndras s� att man hinner med en annan coroutine!!!
    public static List <PlayerProperties> players = new();
    public static List <PlayerProperties> sortedPlayers = new();
    public UIDesktop uiDesktop;
    public GameObject readyAlert;
    public GameObject information;
    private TextMeshProUGUI roundInformation;

    public ActionHandler actionHandler;

    private SpecialMarble specialMarbles;
    //TODO add a sorted list here
    bool startTurn = true;
    int tracking = 0;
    int ratPathKeeping = 0;
    int amountOfRounds = 0;

    private void Awake()
    {
        actionHandler.DrawNewHand(true);
    }

    private void Start()
    {
        uiDesktop.TurnOnMarbleBagAnimation();
        roundInformation = information.GetComponent<TextMeshProUGUI>();
        specialMarbles = gameObject.GetComponent<SpecialMarble>();
    }
    private void Update()
    {
        //Debugging
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(ExecuteTurn());
        }

        // end of debugging
        if (PlayerProperties.ids.Count > tracking)
        {
            uiDesktop.TurnOffMarbleBagAnimation(PlayerProperties.ids[tracking]);
            //Debug.Log("Player " + (PlayerProperties.ids[tracking]) + " has locked in");
            tracking++;
        }
        
        if (PlayerProperties.myActions.Count == players.Count * amountOfTurns && PlayerProperties.myActions.Count != 0)
        {
            //Debug.Log(players.Count);
            //Debug.Log(players[0].marbleEffect.Count);
            
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
        amountOfRounds++;

        yield return new WaitForSeconds(2f);
        uiDesktop.TogglePlayerBags(false);
       
        for (int i = 3; i > 0; i--)
        {
            roundInformation.text = "Round " + amountOfRounds + " Starts in " + i;
            yield return new WaitForSeconds(1f);
        }
        
        roundInformation.text = ""; // TODO add sounds
       
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            uiDesktop.ToggleReadyShine(sortedPlayers[i].playerID, false);
            uiDesktop.InstantiatePlayerOrder(sortedPlayers[i].playerID);
            yield return new WaitForSeconds(0.3f);
        }
        
        for (int currentTurn = 0; currentTurn < amountOfTurns; currentTurn++) //keeps track of turns
        {
            roundInformation.text = "Marble " + (currentTurn + 1);
            for (int playerInList = 0; playerInList < players.Count; playerInList++) // keeps track of which player is currently doing something
            {
                //show intent
                sortedPlayers[playerInList].ShowMyIntent(sortedPlayers[playerInList].playerMarbles[currentTurn]);
                yield return new WaitForSeconds(turnLength);
                for (int steps = 0; steps < Mathf.Abs((int)sortedPlayers[playerInList].marbleEffect[currentTurn].y); steps++)  // execute player j trymove with player j gameobject and player j list of actions    
                {                                                                                                                                                         //extra actions is for rollerskates
                    switch (sortedPlayers[playerInList].marbleEffect[currentTurn].x)
                    {
                        case 0:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, 1);
                           
                            break;
                        case 1:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y);
                            if (sortedPlayers[playerInList].marbleEffect[currentTurn].y > 1)
                            {
                                steps = (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y; // added for 180  turn
                            }
                            break;
                        case 2:
                            sortedPlayers[playerInList].TryMove(sortedPlayers[playerInList].gameObject, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].x, (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y);
                            steps = (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y;
                            break;
                        default:
                            specialMarbles.ExecuteSpecialMarble(sortedPlayers[playerInList], sortedPlayers[playerInList].marbleEffect[currentTurn].x, sortedPlayers[playerInList].marbleEffect[currentTurn].y, currentTurn);
                            steps = (int)sortedPlayers[playerInList].marbleEffect[currentTurn].y;
                            //special marbles
                            break;
                    }
                    yield return new WaitForSeconds(turnLength);
                }
                //hide intent
                sortedPlayers[playerInList].HideMyIntent();
                yield return new WaitForSeconds(turnLength);
            }
            // enemy
            //if(Movement.enemies.Count > 0)
            //{
            //    for (int enemyCounter = 0; enemyCounter < Movement.enemies.Count; enemyCounter++)
            //    {
            //        float pathForRatx = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].x;
            //        float pathForRaty = Movement.enemies[enemyCounter].GetComponent<RatProperties>().moves[ratPathKeeping].y;

            //        Movement.enemies[enemyCounter].DoAMove((int)pathForRatx, (int)pathForRaty, Movement.enemies[enemyCounter].currentDirectionID);
            //        yield return new WaitForSeconds(turnLength);
            //    }
            //}

            ratPathKeeping++;
            if (ratPathKeeping >= FindObjectOfType<RatProperties>().moves.Count)
            {
                ratPathKeeping = 0;
            }
            yield return new WaitForSeconds(turnLength);

            //Environment.Turn();
        }

        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerProperties>().isAlive = true;
            players[i].ResetMarbles();
        }

        PlayerProperties.ids.Clear();
        tracking = 0;
        sortedPlayers.Clear();
        
        uiDesktop.ClearPlayerOrder();
        uiDesktop.TurnOnMarbleBagAnimation();
        uiDesktop.TogglePlayerBags(true);

        readyAlert.GetComponent<Image>().enabled = true;
        
        roundInformation.text = "Round " + amountOfRounds + " is over";
        yield return new WaitForSeconds(2f);
        roundInformation.text = "Play your marbles!";
        startTurn = true;
        
        actionHandler.DrawNewHand(true);
    }
}
