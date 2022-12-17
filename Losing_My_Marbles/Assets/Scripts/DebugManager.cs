using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static int playerToControl = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerToControl = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerToControl = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerToControl = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerToControl = 4;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerToControl >= 5)
                playerToControl = 0;
            if (TurnManager.players.Count >= playerToControl + 1)
                playerToControl++;
            else
                playerToControl = 1;
            //Debug.Log("Player " + playerToControl);
        }
    }
}
