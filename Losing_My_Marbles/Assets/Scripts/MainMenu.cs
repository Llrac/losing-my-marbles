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
    public GameObject creditsPanel;
    public GameObject optionsPanel;
    private static bool audioIsMuted = false;
   
    public void Play()
    {
        FindObjectOfType<GameSession>().CreateSession();
        FindObjectOfType<Scenehandler>().LoadDesktopMatchmaking();
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }
    public void ShowCredits()
    {
        if (creditsPanel.activeSelf == false)
        {
            creditsPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        else
        {
            creditsPanel.SetActive(false);
        }
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    } 

    public void ShowOptions()
    {
        if (optionsPanel.activeSelf == false)
        {
            optionsPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void AskExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
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

    public void SecretButton(GameObject button)
    {
        button.GetComponent<Button>().interactable = false;
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
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
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }
}
