using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSession : MonoBehaviour
{
    public static int sessionID = 0;
    public DatabaseAPI database;
    public int numberOfPlayers = 0;


    public void CreateSession()
    {
        GenerateSessionID();
        database.CreateGameSession(new GameSessionMessage(sessionID, 4), () =>
        {
            // session created
        }, exception => { Debug.Log(exception); });
        
        database.ListenForGameSession(InstantiateGameSession, Debug.Log);
    }

    public void JoinGame()
    {
        database.JoinGameSession(new SessionMessage(sessionID), () =>
        {
            
        }, exception => { Debug.Log(exception); });
    }

    private void InstantiateGameSession(SessionMessage sessionMessage)
    {
        Debug.Log("I'm listening!");
        
        var gameSession = Int32.Parse($"{sessionMessage.gameSessionID}");
        
        if (gameSession == sessionID)
        {
            numberOfPlayers++;
        }
        
        if (numberOfPlayers == 2)
            CheckMatchedGames.matchedGame = true;
    }
    
    

    // private void InstantiateSession(GameSessionMessage gameSessionMessage)
    // {
    //     sessionID = Int32.Parse($"{gameSessionMessage.gameSessionID}");
    // }

    public void GenerateSessionID()
    {
        sessionID = Random.Range(0, 999999);
    }
    
}


