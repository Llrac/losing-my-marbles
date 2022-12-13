using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIntent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ShowIntent(Sprite intent)
    {
        spriteRenderer.sprite = intent;
    }
    public void HideIntent()
    {
        spriteRenderer.sprite = null;
    }
}
