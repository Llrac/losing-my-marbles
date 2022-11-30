using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class MoveMessage
{
    public GameObject character;
    public int dataID;
    public int increment;
    
    public MoveMessage(GameObject character, int dataID, int increment)
    {
        this.character = character;
        this.dataID = dataID;
        this.increment = increment;
    }
}
