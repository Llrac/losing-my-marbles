
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
            SceneManager.LoadScene("level1");
        }
        
    }
}
