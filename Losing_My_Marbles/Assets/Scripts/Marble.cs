using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public int marbleID = 0;

    [HideInInspector] public int handIndex = 0;
    public int orderID = 0;

    [HideInInspector] public bool isInHand = false;
    [HideInInspector] public bool hasBeenClicked = false;

    MarbleManager mm;

    private void Start()
    {
        mm = FindObjectOfType<MarbleManager>();
    }

    public void SelectMarble()
    {
        if (!hasBeenClicked)
        {
            Handheld.Vibrate();
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
        mm.discardBag.Add(this);
        transform.position = mm.marbleBagTransform.position;
    }
}
