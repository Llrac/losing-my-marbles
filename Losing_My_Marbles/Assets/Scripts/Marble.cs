using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public bool hasBeenPlayed;

    public int handIndex;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void OnMouseDown()
    {
        if (!hasBeenPlayed)
        {
            transform.position += Vector3.up * 1.5f;
            hasBeenPlayed = true;
            gm.availableMarbleSlots[handIndex] = true;
            Invoke(nameof(MoveToDiscardPile), 2f);
        }
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
