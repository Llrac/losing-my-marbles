using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class SessionMessage
{
    public int gameSessionID;
    public bool gameIsActive;

    public SessionMessage(int gameSessionID, bool gameIsActive)
    {
        this.gameSessionID = gameSessionID;
        this.gameIsActive = gameIsActive;
    }
    
    
}
