using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve keyProgress;
    public AnimationCurve keyHeight;

    [HideInInspector] public float jumpProgressLength;
    [HideInInspector] public float keyProgressLength;

    // AnimateAction variables
    [HideInInspector] public float jumpAnimTimer = 10f;
    Vector2 startPosition;
    GameObject character;
    Vector2 destination;

    bool wallJump = false;
    int wallJumpProgressID = 0;

    // StealKey & DropKey variables
    [HideInInspector] public float keyAnimTimer = 10f;
    GameObject key;
    GameObject thief;
    GameObject victim;
    Vector2 keyDestination;

    int keyDataID = 0; // 0 = Empty, 1 = StealKey, 2 = DropKey

    // Other Scripts
    GridGenerator gridGen;


    void Start()
    {
        gridGen = FindObjectOfType<GridGenerator>();

        if (jumpProgress != null)
        {
            jumpProgressLength = jumpProgress[jumpProgress.length - 1].time;
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

        #region Jump
        if (jumpAnimTimer < jumpProgressLength && !wallJump)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(jumpAnimTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Jumping INTO wall
        else if (jumpAnimTimer < (jumpProgressLength / 2) && wallJump)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer * 2)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(jumpAnimTimer * 2)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Halfwaypoint Walljump
        if (jumpAnimTimer >= (jumpProgressLength / 2) && wallJumpProgressID == 0 && wallJump)
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

            m.UpdateSkeleton();
            wallJumpProgressID = 1;
        }
        // Jumping FROM wall
        if (wallJumpProgressID == 1 && wallJump)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, startPosition.x, jumpProgress.Evaluate(jumpAnimTimer)),
            Mathf.Lerp(character.transform.position.y, startPosition.y, jumpProgress.Evaluate(jumpAnimTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // End of Walljump
        if (jumpAnimTimer >= jumpProgressLength && wallJump)
        {
            wallJumpProgressID = 0;
            wallJump = false;
        }
        
        #endregion

        #region Key
        // Steal Key
        if (keyAnimTimer <= keyProgressLength && keyDataID == 1)
        {
            keyDestination = thief.transform.position;
            key.transform.position = new Vector2(Mathf.Lerp(key.transform.position.x, keyDestination.x, keyProgress.Evaluate(keyAnimTimer)),
            Mathf.Lerp(key.transform.position.y, keyDestination.y + keyHeight.Evaluate(keyAnimTimer / keyProgressLength), keyProgress.Evaluate(keyAnimTimer)));
            gridGen.UpdateGlitter(key.transform.position.x, key.transform.position.y);
        }
        // End of Key AnimationCurve
        else if (keyAnimTimer > keyProgressLength && keyDataID > 0)
        {
            keyDataID = 0;
            key.GetComponent<SpriteRenderer>().enabled = false;
            key.GetComponent<SpriteRenderer>().sortingOrder = 2;
            gridGen.UpdateGlitter(keyDestination.x, keyDestination.y);
            thief.GetComponent<PlayerProperties>().hasKey = true;
        }
        #endregion
    }

    public void AnimateAction(GameObject character, Vector3 destination, bool wallJump = false)
    {
        jumpAnimTimer = 0;
        startPosition = character.transform.position;
        this.character = character;
        this.destination = destination;

        if (wallJump)
        {
            this.wallJump = wallJump;
        }
    }

    public void StealKey(GameObject thief, GameObject victim)
    {
        keyDataID = 1;
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
        keyDataID = 2;
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
}
