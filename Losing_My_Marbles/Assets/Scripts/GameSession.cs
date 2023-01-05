using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSession : MonoBehaviour
{
    public static int sessionID = 0;
    public DatabaseAPI database;
    public int numberOfPlayers = 4;
    public static int activePlayers = 0;

    public void CreateSession()
    {
        GenerateSessionID();
        database.CreateGameSession(new GameSessionMessage(sessionID, 1), () =>
        {
            // session created
        }, exception => { Debug.Log(exception); });
        
        database.ListenForGameSession(InstantiateGameSession, Debug.Log);
    }

    private void InstantiateGameSession(SessionMessage sessionMessage)
    {
        Debug.Log("I'm listening!");

        var gameSession = Int32.Parse($"{sessionMessage.gameSessionID}");
        var gameIsActive = Convert.ToBoolean($"{sessionMessage.gameIsActive}");

        Debug.Log(gameSession);

        if (gameIsActive == false)
        {
            return;
        }
        
        if (gameSession == sessionID)
        {
            activePlayers++;
        }
        
        if (activePlayers == numberOfPlayers)
            CheckMatchedGames.matchedGame = true;
        
        
    }
    
    public void JoinGame()
    {
        database.JoinGameSession(new SessionMessage(sessionID, true), () =>
        {
            
        }, exception => { Debug.Log(exception); });
    }

    public void GenerateSessionID()
    {
        sessionID = Random.Range(0, 999999);
    }
    
}


