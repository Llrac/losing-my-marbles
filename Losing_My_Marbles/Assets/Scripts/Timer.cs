using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeValue;
    public float playerTime = 5f;

    public bool timerOn;

    private void Start()
    {
        timeValue = playerTime;
        timerOn = true;
    }

    private void Update()
    {
        if (timeValue > 0 && timerOn)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }

        DisplayTime(timeValue);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        if (timeToDisplay <= 10f)
        {
            timerText.color = Color.red;
        }
        
        float minutes = timeValue / 60;
        float seconds = timeValue % 60;
        
        
        timerText.text = string.Format("{0:00}", seconds);
    }

    public void ResetTimer()
    {
        timeValue = playerTime;
        timerText.color = Color.white;
    }
}
