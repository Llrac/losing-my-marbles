using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class NewHandMessage
{
    public bool drawNewHand;
    
    public NewHandMessage(bool drawNewHand)
    {
        this.drawNewHand = drawNewHand;
    }
}
