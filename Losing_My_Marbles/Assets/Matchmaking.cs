using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Matchmaking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TMP_Text>().text = Convert.ToString(DatabaseAPI.matchMakingCode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
