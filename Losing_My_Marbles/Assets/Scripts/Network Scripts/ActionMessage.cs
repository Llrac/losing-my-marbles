using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ActionMessage
{
    public int playerID;
    public int firstAction;
    public int secondAction;
    public int thirdAction;
    public int fourthAction;
    public int fifthAction;

    public ActionMessage(int playerID, int firstAction, int secondAction, int thirdAction)
    {
        this.playerID = playerID;
        this.firstAction = firstAction;
        this.secondAction = secondAction;
        this.thirdAction = thirdAction;
        //this.fourthAction = fourthAction;
        //this.fifthAction = fifthAction;
    }
}
