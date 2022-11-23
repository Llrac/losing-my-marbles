using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    [HideInInspector] public bool hasBeenClicked = false;
    [HideInInspector] public bool isInHand = false;

    [HideInInspector] public int handIndex = 0;
    [HideInInspector] public int orderID = 0;

    MarbleManager mm;

    private void Start()
    {
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
        mm.discardPile.Add(this);
        transform.position = mm.marbleBagTransform.position;
    }
}
