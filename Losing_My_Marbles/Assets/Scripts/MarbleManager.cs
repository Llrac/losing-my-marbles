using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class MarbleManager : MonoBehaviour
{
    [Header("Marbles & Slots")]
    public Transform[] marbleSlotsTop = new Transform[7];
    public Transform[] marbleSlotsBottom = new Transform[5];
    public List<Marble> marbleBag = new();
    public Transform marbleBagTransform;

    public GameObject highlight;

    public bool[] availableMarbleSlotsTop = new bool[7];
    [HideInInspector] public bool[] availableMarbleSlotsBottom = new bool[5];
    [HideInInspector] public List<Marble> discardBag = new();

    TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            availableMarbleSlotsTop[i] = true;
        }
        
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            availableMarbleSlotsBottom[i] = true;
        }

        //FillHandWithMarbles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FillHandWithMarbles();
        }
    }

    public void FillHandWithMarbles()
    {
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            if (marbleBag.Count <= 0)
            {
                Shuffle();
                i--;
                if (marbleBag.Count <= 0)
                {
                    return;
                }
            }
            else if (availableMarbleSlotsTop[i])
            {
                Marble randomMarble = marbleBag[Random.Range(0, marbleBag.Count)];
                randomMarble.handIndex = i;
                randomMarble.transform.position = marbleSlotsTop[i].position;
                randomMarble.isInHand = true;
                availableMarbleSlotsTop[i] = false;
                marbleBag.Remove(randomMarble);
            }
        }
    }

    public bool MoveMarbleToBottomRow(GameObject marble)
    {
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            if (availableMarbleSlotsBottom[i])
            {
                marble.transform.position = marbleSlotsBottom[i].position;
                marble.GetComponent<Marble>().bottomRowIndex = i;
                availableMarbleSlotsBottom[i] = false;
                availableMarbleSlotsTop[marble.GetComponent<Marble>().handIndex] = true;
                Debug.Log(marble.GetComponent<Marble>().handIndex);
                return true;
            }

        } 
        return false;
    }
    
    public bool MoveMarbleToTopRow(GameObject marble)
    {
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            if (availableMarbleSlotsTop[i])
            {
                marble.transform.position = marbleSlotsTop[i].position;
                marble.GetComponent<Marble>().handIndex = i;
                availableMarbleSlotsTop[i] = false;
                availableMarbleSlotsBottom[marble.GetComponent<Marble>().bottomRowIndex] = true;
                return false;
            }
        }
        return true;
    }

    public void Shuffle()
    {
        if (discardBag.Count >= 1)
        {
            foreach (Marble marble in discardBag)
            {
                marbleBag.Add(marble);
            }
            
            discardBag.Clear();
        }
    }

    public void GetHighlight(GameObject marbleToHighlight)
    {
        Marble marbleToHighlightScript = marbleToHighlight.GetComponent<Marble>();
        if (!marbleToHighlightScript.isOnBottomRow)
        {
            GameObject newHighlight = Instantiate(highlight, marbleToHighlight.transform);
            newHighlight.transform.position = marbleToHighlight.transform.position;

            turnManager.globalOrderID++;
            marbleToHighlightScript.orderID += turnManager.globalOrderID;
            marbleToHighlightScript.isOnBottomRow = true;

            turnManager.selectedMarbles.Add(marbleToHighlight);

            if (turnManager.globalOrderID >= 5)
            {
                Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
                foreach (Marble marbleScript in allMarbleScripts)
                {
                    if (marbleScript.isInHand)
                        marbleScript.MoveToDiscardPile();
                }
                for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
                {
                    availableMarbleSlotsTop[i] = true;
                }

                turnManager.SelectedMarbles();

                turnManager.ResetOrder();
            }
        }
        else
        {
            turnManager.ResetOrder();
        }
    }

    

    
}
