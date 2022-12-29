using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class SessionMessage
{
    public int gameSessionID;

    public SessionMessage(int gameSessionID)
    {
        this.gameSessionID = gameSessionID;
    }
    
    
}
