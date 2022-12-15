using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public List<Sprite> winScreens = new();
    public static List<Sprite> Screens = new();
    [SerializeField] private GameObject winImage = null;
    private static Image win;

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
    }

    public void ResetLevel()
    {
        PlayerProperties.myActions.Clear();
        TurnManager.sortedPlayers.Clear();
        TurnManager.players.Clear();
        DebugManager.playerToControl = 1;
        
        SceneManager.LoadScene(0);
    }
    public static void PlayerWin(int playerID)
    {
        if (win != null)
        {
            win.sprite = Screens[playerID - 1];
            win.enabled = true;
        }
    }

}
