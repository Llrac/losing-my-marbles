using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Desktop FX")]
    public AudioClip playerJump = null;
    public AudioClip pickupKey = null;
    public AudioClip dropKey = null;
    public AudioClip characterFall = null;
    public AudioClip pushHit = null;
    public AudioClip wallHit = null;
    public AudioClip playerReady = null;

    [Header("Special Marbles")]
    public AudioClip selectAmplifier = null;
    public AudioClip selectBlink = null;
    public AudioClip selectBlock = null;
    public AudioClip selectBomb = null;
    public AudioClip selectDaze = null;
    public AudioClip selectEarthquake = null;
    public AudioClip selectSwap = null;

    [Header("Mobile FX")]
    public AudioClip newMarbles = null;
    public AudioClip selectMarble = null;
    public AudioClip marblesReady = null;
    public AudioClip pressGo = null;
}
