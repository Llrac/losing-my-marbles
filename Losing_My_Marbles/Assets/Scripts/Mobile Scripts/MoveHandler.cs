using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{
    public DatabaseAPI database;
    private GameObject player;
    private PlayerProperties pp;
    
    // Start is called before the first frame update
    private void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
        player = pp.gameObject;
        database.ListenForMovement(InstantiateMove, Debug.Log);
    }

    private void Update()
    {
    }

    public void SendMove()
    {
        database.PostMove(new MoveMessage(player, 0, 1), () =>
        {
            Debug.Log("Move was sent!");
        }, exception => {
            Debug.Log(exception);
        });
    }
    
    private void InstantiateMove(MoveMessage moveMessage)
    {
        Debug.Log("Instantiate Move");
        
        
        var character = $"{moveMessage.character}";
        var dataID = Int32.Parse($"{moveMessage.dataID}");
        var increment = Int32.Parse($"{moveMessage.increment}");
        
        pp.TryMove(player, dataID, increment);
    }
}
