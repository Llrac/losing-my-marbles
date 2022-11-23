using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Marble> marbleBag = new();
    public List<Marble> discardPile = new();
    public Transform[] marbleSlots;
    public Transform marbleBagTransform;
    public bool[] availableMarbleSlots;

    public TextMeshProUGUI bagSizeText;
    public TextMeshProUGUI discardPileText;

    private void Update()
    {
        bagSizeText.text = marbleBag.Count.ToString();
        discardPileText.text = discardPile.Count.ToString();
    }

    public void FillHandWithMarbles()
    {
        for (int i = 0; i < availableMarbleSlots.Length; i++)
        {
            if (marbleBag.Count <= 0)
            {
                Shuffle();
                i--;
            }
            if (availableMarbleSlots[i])
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
        if (discardPile.Count >= 1)
        {
            foreach (Marble marble in discardPile)
            {
                marbleBag.Add(marble);
            }
            discardPile.Clear();
        }
    }
}
