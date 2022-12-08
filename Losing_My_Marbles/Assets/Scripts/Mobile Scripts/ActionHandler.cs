using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public DatabaseAPI database;
    public UIManager uiManager;
    public PlayerID playerId;

    private int playerID;
    private void Start()
    {
        playerID = playerId.playerID;
        database.ListenForActions(InstantiateAction, Debug.Log);
        
    }

    public void SendAction()
    {
        
        database.PostActions(new ActionMessage(playerID, uiManager.orderID[0],
            uiManager.orderID[1], uiManager.orderID[2],
            uiManager.orderID[3], uiManager.orderID[4]), () =>
        {
            // Action was sent!
        }, exception => {
            Debug.Log(exception);
        });
    }
    
    private void InstantiateAction(ActionMessage actionMessage)
    {
        var playerID = Int32.Parse($"{actionMessage.playerID}"); 
        var action1 = Int32.Parse($"{actionMessage.firstAction}");
        var action2 = Int32.Parse($"{actionMessage.secondAction}");
        var action3 = Int32.Parse($"{actionMessage.thirdAction}");
        var action4 = Int32.Parse($"{actionMessage.fourthAction}");
        var action5 = Int32.Parse($"{actionMessage.fifthAction}");

        List<int> listOfActions = new()
        {
            action1, action2, action3, action4, action5
        };
        
        Debug.Log("Instantiate Action");

        PlayerProperties.ids.Add(playerID);
        
        foreach (int action in listOfActions)
        {
            switch (action)
            {
                case 1: // Move 1
                    PlayerProperties.myActions.Add(new Vector2(0, 1));
                    break;
                case 2: // Move 2
                    PlayerProperties.myActions.Add(new Vector2(0, 2));
                    break;
                case 3: // Move 3
                    PlayerProperties.myActions.Add(new Vector2(0, 3));
                    break;
                case 4: // Turn L
                    PlayerProperties.myActions.Add(new Vector2(1, -1));
                    break;
                case 5: // Turn R
                    PlayerProperties.myActions.Add(new Vector2(1, 1));
                    break;
            }
        }
    }
}
