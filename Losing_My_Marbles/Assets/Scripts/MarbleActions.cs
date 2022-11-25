using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleActions : MonoBehaviour
{
    public List<GameObject> selectedMarbles = new();

    public GameObject[] marblesToExecute = new GameObject[5];

    GameObject player;
    PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        player = pm.gameObject;
    }

    public void SelectedMarbles()
    {
        foreach (GameObject marble in selectedMarbles)
        {
            switch (marble.GetComponent<Marble>().orderID)
            {
                case 1:
                    marblesToExecute[0] = marble;
                    break;
                case 2:
                    marblesToExecute[1] = marble;
                    break;
                case 3:
                    marblesToExecute[2] = marble;
                    break;
                case 4:
                    marblesToExecute[3] = marble;
                    break;
                case 5:
                    marblesToExecute[4] = marble;
                    break;
                default:
                    Debug.Log(gameObject + " has no marble ID.");
                    break;
            }
        }

        MarbleToAction(marblesToExecute[0]);
        MarbleToAction(marblesToExecute[1]);
        MarbleToAction(marblesToExecute[2]);
        MarbleToAction(marblesToExecute[3]);
        MarbleToAction(marblesToExecute[4]);
    }

    public void MarbleToAction(GameObject marbleToAction)
    {
        switch (marbleToAction.GetComponent<Marble>().marbleID)
        {
            case 1:
                pm.UpdatePlayerProperties(player, 0, 0);
                Debug.Log("move1");
                break;
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    pm.UpdatePlayerProperties(player, 0, 0);
                }
                Debug.Log("move2");
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    pm.UpdatePlayerProperties(player, 0, 0);
                }
                Debug.Log("move3");
                break;
            case 4:
                pm.UpdatePlayerProperties(player, -1, 1);
                Debug.Log("turnL");
                break;
            case 5:
                pm.UpdatePlayerProperties(player, 1, 1);
                Debug.Log("turnR");
                break;
        }
    }
}
