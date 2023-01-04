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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(1);
        }
    }
    public void JoinGameMobile()
    {
        GameSession.sessionID = Int32.Parse(sessionCode.text);
        SceneManager.LoadScene("Mobile Interface");
        
    }

    public void LoadDesktopMatchmaking()
    {
        SceneManager.LoadScene("Desktop Matchmaking");
    }

    
}
