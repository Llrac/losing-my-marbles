using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleActions : MonoBehaviour
{
    public List<GameObject> selectedMarbles = new();

    public GameObject[] marblesToExecute = new GameObject[5];

    GameObject player;
    PlayerProperties pp;

    void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
        player = pp.gameObject;
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
                pp.UpdateData(player, 0, 1);
                break;
            case 2:
                pp.UpdateData(player, 0, 2);
                break;
            case 3:
                pp.UpdateData(player, 0, 3);
                break;
            case 4:
                pp.UpdateData(player, 1, -1);
                break;
            case 5:
                pp.UpdateData(player, 1, 1);
                break;
        }
    }
}
