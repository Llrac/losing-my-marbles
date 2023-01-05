
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckMatchedGames : MonoBehaviour
{
    public static bool matchedGame = false;
    
    public void Update()
    {
        if (matchedGame)
        {
            Debug.Log(matchedGame);
            matchedGame = false;
            GameSession.activePlayers = 0;
            SceneManager.LoadScene("level1");
        }
        
    }
}
