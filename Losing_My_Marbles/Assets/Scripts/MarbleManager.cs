using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarbleManager : MonoBehaviour
{
    [HideInInspector] public int globalOrderID = 0;
    public bool[] availableMarbleSlots;
    public TextMeshProUGUI marblesInDeckText;
    public TextMeshProUGUI marblesInDiscardPileText;
    public List<Marble> marbleList = new();
    public Transform[] marbleSlots;

    [HideInInspector] public List<Marble> discardList = new();

    public GameObject highlight;
    public Transform marbleBagTransform;

    MarbleEffects me;

    private void Start()
    {
        me = FindObjectOfType<MarbleEffects>();
    }

    void Update()
    {
        marblesInDeckText.text = marbleList.Count.ToString();
        marblesInDiscardPileText.text = discardList.Count.ToString();
    }

    public void FillHandWithMarbles()
    {
        for (int i = 0; i < availableMarbleSlots.Length; i++)
        {
            if (marbleList.Count <= 0)
            {
                Shuffle();
                i--;
                if (marbleList.Count <= 0)
                {
                    return;
                }
            }
            if (availableMarbleSlots[i])
            {
                Marble randomMarble = marbleList[Random.Range(0, marbleList.Count)];
                randomMarble.handIndex = i;
                randomMarble.transform.position = marbleSlots[i].position;
                randomMarble.hasBeenClicked = false;
                randomMarble.isInHand = true;
                availableMarbleSlots[i] = false;
                marbleList.Remove(randomMarble);
            }
        }
    }

    public void Shuffle()
    {
        if (discardList.Count >= 1)
        {
            foreach (Marble marble in discardList)
            {
                marbleList.Add(marble);
            }
            discardList.Clear();
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

            me.marblesToTriggerList.Add(marbleToHighlight);

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

                me.TriggerMarbles();

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
        me.marblesToTriggerList.Clear();
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
