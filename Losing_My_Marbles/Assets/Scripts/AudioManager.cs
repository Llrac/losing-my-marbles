using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background")]
    public AudioClip ambience = null;
    public AudioClip marbleMachine = null;

    [Header("Desktop FX")]
    public AudioClip playerJump = null;
    public AudioClip pickupMarble = null;
    public AudioClip dropKey = null;
    public AudioClip characterFall = null;
    public AudioClip pushHit = null;
    public AudioClip wallHit = null;
    public AudioClip playerReady = null;

    [Header("Special Marbles")]
    public AudioClip triggerAmplifier = null;
    public AudioClip triggerBlink = null;
    public AudioClip triggerBlock = null;
    public AudioClip triggerBomb = null;
    public AudioClip triggerDaze = null;
    public AudioClip triggerEarthquake = null;
    public AudioClip triggerRollerskates = null;
    public AudioClip triggerSwap = null;

    [Header("Menu Buttons")]
    public AudioClip onHoverEnter = null;
    public AudioClip onHoverExit = null;
    public AudioClip onClick = null;

    [Header("Mobile FX")]
    public AudioClip newMarbles = null;
    public AudioClip selectMarble = null;
    public AudioClip marblesReady = null;
    public AudioClip pressGo = null;

    private void UpdateBackgroundAudio()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<AudioSource>() != null)
            {
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (sceneName == "MainMenu")
                {
                    child.gameObject.GetComponent<AudioSource>().Play();
                    return;
                }
                if (sceneName != "Mobile Interface")
                {
                    child.gameObject.GetComponent<AudioSource>().Stop();
                    child.gameObject.GetComponent<AudioSource>().PlayOneShot(ambience);

                }
            }
        }
    }

    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (AudioManager)FindObjectOfType(typeof(AudioManager));
            }
            return instance;
        }
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        Instance.UpdateBackgroundAudio();
    }
}
