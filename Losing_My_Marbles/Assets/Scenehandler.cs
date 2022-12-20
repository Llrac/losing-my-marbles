using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Scenehandler : MonoBehaviour
{
    public TMP_Text gameCode;
    public TMP_Text invalidCodeMessage;
    
    public void JoinGame()
    {

        var mmCode = DatabaseAPI.matchMakingCode;
        var gmCode = gameCode.text;
        
        Debug.Log(mmCode);
        Debug.Log(gmCode);
        Debug.Log(gameCode.text);
        Debug.Log(mmCode.Equals(gmCode));
        
        if (gmCode == mmCode)
        {
            Debug.Log("hej");
            SceneManager.LoadScene("Mobile Interface");
        }
        else
        {
            invalidCodeMessage.text = "Enter a valid code";
        }
            
    }

    public void CreateGame()
    {
        SceneManager.LoadScene("Matchmaking Desktop");
        Debug.Log(DatabaseAPI.matchMakingCode);
    }
}
