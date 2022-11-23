using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    public GameObject highlight;
    public int globalOrderID = 0;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void GetHighlight(GameObject marbleToHighlight)
    {
        Marble marbleToHighlightScript = marbleToHighlight.GetComponent<Marble>();
        if (!marbleToHighlightScript.hasBeenClicked)
        {
            GameObject newHighlight = Instantiate(highlight, marbleToHighlight.transform);
            newHighlight.transform.position = marbleToHighlight.transform.position;

            globalOrderID++;
            marbleToHighlightScript.orderID += globalOrderID;
            marbleToHighlightScript.hasBeenClicked = true;

            if (globalOrderID >= 5)
            {
                ResetOrder();
                Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
                foreach (Marble marbleScript in allMarbleScripts)
                {
                    if (marbleScript.isInHand)
                        marbleScript.MoveToDiscardPile();
                }
                for (int i = 0; i < gm.availableMarbleSlots.Length; i++)
                {
                    gm.availableMarbleSlots[i] = true;
                }
            }
        }
        else
        {
            ResetOrder();
        }
    }

    public void ResetOrder()
    {
        Debug.Log("resetorder");
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
}
