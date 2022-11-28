using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public int marbleID = 0;

    [HideInInspector] public int handIndex = 0;
    [HideInInspector] public int orderID = 0;

    [HideInInspector] public bool isInHand = false;
    [HideInInspector] public bool hasBeenClicked = false;

    MarbleManager mm;

    private void Start()
    {
        transform.localScale = new Vector3(Camera.main.orthographicSize / 10,
            Camera.main.orthographicSize / 10, Camera.main.orthographicSize / 10);
        mm = FindObjectOfType<MarbleManager>();
    }

    public void OnMouseDown()
    {
        if (!hasBeenClicked)
        {
            mm.GetHighlight(gameObject);
        }
        else if (hasBeenClicked)
        {
            mm.GetHighlight(gameObject);
        }
    }

    public void MoveToDiscardPile()
    {
        isInHand = false;
        mm.discardBag.Add(this);
        transform.position = mm.marbleBagTransform.position;
    }
}
