using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarbleManager : MonoBehaviour
{
    public bool[] availableMarbleSlots = new bool[7];
    public Transform[] marbleSlots;
    public GameObject highlight;
    public Transform marbleBagTransform;

    [HideInInspector] public int globalOrderID = 0;
    public List<Marble> marbleBag = new();
    [HideInInspector] public List<Marble> discardBag = new();

    MarbleActions ma;

    private void Start()
    {
        for (int i = 0; i < availableMarbleSlots.Length; i++)
        {
            availableMarbleSlots[i] = true;
        }
        ma = FindObjectOfType<MarbleActions>();
        
        FillHandWithMarbles();
    }

    void Update()
    {
        //marblesInMarbleBagText.text = marbleBag.Count.ToString();
        //marblesInDiscardBagText.text = discardBag.Count.ToString();
    }

    public void FillHandWithMarbles()
    {
        for (int i = 0; i < availableMarbleSlots.Length; i++)
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
            else if (availableMarbleSlots[i])
            {
                Marble randomMarble = marbleBag[Random.Range(0, marbleBag.Count)];
                randomMarble.handIndex = i;
                randomMarble.transform.position = marbleSlots[i].position;
                randomMarble.hasBeenClicked = false;
                randomMarble.isInHand = true;
                availableMarbleSlots[i] = false;
                marbleBag.Remove(randomMarble);
                Debug.Log("whatever");
            }
        }
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
        if (!marbleToHighlightScript.hasBeenClicked)
        {
            GameObject newHighlight = Instantiate(highlight, marbleToHighlight.transform);
            newHighlight.transform.position = marbleToHighlight.transform.position;

            globalOrderID++;
            marbleToHighlightScript.orderID += globalOrderID;
            marbleToHighlightScript.hasBeenClicked = true;

            ma.selectedMarbles.Add(marbleToHighlight);

            if (globalOrderID >= 5)
            {
                Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
                foreach (Marble marbleScript in allMarbleScripts)
                {
                    if (marbleScript.isInHand)
                        marbleScript.MoveToDiscardPile();
                }
                for (int i = 0; i < availableMarbleSlots.Length; i++)
                {
                    availableMarbleSlots[i] = true;
                }

                ma.SelectedMarbles();

                ResetOrder();
            }
        }
        else
        {
            ResetOrder();
        }
    }

    public void ResetOrder()
    {
        ma.selectedMarbles.Clear();
        Marble[] allMarbleScripts = FindObjectsOfType<Marble>();
        foreach (Marble marbleScript in allMarbleScripts)
        {
            marbleScript.hasBeenClicked = false;
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
