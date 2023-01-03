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
        StartCoroutine(LoadMobileInterface());
    }

    public void LoadDesktopMatchmaking()
    {
        SceneManager.LoadScene("Desktop Matchmaking");
    }


    public IEnumerator LoadMobileInterface()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Mobile Interface");
    }
    
}