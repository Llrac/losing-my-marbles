using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntent : MonoBehaviour
{
    GameObject cross;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (Transform childInPlayer in GetComponentInParent<Movement>().gameObject.transform)
        {
            if (childInPlayer.name == "Cross" && childInPlayer.GetComponent<SpriteRenderer>() != null)
            {
                cross = childInPlayer.gameObject;
            }
        }
    }
    public void ShowIntent(Sprite intent, bool displayCross)
    {
        spriteRenderer.sprite = intent;
        if (displayCross && cross != null)
        {
            cross.SetActive(true);
        }
    }
    public void HideIntent()
    {
        spriteRenderer.sprite = null;
        if (cross != null)
            cross.SetActive(false);
    }
}
