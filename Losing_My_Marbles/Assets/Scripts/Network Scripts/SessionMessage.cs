using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class SessionMessage
{
    public int gameSessionID;
    public bool gameActive;

    public SessionMessage(int gameSessionID, bool gameActive)
    {
        this.gameSessionID = gameSessionID;
        this.gameActive = gameActive;
    }
    
    
}
