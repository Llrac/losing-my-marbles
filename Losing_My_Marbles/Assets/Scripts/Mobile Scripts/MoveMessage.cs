using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class MoveMessage
{

    public int playerID;
    public int firstAction;
    public int secondAction;
    public int thirdAction;
    public int fourthAction;
    public int fifthAction;

    public MoveMessage(int playerID, int firstAction, int secondAction, int thirdAction, int fourthAction, int fifthAction)
    {
        this.playerID = playerID;
        this.firstAction = firstAction;
        this.secondAction = secondAction;
        this.thirdAction = thirdAction;
        this.fourthAction = fourthAction;
        this.fifthAction = fifthAction;
    }
   
}
