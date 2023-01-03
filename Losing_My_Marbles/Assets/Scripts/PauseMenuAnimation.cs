using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimation : MonoBehaviour
{
    public void StartMainMenuAnim()
    {
        
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Main_Menu_Shake");
    }
    public void StopMainMenuAnim()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Main_Menu_Stop");
    }
    public void StartResumeAnim()
    {
        gameObject.transform.GetChild(1).GetComponent<Animator>().Play("Resume_Shake");
    }
    public void StopResumeAnim()
    {
        gameObject.transform.GetChild(1).GetComponent<Animator>().Play("Resume_Stop");
    }
    public void StartRestartAnim()
    {
        gameObject.transform.GetChild(2).GetComponent<Animator>().Play("Restart_Shake");
    }
    public void StopRestartAnim()
    {
        gameObject.transform.GetChild(2).GetComponent<Animator>().Play("Restart_Stop");
    }

}
