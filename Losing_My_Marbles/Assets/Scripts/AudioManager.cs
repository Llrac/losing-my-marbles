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

    [Header("Mobile FX")]
    public AudioClip newMarbles = null;
    public AudioClip selectMarble = null;
    public AudioClip marblesReady = null;
    public AudioClip pressGo = null;
}
