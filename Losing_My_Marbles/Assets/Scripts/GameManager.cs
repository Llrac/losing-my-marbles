using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Marble> deck = new();
    public List<Marble> discardPile = new();
    public Transform[] marbleSlots;
    public bool[] availableMarbleSlots;

    public TextMeshProUGUI deckSizeText;
    public TextMeshProUGUI discardPileText;

    private void Update()
    {
        deckSizeText.text = deck.Count.ToString();
        discardPileText.text = discardPile.Count.ToString();
    }

    public void DrawMarbles()
    {
        if (deck.Count >= 1)
        {
            Marble randomMarble = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableMarbleSlots.Length; i++)
            {
                if (availableMarbleSlots[i] == true)
                {
                    randomMarble.gameObject.SetActive(true);
                    randomMarble.handIndex = i;

                    randomMarble.transform.position = marbleSlots[i].position;
                    randomMarble.hasBeenPlayed = false;
                    availableMarbleSlots[i] = false;
                    deck.Remove(randomMarble);
                    return;
                }
            }
        }
    }

    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Marble marble in discardPile)
            {
                deck.Add(marble);
            }
            discardPile.Clear();
        }
    }
}
