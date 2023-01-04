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
    private static float timer = 2;
    bool starting = false;
    public static List<int> levelOrder = new List<int>();
    static int tcurrentLevel = 0;
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
        if(tcurrentLevel == 0)
        {
            RandomizeLevels();
        }
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
            
            NextLevel();
        }
    }

   
    public void ResetLevel()
    {
        ResetScores();
        ResetValues();
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
       ResetScores();
       ResetValues();
       
       StartCoroutine(LS());
    }
    IEnumerator LS()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(1);
    }
    public static void PlayerWin(int playerID)
    {
        if (win != null)
        {
            win.sprite = Screens[playerID - 1];
            win.enabled = true;
        }
        TurnManager.isPaused = true;

        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if (playerID == TurnManager.players[i].playerID)
            {
                //do animation
            }
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
        ResetValues(false);
        tcurrentLevel++;
        try 
        { 
            SceneManager.LoadScene(levelOrder[tcurrentLevel]); 
        }
        catch (System.ArgumentOutOfRangeException)
        {
            ResetValues(true);
            SceneManager.LoadScene(1);
        }
    }
    private void ResetValues( bool shouldRandomizeLevels = true)
    {
        PlayerProperties.ids.Clear();
        PlayerProperties.myActions.Clear();
        TurnManager.sortedPlayers.Clear();
        TurnManager.players.Clear();
        DebugManager.characterToControl = 1;
        DatabaseAPI.hasBeenRestarted = true;
        TurnManager.isPaused = false;
        
        if(shouldRandomizeLevels == true)
        {
            tcurrentLevel = 0;
            RandomizeLevels();
        }
          
    }

    private void ResetScores()
    {
        PlayerProperties.scoreKeeper = new int[4]
        {
            0, 0, 0, 0
        };
        
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            TurnManager.players[i].specialMarbleCount = 0;
        }
    }
    
    public void RandomizeLevels()
    {
        GridManager gm = FindObjectOfType<GridManager>();

        levelOrder.Clear();

        while (levelOrder.Count < gm.levels.Length)
        {
            int randomLevel = Random.Range(1, gm.levels.Length+1);
            if (levelOrder.Contains(randomLevel) == false)
            {
                levelOrder.Add(randomLevel);
            }
        }
    }
}