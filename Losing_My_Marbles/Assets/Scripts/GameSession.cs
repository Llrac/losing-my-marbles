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
    public int activePlayers = 7;

    private void Start()
    {
        if (iAmAMobilePhone == true) // toggle this in the editor when building for mobile
        {
            database.ListenForGameSessionMobile(InstantiateNewSession, Debug.Log);
            
            Debug.Log(mobilePlayerID);

            if (SceneManager.GetActiveScene().name == "Mobile Matchmaking")
            {
                mobilePlayerID = -1;
                Debug.Log(SceneManager.GetActiveScene().name);
            }
            
            Debug.Log(mobilePlayerID);
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
            
            database.PostGameSessionInfoToMobile(new MobileGameSessionMessage(7), () =>
            {
                // posted playerID to mobile
            }, exception => { Debug.Log(exception); });

        }
        
        if (activePlayers == numberOfPlayers)
            CheckMatchedGames.matchedGame = true;
    }
    
    private void InstantiateNewSession(MobileGameSessionMessage mobileGameSessionMessage)
    {
        var playerID= Int32.Parse($"{mobileGameSessionMessage.playerID}");
        
        if (playerID == 0)
            return;

        if (mobilePlayerID == -1)
        {
            PlayerID.playerID = playerID;
            mobilePlayerID = playerID;
            Debug.Log(playerID);
            isNewPlayer = false;
            Debug.Log(isNewPlayer);
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