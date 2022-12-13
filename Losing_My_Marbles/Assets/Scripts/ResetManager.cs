using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public List<Sprite> winScreens = new List<Sprite> ();
    public static List<Sprite> Screens = new List<Sprite> ();
    [SerializeField] private GameObject winImage;
    private static Image win;
    private void Start()
    {
        Screens = winScreens;
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
        win.sprite = Screens[playerID-1];
        win.enabled = true;
    }

}
