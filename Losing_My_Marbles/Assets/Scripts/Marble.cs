using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public int marbleID = 1;

    [HideInInspector] public int topRowIndex = 0;
    [HideInInspector] public int bottomRowIndex = 0;

    public int orderID;
    
    public bool isInHand = false;
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
    
}
