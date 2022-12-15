using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve jumpHeight;
    public AnimationCurve keyProgress;
    public AnimationCurve keyHeight;

    [HideInInspector] public float jumpProgressLength;
    [HideInInspector] public float keyProgressLength;
    [HideInInspector] public float keyHeightLength;

    float jumpHeightLength;
    float jumpCurveDiff;

    // AnimateAction variables
    [HideInInspector] public float jumpAnimTimer = 10f;
    Vector2 startPosition;
    GameObject character;
    Vector2 destination;

    int jumpProgressID = 0;
    int wallJumpProgressID = 0;

    // StealKey & DropKey variables
    [HideInInspector] public float keyAnimTimer = 10f;
    GameObject key;
    GameObject keyGetter;
    GameObject thief;
    GameObject victim;
    Vector2 keyDestination;

    int keyProgressID = 0; // 0 = Empty, 1 = StealKey, 2 = DropKey

    // Other Scripts
    GridGenerator gridGen;


    void Start()
    {
        Application.targetFrameRate = 60;

        gridGen = FindObjectOfType<GridGenerator>();

        if (jumpProgress != null)
        {
            jumpProgressLength = jumpProgress[jumpProgress.length - 1].time;
            if (jumpHeight != null)
            {
                jumpHeightLength = jumpHeight[jumpHeight.length - 1].time;
                jumpCurveDiff = jumpProgressLength / jumpHeightLength;
            }
        }
        if (keyProgress != null)
        {
            keyProgressLength = keyProgress[keyProgress.length - 1].time;
        }
        if (GameObject.FindGameObjectWithTag("Key") != null)
        {
            key = GameObject.FindGameObjectWithTag("Key");
        }
    }

    void Update()
    {
        jumpAnimTimer += Time.deltaTime;
        keyAnimTimer += Time.deltaTime;

        #region Jump Animations
        // Normal Jump
        if (jumpAnimTimer < jumpProgressLength && jumpProgressID == 1)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // End of Normal Jump
        else if (jumpAnimTimer >= jumpProgressLength && jumpProgressID >= 1)
        {
            jumpProgressID = 0;
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(jumpAnimTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Jumping INTO wall
        else if (jumpAnimTimer < (jumpProgressLength / 2) && wallJumpProgressID == 1)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer * 2)),
            Mathf.Lerp(character.transform.position.y, destination.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer * 2)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Halfwaypoint Walljump
        if (jumpAnimTimer >= (jumpProgressLength / 2) && wallJumpProgressID == 1)
        {
            Movement m = GetComponent<Movement>();

            // for (int i = 0; i < 2; i++)
            // {
            //     if (m.currentDirectionID <= -4 || m.currentDirectionID >= 4)
            //     {
            //         m.currentDirectionID = 0;
            //     }
            //     m.currentDirectionID++;
            // }

            Debug.Log(jumpProgress.Evaluate(jumpAnimTimer / jumpCurveDiff));

            m.UpdateSkeleton();
            wallJumpProgressID = 2;
        }
        // Jumping FROM wall
        if (jumpAnimTimer < jumpProgressLength && wallJumpProgressID == 2)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, startPosition.x, jumpProgress.Evaluate(jumpAnimTimer)),
            Mathf.Lerp(character.transform.position.y, startPosition.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // End of Walljump
        if (jumpAnimTimer >= jumpProgressLength && wallJumpProgressID == 2)
        {
            wallJumpProgressID = 0;
            character.transform.position = startPosition;
        }
        
        #endregion

        #region Key Animations
        // <Insert Key Animation> AnimationCurve
        if (keyAnimTimer <= keyProgressLength && keyProgressID > 0)
        {
            if (keyGetter != null)
                keyDestination = keyGetter.transform.position;
            else if (thief != null)
                keyDestination = thief.transform.position;
            
            key.transform.position = new Vector2(Mathf.Lerp(key.transform.position.x, keyDestination.x, keyProgress.Evaluate(keyAnimTimer)),
            Mathf.Lerp(key.transform.position.y, keyDestination.y + keyHeight.Evaluate(keyAnimTimer / keyProgressLength), keyProgress.Evaluate(keyAnimTimer)));
            gridGen.UpdateGlitter(key.transform.position.x, key.transform.position.y);
        }
        // End of <Insert Key Animation> AnimationCurve
        else if (keyAnimTimer > keyProgressLength && keyProgressID > 0)
        {
            keyProgressID = 0;
            key.GetComponent<SpriteRenderer>().sortingOrder = 2;
            gridGen.UpdateGlitter(keyDestination.x, keyDestination.y);
            key.transform.position = keyDestination;
            if (keyGetter != null)
            {
                key.GetComponent<SpriteRenderer>().enabled = false;
                keyGetter.GetComponent<Movement>().hasKey = true;
                keyGetter = null;
            }
            else if (thief != null)
            {
                key.GetComponent<SpriteRenderer>().enabled = false;
                thief.GetComponent<PlayerProperties>().hasKey = true;
                thief = null;
            }
        }
        #endregion
    }

    #region Player Animation Functions
    public void AnimateAction(GameObject character, Vector3 destination, bool wallJump = false)
    {
        jumpAnimTimer = 0;
        startPosition = character.transform.position;
        this.character = character;
        this.destination = destination;

        if (wallJump)
        {
            wallJumpProgressID = 1;
        }
        else
        {
            jumpProgressID = 1;
        }
    }
    #endregion

    #region Key Animation Functions
    public void PickupKey(GameObject keyGetter)
    {
        keyProgressID = 1;
        keyAnimTimer = 0;
        this.keyGetter = keyGetter;
        key.GetComponent<SpriteRenderer>().enabled = true;
        key.GetComponent<SpriteRenderer>().sortingOrder++;
    }

    public void StealKey(GameObject thief, GameObject victim)
    {
        keyProgressID = 2;
        keyAnimTimer = 0;
        this.thief = thief;
        this.victim = victim;
        this.victim.GetComponent<PlayerProperties>().hasKey = false;
        key.transform.position = victim.transform.position;
        key.GetComponent<SpriteRenderer>().enabled = true;
        key.GetComponent<SpriteRenderer>().sortingOrder++;
    }

    public void DropKey(GameObject keyDropper)
    {
        keyProgressID = 3;
        keyAnimTimer = 0;
        Movement m = keyDropper.GetComponent<Movement>();
        m.savedTile = 'K';
        m.hasKey = false;
        key.transform.position = keyDropper.transform.position;
        keyDestination = new Vector2(
            m.gridPosition.x * 1 + m.gridPosition.y * 1 + -7 - 1,
            ((-m.gridPosition.x * 1 + m.gridPosition.y * 1) / 2) + 1.5f);
        key.GetComponent<SpriteRenderer>().enabled = true;
        key.GetComponent<SpriteRenderer>().sortingOrder++;
    }
    #endregion
}
