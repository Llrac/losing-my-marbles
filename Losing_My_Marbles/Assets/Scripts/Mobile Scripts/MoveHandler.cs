using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    public DatabaseAPI database;
    private GameObject player;
    private PlayerProperties playerProperties;
    public MarbleManager marbleManager;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerProperties = FindObjectOfType<PlayerProperties>();
        player = playerProperties.gameObject;
        
        database.ListenForMovement(InstantiateMove, Debug.Log);
    }

    public void SendMove()
    {
        database.PostMove(new MoveMessage(1, marbleManager.orderID[0], marbleManager.orderID[1], marbleManager.orderID[2], marbleManager.orderID[3], marbleManager.orderID[4]), () =>
        {
            Debug.Log("Move was sent!");
        }, exception => {
            Debug.Log(exception);
        });
    }
    
    private void InstantiateMove(MoveMessage moveMessage)
    {
        var move1 = Int32.Parse($"{moveMessage.firstAction}");
        var move2 = Int32.Parse($"{moveMessage.secondAction}");
        var move3 = Int32.Parse($"{moveMessage.thirdAction}");
        var move4 = Int32.Parse($"{moveMessage.fourthAction}");
        var move5 = Int32.Parse($"{moveMessage.fifthAction}");
        

        List<int> moves = new List<int>()
        {
            move1,move2,move3,move4,move5
        };
        
        Debug.Log("Instantiate Move");
        
        
        foreach (int move in moves)
        {
            switch (move)
            {
                case 1:    
                    playerProperties.TryMove(player, 0, 1);
                    break;
                case 2:    
                    playerProperties.TryMove(player, 0, 2);
                    break;
                case 3:    
                    playerProperties.TryMove(player, 0, 3);
                    break;
                case 4:    
                    playerProperties.TryMove(player, 1, -1);
                    break;
                case 5:    
                    playerProperties.TryMove(player, 1, 1);
                    break;
            }
        }
    }
}
