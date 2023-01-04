using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject creditsGO;
    public DatabaseAPI databaseAPI;
    private TextMeshProUGUI creditsText;
    private void Start()
    {
        creditsText = creditsGO.GetComponent<TextMeshProUGUI>();
    }
    public void StartGame()
    {
        GameSession.activePlayers = 0;
        SceneManager.LoadScene(1);
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
    public void ShowHowTo()
    {

    }
   
}
