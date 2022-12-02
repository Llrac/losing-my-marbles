using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // turn order

    // player 1, 2, 3, 4

    // enemy 1, 2, 3, etc
    // all hazard tiles
    // all environment tiles

    public GameObject[] marblesToExecute = new GameObject[5];
    public List<GameObject> selectedMarbles = new();
    [HideInInspector] public int globalOrderID = 0;

    GameObject player;
    PlayerProperties pp;

    private void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
        player = pp.gameObject;
    }

    public void OrderSelectedMarbles()
    {
        Debug.Log(globalOrderID);
        if (globalOrderID < 5)
        {
            return;
        }
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

                    break;
            }
        }

        for (int i = 0; i < marblesToExecute.Length; i++)
        {
            Debug.Log(marblesToExecute[i]);
            MarbleIDToAction(marblesToExecute[i]);
        }

        //ResetOrder();
    }

    public void ResetOrder()
    {
        selectedMarbles.Clear();
        Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
        foreach (Marble marbleScript in allMarbleScripts)
        {
            //marbleScript.hasBeenClicked = false;
            marbleScript.orderID = 0;
            globalOrderID = 0;
        }
        //Highlight[] highlights = FindObjectsOfType<Highlight>();
        //foreach (Highlight highlight in highlights)
        //{
        //    Destroy(highlight.gameObject);
        //}
    }

    public void MarbleIDToAction(GameObject marbleToAction)
    {
        switch (marbleToAction.GetComponent<Marble>().marbleID)
        {
            case 1:
                pp.TryMove(player, 0, 1);
                break;
            case 2:
                pp.TryMove(player, 0, 2);
                break;
            case 3:
                pp.TryMove(player, 0, 3);
                break;
            case 4:
                pp.TryMove(player, 1, -1);
                break;
            case 5:
                pp.TryMove(player, 1, 1);
                break;
            default:
                Debug.Log(gameObject + " has an unknown marble ID.");
                break;
        }
    }
}
