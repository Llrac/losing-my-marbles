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
