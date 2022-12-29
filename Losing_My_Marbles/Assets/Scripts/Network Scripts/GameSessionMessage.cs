using System;

[Serializable]
public class GameSessionMessage
{

    public int gameSessionID;
    public int numberOfPlayers;
    
    public GameSessionMessage(int gameSessionID, int numberOfPlayers)
    {
        this.gameSessionID = gameSessionID;
        this.numberOfPlayers = numberOfPlayers;
    }
}
