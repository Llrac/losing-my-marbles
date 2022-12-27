using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSession : MonoBehaviour
{
    public static int sessionID;
    public int numberOfPlayers;

    public void Start()
    {
        sessionID = Random.Range(0, 999999);
    }
}
