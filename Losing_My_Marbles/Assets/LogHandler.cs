using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform messagesContainer;
    public DatabaseAPI database;
    
    private void Start()
    {
        database.ListenForActions(InstantiateMessage, Debug.Log);
    }
    
    private void InstantiateMessage(ActionMessage message)
    {
        if (message != null)
        {
            var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
            newMessage.transform.SetParent(messagesContainer, false);
            newMessage.GetComponent<TextMeshProUGUI>().text = message.playerID.ToString();
        }
    }
}
