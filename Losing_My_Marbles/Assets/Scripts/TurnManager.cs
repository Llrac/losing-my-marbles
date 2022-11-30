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

    [HideInInspector] public GameObject[] marblesToExecute = new GameObject[5];
    [HideInInspector] public List<GameObject> selectedMarbles = new();
    [HideInInspector] public int globalOrderID = 0;

    GameObject player;
    PlayerProperties pp;

    private void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
        player = pp.gameObject;
    }

    public void ResetOrder()
    {
        selectedMarbles.Clear();
        Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
        foreach (Marble marbleScript in allMarbleScripts)
        {
            marbleScript.hasBeenClicked = false;
            marbleScript.orderID = 0;
            globalOrderID = 0;
        }
        Highlight[] highlights = FindObjectsOfType<Highlight>();
        foreach (Highlight highlight in highlights)
        {
            Destroy(highlight.gameObject);
        }
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

        for (int i = 0; i < marblesToExecute.Length; i++)
        {
            MarbleToAction(marblesToExecute[i]);
        }
    }

    public void MarbleToAction(GameObject marbleToAction)
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
        }
    }

    public void RequestMove(GameObject character, int dataID, int increment, int callersDirectionID = 5)
    {
        //list spelare
        //
    }

    public IEnumerator TurnOrder()
    {


        yield return null;
    }
}
