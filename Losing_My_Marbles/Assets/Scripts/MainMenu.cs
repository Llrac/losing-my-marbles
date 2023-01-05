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
    Animator arrow;
    private static bool audioIsMuted = false;
    float audioTimer = 0;
    enum ArrowStates { options, howtoplay, disable }
    ArrowStates currentArrowState = ArrowStates.options;

    private void Start()
    {
        Application.targetFrameRate = 60;

        arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<Animator>();
    }

    private void Update()
    {
        audioTimer += Time.deltaTime * Time.deltaTime * Application.targetFrameRate;
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
        if (audioTimer >= 0.1f)
        {
            audioTimer = 0;
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onHoverEnter);
        }
    }

    public void Play()
    {
        FindObjectOfType<GameSession>().CreateSession();
        FindObjectOfType<Scenehandler>().LoadDesktopMatchmaking();
    }

    public void ShowOptions()
    {
        if (!optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(true);
            creditsPanel.SetActive(false);

            if (currentArrowState == ArrowStates.options)
            {
                currentArrowState = ArrowStates.howtoplay;
                arrow.SetTrigger("howtoplay");
            }
        }
        else
        {
            optionsPanel.SetActive(false);

            if (currentArrowState == ArrowStates.howtoplay)
            {
                currentArrowState = ArrowStates.options;
                arrow.SetTrigger("options");
            }
        }
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void ShowHTP(GameObject howToPlay)
    {
        GameObject button = null;
        foreach (GameObject buttonInScene in GameObject.FindGameObjectsWithTag("MenuButton"))
        {
            foreach (Transform child in buttonInScene.transform)
            {
                if (child.GetComponent<Button>() != null)
                {
                    button = child.gameObject;
                }
            }
        }
        Animator anim = howToPlay.GetComponent<Animator>();
        if (!anim.GetBool("isShowing"))
        {
            anim.SetTrigger("show");
            anim.SetBool("isShowing", true);
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Stop();
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Clear();
            button.GetComponent<Button>().enabled = false;

            if (currentArrowState == ArrowStates.howtoplay)
            {
                currentArrowState = ArrowStates.disable;
                arrow.SetTrigger("disable");
            }
        }
        else
        {
            anim.SetTrigger("back");
            anim.SetBool("isShowing", false);
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Play();
            button.GetComponent<Button>().enabled = true;
        }
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void ShowCredits()
    {
        if (!creditsPanel.activeSelf)
        {
            creditsPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        else
        {
            creditsPanel.SetActive(false);

            if (currentArrowState == ArrowStates.howtoplay)
            {
                currentArrowState = ArrowStates.options;
            }
        }
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void AskExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
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
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerDaze);
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
