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

    public Button confirmButton;
    
    [HideInInspector] public bool[] availableMarbleSlotsTop = new bool[7];
    [HideInInspector] public bool[] availableMarbleSlotsBottom = new bool[5];
    [HideInInspector] public List<Marble> discardBag = new();
    [HideInInspector] public int[] orderID = new int[5];

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

        FillHandWithMarbles();
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
                randomMarble.topRowIndex = i;
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
                Marble currentMarble = marble.GetComponent<Marble>();
                currentMarble.transform.position = marbleSlotsBottom[i].position;
                currentMarble.bottomRowIndex = i;
                availableMarbleSlotsBottom[i] = false;
                orderID[i] = currentMarble.marbleID;
                availableMarbleSlotsTop[currentMarble.topRowIndex] = true;
                confirmButton.interactable = BottomRowFull();
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
                Marble currentMarble = marble.GetComponent<Marble>();
                currentMarble.transform.position = marbleSlotsTop[i].position;
                currentMarble.topRowIndex = i;
                availableMarbleSlotsTop[i] = false;
                availableMarbleSlotsBottom[currentMarble.bottomRowIndex] = true;
                confirmButton.interactable = BottomRowFull();
                
                return false;
            }
        }
        
        return true;
    }

    private bool BottomRowFull()
    {
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            if (availableMarbleSlotsBottom[i])
            {
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
}
