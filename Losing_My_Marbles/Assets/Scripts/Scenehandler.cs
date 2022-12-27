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
    
    public void JoinGame()
    {
        var sessionID = GameSession.sessionID.ToString();
        
        if (sessionCode.text == sessionID)
            SceneManager.LoadScene("Mobile Interface");
        
        else 
            Debug.Log("You died!");
    }
}
