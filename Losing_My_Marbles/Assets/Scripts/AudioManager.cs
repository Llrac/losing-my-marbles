using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Ambience")]
    public AudioClip ambience = null;

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

    private void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName != "Mobile Interface")
            GetComponent<AudioSource>().PlayOneShot(ambience);
    }
}
