using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public bool hasBeenClicked = false;
    public bool isInHand = false;

    public int handIndex = 0;
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
        if (!hasBeenClicked)
        {
            mm.GetHighlight(gameObject);
        }
        else if (hasBeenClicked)
        {
            mm.GetHighlight(gameObject);
        }
    }

    public void MoveToDiscardPile()
    {
        isInHand = false;
        gm.discardPile.Add(this);
        transform.position = gm.marbleBagTransform.position;
    }
}
