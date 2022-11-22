using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public bool hasBeenSelected = false;
    public bool hasHighlight = false;

    public int handIndex;

    public int orderID = 0;

    GameManager gm;
    MarbleManager mm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        mm = FindObjectOfType<MarbleManager>();
    }

    public void OnMouseDown()
    {
        if (!hasBeenSelected)
        {
            mm.GetHighlight(gameObject);
            hasBeenSelected = true;
            hasHighlight = true;
            Debug.Log(orderID);
        }
        else if (hasBeenSelected)
        {
            mm.GetHighlight(gameObject);
            hasBeenSelected = false;
            hasHighlight = false;
        }
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
