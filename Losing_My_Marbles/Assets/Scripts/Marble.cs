using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public bool hasBeenSelected;

    public int handIndex;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void OnMouseDown()
    {
        if (!hasBeenSelected)
        {
            transform.position += Vector3.up * 1.25f;
            hasBeenSelected = true;
            gm.availableMarbleSlots[handIndex] = true;
        }
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
