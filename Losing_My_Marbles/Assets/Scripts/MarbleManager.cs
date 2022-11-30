using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarbleManager : MonoBehaviour
{
    [Header("Marbles & Slots")]
    public Transform[] marbleSlots = new Transform[7];
    public List<Marble> marbleBag = new();
    public Transform marbleBagTransform;

    public GameObject highlight;

    [HideInInspector] public bool[] availableMarbleSlots = new bool[7];
    [HideInInspector] public List<Marble> discardBag = new();

    TurnManager tm;

    private void Start()
    {
        tm = FindObjectOfType<TurnManager>();
        for (int i = 0; i < availableMarbleSlots.Length; i++)
        {
            availableMarbleSlots[i] = true;
        }

        FillHandWithMarbles();
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

            tm.globalOrderID++;
            marbleToHighlightScript.orderID += tm.globalOrderID;
            marbleToHighlightScript.hasBeenClicked = true;

            tm.selectedMarbles.Add(marbleToHighlight);

            if (tm.globalOrderID >= 5)
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

                tm.SelectedMarbles();

                tm.ResetOrder();
            }
        }
        else
        {
            tm.ResetOrder();
        }
    }

    

    
}
