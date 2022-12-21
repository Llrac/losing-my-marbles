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
    [HideInInspector] public bool isOnTopRow;

    UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void SelectMarble()
    {
        //Handheld.Vibrate(); 
        if (!isOnBottomRow)
        {
            isOnBottomRow = uiManager.MoveMarbleToBottomRow(gameObject);
        }
        else
        {
            isOnBottomRow = uiManager.MoveMarbleToTopRow(gameObject);
        }
    }
    
}
