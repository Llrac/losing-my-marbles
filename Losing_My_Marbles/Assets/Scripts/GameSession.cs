using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private bool iAmAMobilePhone;
    
    public static int sessionID = 0;
    public static bool isNewPlayer = true;
    public static int mobilePlayerID;
    public DatabaseAPI database;
    public int numberOfPlayers = 4;
    public int activePlayers = 0;

    private void Start()
    {
        if (iAmAMobilePhone == true) // toggle this in the editor when building for mobile
        {
            database.ListenForGameSessionMobile(InstantiateNewSession, Debug.Log);
            
            if (isNewPlayer)
                mobilePlayerID = -1;
            //DatabaseAPI.mobileIsListening = true;
        }
    }

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
        var gameSession = Int32.Parse($"{sessionMessage.gameSessionID}");
        var gameActive = Convert.ToBoolean($"{sessionMessage.gameActive}");

        if (gameActive == false)
            return;

        Debug.Log(gameSession);
        
        if (gameSession == sessionID)
        {
            activePlayers++;
            
            database.PostGameSessionInfoToMobile(new GameSessionMessage(activePlayers), () =>
            {
                // posted playerID to mobile
            }, exception => { Debug.Log(exception); });

        }
        
        if (activePlayers == numberOfPlayers)
            CheckMatchedGames.matchedGame = true;
    }
    
    private void InstantiateNewSession(GameSessionMessage gameSessionMessage)
    {
        var playerID= Int32.Parse($"{gameSessionMessage.playerID}");
        
        if (playerID == 0)
            return;

        if (mobilePlayerID == -1)
        {
            mobilePlayerID = playerID;
            isNewPlayer = false;
            Debug.Log("I am now " + mobilePlayerID);
        }
        
        Debug.Log("I am now " + mobilePlayerID);

    }

    public void JoinGame()
    {
        database.JoinGameSession(new SessionMessage(sessionID, true), () =>
        {
            
        }, exception => { Debug.Log(exception); });
        
    }

    private void GenerateSessionID()
    {
        sessionID = Random.Range(0, 999999);
    }
    
}