using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class Scenehandler : MonoBehaviour
{
    public GameSession gameSession;
    public TMP_InputField sessionCode;

    public void Start()
    {
        
    }

    public void JoinGameMobile()
    {
        GameSession.sessionID = Int32.Parse(sessionCode.text);
        SceneManager.LoadScene("Mobile Interface");
    }

    public void LoadMobileMatchmaking()
    {
        SceneManager.LoadScene("Mobile Matchmaking");
    }
    
    public void LoadChooseColor()
    {
        SceneManager.LoadScene("Choose Color");
    }

    public void LoadDesktopMatchmaking()
    {
        SceneManager.LoadScene("Desktop Matchmaking");
    }

    
}
