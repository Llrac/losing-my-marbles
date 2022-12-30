using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public List<Sprite> winScreens = new();
    public static List<Sprite> Screens = new();
    public DatabaseAPI databaseAPI;
    [SerializeField] private GameObject winImage = null;
    private static Image win;
    private float timer = 2;
    bool starting = false;
    private void Start()
    {
        if (winImage == null)
        {
            Image[] images = FindObjectsOfType<Image>();
            foreach (Image image in images)
            {
                if (image.name == "Win_Image")
                {
                    winImage = image.gameObject;
                }
            }
        }

        Screens = winScreens;

        if (winImage != null)
            win = winImage.GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //pause game
            PauseGame();
            
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetValues();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ResetLevel()
    {
        ResetValues();
        SceneManager.LoadScene("MainMenu");
    }

    public static void PlayerWin(int playerID)
    {
        if (win != null)
        {
            win.sprite = Screens[playerID - 1];
            win.enabled = true;
        }
    }
    public void PauseGame()
    {
        GameObject pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen");
        if (TurnManager.isPaused == false)
        {
            TurnManager.isPaused = true;
            pauseScreen.GetComponent<Image>().enabled = true;
            foreach(Transform child in pauseScreen.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            TurnManager.isPaused = false;
            pauseScreen.GetComponent<Image>().enabled = false;
            foreach (Transform child in pauseScreen.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void NextLevel()
    {
        ResetValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void ResetValues()
    {
        PlayerProperties.ids.Clear();
        PlayerProperties.myActions.Clear();
        TurnManager.sortedPlayers.Clear();
        TurnManager.players.Clear();
        DebugManager.characterToControl = 1;
        DatabaseAPI.hasBeenRestarted = true;
    }
}