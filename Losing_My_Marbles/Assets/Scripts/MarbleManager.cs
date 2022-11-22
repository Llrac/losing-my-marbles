using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    public GameObject highlight;
    int globalOrderID = 0;

    public void GetHighlight(GameObject marbleToHighlight)
    {
        Marble marbleToHighlightScript = marbleToHighlight.GetComponent<Marble>();
        if (!marbleToHighlightScript.hasHighlight)
        {
            GameObject newHighlight = Instantiate(highlight, marbleToHighlight.transform);
            newHighlight.transform.position = marbleToHighlight.transform.position;

            globalOrderID++;
            marbleToHighlightScript.orderID += globalOrderID;

            if (globalOrderID >= 5)
            {
                PrepareForNextTurn();
            }
        }
        else
        {
            PrepareForNextTurn();
        }
    }

    public void PrepareForNextTurn()
    {
        Marble[] marbleScripts = FindObjectsOfType<Marble>();
        foreach (Marble marbleScript in marbleScripts)
        {
            marbleScript.hasBeenSelected = false;
            marbleScript.hasHighlight = false;
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
