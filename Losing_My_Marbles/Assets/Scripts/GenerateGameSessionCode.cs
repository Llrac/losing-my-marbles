using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateGameSessionCode : MonoBehaviour
{
    public TMP_Text sessionCode;
    
    public void Start()
    {
        if (sessionCode.text != null)
            sessionCode.text = GameSession.sessionID.ToString();
    }
}
