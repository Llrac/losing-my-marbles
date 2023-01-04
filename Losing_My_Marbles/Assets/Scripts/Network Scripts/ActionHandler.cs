using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionHandler : MonoBehaviour
{
    public DatabaseAPI database;
    public UIManager uiManager;
    

    private int playerID;

    private void Start()
    {
        database.ListenForActions(InstantiateAction, Debug.Log);
        database.ListenForNewHand(InstantiateNewHand, Debug.Log);
    }


    public void SendAction()
    {
        if (uiManager != null)
        {
            database.PostActions(new ActionMessage(PlayerID.playerID, uiManager.orderID[0],
                uiManager.orderID[1], uiManager.orderID[2]), () =>
            {
                // Action was sent!
            }, exception => { Debug.Log(exception); });

            uiManager.confirmButton.interactable = false;
            uiManager.PlayerCanInteractWithMarbles(false);
        }
    }

    public void DrawNewHand(bool drawNewHand)
    {
        database.PostNewHand(new NewHandMessage(drawNewHand), () =>
        {
            // New hand was sent!
        }, exception => { Debug.Log(exception); });
    }

    // This happens on the mobile side
    private void InstantiateNewHand(NewHandMessage newHandMessage)
    {
        var drawNewHand = Convert.ToBoolean($"{newHandMessage.drawNewHand}");

        if (uiManager == null)
            return;
        
        if (drawNewHand)
        {
            uiManager.DiscardMarblesFromHand();
            uiManager.FillHandWithMarbles();
            uiManager.PlayerCanInteractWithMarbles(true);

            if (uiManager.timer != null)
            {
                uiManager.timer.ResetTimer();
            }
        }
    }

    // This happens on the desktop side
    private void InstantiateAction(ActionMessage actionMessage)
    {
        var playerID = Int32.Parse($"{actionMessage.playerID}");
        var action1 = Int32.Parse($"{actionMessage.firstAction}");
        var action2 = Int32.Parse($"{actionMessage.secondAction}");
        var action3 = Int32.Parse($"{actionMessage.thirdAction}");

        if (playerID == 0)
            return;

        List<int> listOfActions = new()
        {
            action1, action2, action3
        };

        Debug.Log("Instantiate Action");
        Debug.Log(playerID);

        PlayerProperties.ids.Add(playerID);

        foreach (int action in listOfActions)
        {
            PlayerProperties.myActions.Add(action);
        }
    }
}