using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
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
}
