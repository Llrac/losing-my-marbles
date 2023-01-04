using System;

[Serializable]

public class NewHandMessage
{
    public bool drawNewHand;

    public NewHandMessage(bool drawNewHand)
    {
        this.drawNewHand = drawNewHand;
    }
}
