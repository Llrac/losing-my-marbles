using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    public GameObject background;
    public List<Sprite> backgroundColor = new();

    private void Start()
    {
        background.GetComponent<Image>().sprite = background.GetComponent<PlayerColor>().backgroundColor[PlayerID.playerID - 1];
    }
}
