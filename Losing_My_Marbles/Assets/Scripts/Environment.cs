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
                int savedDiD = TurnManager.players[i].currentDirectionID;
                TurnManager.players[i].currentDirectionID = waterFlow;
                TurnManager.players[i].TryMove(TurnManager.players[i].gameObject, 0, 1);
                TurnManager.players[i].currentDirectionID = savedDiD;
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
