using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSession : MonoBehaviour
{
    public static int sessionID = 0;
    public DatabaseAPI database;
    public int numberOfPlayers;

    public void Start()
    {
        //database.ListenForGameSession(InstantiateSession, Debug.Log);
    }

    public void CreateSession()
    {
        GenerateSessionID();
        database.CreateGameSession(new GameSessionMessage(sessionID, 4), () =>
        {
            // session created
        }, exception => { Debug.Log(exception); });
    }

    // private void InstantiateSession(GameSessionMessage gameSessionMessage)
    // {
    //     var sessionID = ($"{gameSessionMessage.gameSessionID}");
    //     
    // }

    public void GenerateSessionID()
    {
        sessionID = Random.Range(0, 999999);
    }
    
}
