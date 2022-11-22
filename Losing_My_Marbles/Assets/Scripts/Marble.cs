using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public bool hasBeenSelected = false;
    public bool hasHighlight = false;

    public int handIndex;

    GameManager gm;
    MarbleManager mm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        mm = FindObjectOfType<MarbleManager>();
    }

    public void OnMouseDown()
    {
        if (!hasBeenSelected)
        {
            transform.position += Vector3.up * 1.25f;
            hasBeenSelected = true;
            hasHighlight = true;
            mm.GetHighlight(this.gameObject);
        }
        if (hasBeenSelected)
        {
            transform.position -= Vector3.up * 1.25f;
            hasBeenSelected = false;
            hasHighlight = false;
            mm.GetHighlight(this.gameObject);
        }
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
