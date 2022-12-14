using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intent : MonoBehaviour
{
    public List<Sprite> listOfMarbleSprites = new List<Sprite>();
    public static List<Sprite> intents = new List<Sprite>();
    // Start is called before the first frame update
    private void Awake()
    {
        intents = listOfMarbleSprites;
    }
    public static Sprite GiveIntent(int marbleId)
    {
        return intents[marbleId-1];
    }

}
