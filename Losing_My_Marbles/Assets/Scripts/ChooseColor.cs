using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChooseColor : MonoBehaviour
{
    public void PickRed()
    {
        PlayerID.playerID = 1;
    }

    public void PickPurple()
    {
        PlayerID.playerID = 2;
    }
    
    public void PickTeal()
    {
        PlayerID.playerID = 3;
    }
    
    public void PickYellow()
    {
        PlayerID.playerID = 4;
    }
}
