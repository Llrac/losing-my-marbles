using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private GameObject creditsGO;
    public DatabaseAPI databaseAPI;
    private TextMeshProUGUI creditsText;
    private void Start()
    {
        //creditsText = creditsGO.GetComponent<TextMeshProUGUI>();
    }
    public void AskPlay()
    {
        FindObjectOfType<GameSession>().CreateSession();
        FindObjectOfType<Scenehandler>().LoadDesktopMatchmaking();
        //SceneManager.LoadScene(1);
    }
    public void ShowCredits()
    {
        if (creditsText.enabled == false)
        {
            creditsText.enabled = true;
        }
        else
        {
            creditsText.enabled = false;
        }
    } 

    public void ShowOptions()
    {

    }

    public void AskExit()
    {

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
}
