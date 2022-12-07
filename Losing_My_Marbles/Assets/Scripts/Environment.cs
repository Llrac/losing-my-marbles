using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<WaterFlowDecider> waterFlowDeciders = new List<WaterFlowDecider>();
    public static void Turn()
    {
        int waterFlow = 0;
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if(TurnManager.players[i].savedTile == 'W')
            {
                
                for (int j = 0; j < waterFlowDeciders.Count; j++)
                {
                    if (TurnManager.players[i].gridPosition == waterFlowDeciders[j].gridPos)
                    {
                        waterFlow = waterFlowDeciders[j].flowDirection;
                        Debug.Log("hej");
                        break;
                    }
                }
                Debug.Log(waterFlow);
                TurnManager.players[i].Pushed(waterFlow);
            }
           
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Turn();
        }
    }
}
