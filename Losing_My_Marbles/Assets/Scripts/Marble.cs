using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public int marbleID = 1;

    [HideInInspector] public int handIndex = 0;
    [HideInInspector] public int bottomRowIndex = 0;
    
    [HideInInspector] public int orderID = 0;

    [HideInInspector] public bool isInHand = false;
    [HideInInspector] public bool isOnBottomRow = false;

    MarbleManager marbleManager;

    private void Start()
    {
        marbleManager = FindObjectOfType<MarbleManager>();
    }

    public void SelectMarble()
    {
        //Handheld.Vibrate(); 
        if (!isOnBottomRow)
        {
            isOnBottomRow = marbleManager.MoveMarbleToBottomRow(gameObject);
        }
        else
        {
            isOnBottomRow = marbleManager.MoveMarbleToTopRow(gameObject);
        }
    }
    

    public void MoveToDiscardPile()
    {
        isInHand = false;
        marbleManager.discardBag.Add(this);
        transform.position = marbleManager.marbleBagTransform.position;
    }
}
