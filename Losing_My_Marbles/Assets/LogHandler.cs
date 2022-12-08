using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform messagesContainer;
    public PlayerProperties playerProperties;
    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void InstantiateMessage(int message)
    {
        var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(messagesContainer, false);
        newMessage.GetComponent<TextMeshProUGUI>().text = message.ToString();
    }
}
