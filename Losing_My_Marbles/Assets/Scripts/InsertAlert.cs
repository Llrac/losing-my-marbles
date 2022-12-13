using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertAlert : MonoBehaviour
{
    public Image insertAlertMessage;
    
    void Start()
    {
        InsertMessage();
    }

    public void InsertMessage()
    {
        SetAlertToActive();
        Invoke(nameof(SetAlertToInActive), 2f);
    }

    private void SetAlertToInActive()
    {
        insertAlertMessage.enabled = false;
    }

    private void SetAlertToActive()
    {
        insertAlertMessage.enabled = true;
    }
    
    
}
