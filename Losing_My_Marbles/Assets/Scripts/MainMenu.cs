using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    //[SerializeField] private GameObject creditsGO;
    public DatabaseAPI databaseAPI;
    public Image credits;
    [SerializeField] private GameObject optionsPanel;
    private static bool audioIsMuted = false;
   
    public void AskPlay()
    {
        FindObjectOfType<GameSession>().CreateSession();
        FindObjectOfType<Scenehandler>().LoadDesktopMatchmaking();
        //SceneManager.LoadScene(1);
    }
    public void ShowCredits()
    {
        if(credits.enabled == true)
        {
            credits.enabled = false;
        }
        else
        {
            credits.enabled = true;
        }
    } 

    public void ShowOptions()
    {
        if (optionsPanel.activeSelf == false)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
    }

    public void AskExit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OnHover(GameObject button)
    {
        foreach (Transform child in button.transform)
        {
            if (child.gameObject.name == "ON")
            {
                child.gameObject.SetActive(true);
            }
            else if (child.name == "OFF")
            {
                child.gameObject.SetActive(false);
            }
        }

        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onHoverEnter);
        
    }

    public void OnExit(GameObject button)
    {
        foreach (Transform child in button.transform)
        {
            if (child.gameObject.name == "ON")
            {
                child.gameObject.SetActive(false);
            }
            else if (child.name == "OFF")
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    public void MuteAllAudio()
    {
        if(audioIsMuted == false)
        {
            audioIsMuted = true;
            AudioListener.volume = 0;
        }
        else
        {
            audioIsMuted = false;
            AudioListener.volume = 1;
        }

        
    }
}
