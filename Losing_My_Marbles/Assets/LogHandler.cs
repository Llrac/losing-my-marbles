using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform messagesContainer;


    public void InstantiateMessage(int messageIndex)
    {
        var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(messagesContainer, false);
        newMessage.GetComponent<TextMeshProUGUI>().text = PlayerProperties.ids[messageIndex].ToString();
        Debug.Log("Player " + (PlayerProperties.ids[messageIndex]) + " has locked in");
    }

}
