using System;

[Serializable]
public class MobileGameSessionMessage
{
    public int playerID;


    public MobileGameSessionMessage(int playerID)
    {
        this.playerID = playerID;
    }
}