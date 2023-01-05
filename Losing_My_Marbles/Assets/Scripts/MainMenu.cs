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
    public GameObject exitPanel;
    Animator arrow;
    private static bool audioIsMuted = false;
    float audioTimer = 0;
    enum ArrowStates { options, howtoplay, disable }
    ArrowStates currentArrowState = ArrowStates.options;

    private void Start()
    {
        Application.targetFrameRate = 60;

        arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<Animator>();

        creditsPanel.SetActive(true);
        optionsPanel.SetActive(true);
        exitPanel.SetActive(true);
    }

    private void Update()
    {
        audioTimer += Time.deltaTime * Time.deltaTime * Application.targetFrameRate;
    }

    public void OnHoverEnter(GameObject button)
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

    public void OnHoverExit(GameObject button)
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

    public void Play()
    {

        FindObjectOfType<GameSession>().CreateSession();
        FindObjectOfType<Scenehandler>().LoadDesktopMatchmaking();
    }

    public void ShowOptions()
    {
        if (!optionsPanel.GetComponent<Animator>().GetBool("isShowing"))
        {
            optionsPanel.GetComponent<Animator>().SetBool("isShowing", true);
            optionsPanel.GetComponent<Animator>().SetTrigger("show");

            if (creditsPanel.GetComponent<Animator>().GetBool("isShowing"))
            {
                creditsPanel.GetComponent<Animator>().SetBool("isShowing", false);
                creditsPanel.GetComponent<Animator>().SetTrigger("hide");
            }

            if (currentArrowState == ArrowStates.options)
            {
                currentArrowState = ArrowStates.howtoplay;
                arrow.SetTrigger("howtoplay");
            }
        }
        else
        {
            optionsPanel.GetComponent<Animator>().SetBool("isShowing", false);
            optionsPanel.GetComponent<Animator>().SetTrigger("hide");

            if (currentArrowState == ArrowStates.howtoplay)
            {
                currentArrowState = ArrowStates.options;
                arrow.SetTrigger("options");
            }
        }

        AskExit(true);

        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void ShowHTP(GameObject howToPlay)
    {
        AskExit(true);

        Animator anim = howToPlay.GetComponent<Animator>();
        if (!anim.GetBool("isShowing"))
        {
            anim.SetTrigger("show");
            anim.SetBool("isShowing", true);
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Stop();
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Clear();

            if (currentArrowState == ArrowStates.howtoplay)
            {
                currentArrowState = ArrowStates.disable;
                arrow.SetTrigger("disable");
            }
        }
        else
        {
            anim.SetTrigger("hide");
            anim.SetBool("isShowing", false);
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Play();
        }
    }

    public void ShowCredits()
    {
        if (!creditsPanel.GetComponent<Animator>().GetBool("isShowing"))
        {
            creditsPanel.GetComponent<Animator>().SetBool("isShowing", true);
            creditsPanel.GetComponent<Animator>().SetTrigger("show");

            if (optionsPanel.GetComponent<Animator>().GetBool("isShowing"))
            {
                optionsPanel.GetComponent<Animator>().SetBool("isShowing", false);
                optionsPanel.GetComponent<Animator>().SetTrigger("hide");
            }
        }
        else
        {
            creditsPanel.GetComponent<Animator>().SetBool("isShowing", false);
            creditsPanel.GetComponent<Animator>().SetTrigger("hide");

        }
        if (currentArrowState == ArrowStates.howtoplay)
        {
            currentArrowState = ArrowStates.options;
            arrow.SetTrigger("options");
        }

        AskExit(true);

        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void AskExit(bool hide = false)
    {
        if (exitPanel.GetComponent<Animator>().GetBool("isShowing"))
        {
            exitPanel.GetComponent<Animator>().SetBool("isShowing", false);
            exitPanel.GetComponent<Animator>().SetTrigger("hide");
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Play();
            if (currentArrowState == ArrowStates.options)
            {
                arrow.SetTrigger("options");
            }
            else if (currentArrowState == ArrowStates.howtoplay)
            {
                arrow.SetTrigger("howtoplay");
            }
        }
        else if (!hide)
        {
            exitPanel.GetComponent<Animator>().SetBool("isShowing", true);
            exitPanel.GetComponent<Animator>().SetTrigger("show");
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Stop();
            GameObject.FindGameObjectWithTag("Special Marble").GetComponent<ParticleSystem>().Clear();
            arrow.SetTrigger("disable");
        }
        if (!hide)
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
    }

    public void ExitApplication()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().onClick);
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
