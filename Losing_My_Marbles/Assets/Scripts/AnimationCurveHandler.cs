using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnimationCurveHandler : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve jumpHeight;
    public AnimationCurve marbleTravelProgress;
    public AnimationCurve keyHeight;

    [HideInInspector] public float jumpProgressLength;
    [HideInInspector] public float marbleTravelLength;

    float jumpHeightLength;
    float jumpCurveDiff;

    // ForwardJump variables
    [HideInInspector] public float jumpAnimTimer = 10f;
    Vector2 startPosition;
    GameObject character;
    [HideInInspector] public Vector2 destination;

    int normalJumpProgressID = 0;
    int wallJumpProgressID = 0;

    // PickupMarble variables
    [HideInInspector] public float marbleAnimTimer = 10f;
    GameObject marble;
    GameObject marbleGetter;
    Vector2 marbleDestination;

    int marbleTravelProgressID = 0; // 0 = Empty
    int typeID;

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
        if (marbleTravelProgress != null)
        {
            marbleTravelLength = marbleTravelProgress[marbleTravelProgress.length - 1].time;
        }
    }

    void Update()
    {
        jumpAnimTimer += Time.deltaTime;
        marbleAnimTimer += Time.deltaTime;

        #region Jump Animations
        // Normal Jump
        if (jumpAnimTimer < jumpProgressLength && normalJumpProgressID == 1)
        {
            if(typeID == 2)
            {
                character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate),
                Mathf.Lerp(character.transform.position.y, destination.y , jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate));
            }
            else
            {
                character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate),
                Mathf.Lerp(character.transform.position.y, destination.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate));
            }
        }
        // End of Normal Jump
        else if (jumpAnimTimer >= jumpProgressLength && normalJumpProgressID >= 1)
        {
            normalJumpProgressID = 0;
            character.transform.position = destination;
        }
        // Jumping INTO wall
        else if (jumpAnimTimer < (jumpProgressLength / 2) && wallJumpProgressID == 1)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate),
            Mathf.Lerp(character.transform.position.y, destination.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate));
        }
        // Halfwaypoint Walljump
        if (jumpAnimTimer >= (jumpProgressLength / 2) && wallJumpProgressID == 1)
        {
            Movement m = GetComponent<Movement>();
            m.UpdateSkeleton();
            wallJumpProgressID = 2;
        }
        // Jumping FROM wall
        if (jumpAnimTimer < jumpProgressLength && wallJumpProgressID == 2)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, startPosition.x, jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate),
            Mathf.Lerp(character.transform.position.y, startPosition.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff), jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate));
        }
        // End of Walljump
        else if (jumpAnimTimer >= jumpProgressLength && wallJumpProgressID == 2)
        {
            wallJumpProgressID = 0;
            character.transform.position = startPosition;
        }
        #endregion

        #region Key Animations

        if (marbleAnimTimer <= marbleTravelLength && marbleTravelProgressID > 0 && marble != null)
        {
            if (marbleGetter != null)
            {
                marbleDestination = marbleGetter.transform.position;
            }
            marble.transform.position = new Vector2(Mathf.Lerp(marble.transform.position.x, marbleDestination.x, marbleTravelProgress.Evaluate(marbleAnimTimer)),
            Mathf.Lerp(marble.transform.position.y, marbleDestination.y + keyHeight.Evaluate(marbleAnimTimer / marbleTravelLength), marbleTravelProgress.Evaluate(marbleAnimTimer)));
        }
        // End of <Insert Key Animation> AnimationCurve
        else if (marbleAnimTimer > marbleTravelLength && marbleTravelProgressID > 0 && marble != null)
        {
            marbleTravelProgressID = 0;
            marble.GetComponent<SpriteRenderer>().sortingOrder = 2;
            marble.transform.position = marbleDestination;
            if (marbleGetter != null)
            {
                marble.GetComponent<SpriteRenderer>().enabled = false;
                marbleGetter.GetComponent<Movement>().specialMarbleCount++;
                marbleGetter = null;
            }
            marble = null;
        }
        #endregion
    }

    #region Player Animation Functions
    public void ForwardJump(GameObject character, Vector3 destination, int typeID = 0)
        // typeID 0 = Normal Jump, typeID 1 = Wall Jump
    {
        jumpAnimTimer = 0;
        startPosition = character.transform.position;
        this.character = character;
        this.destination = destination;
        this.typeID = typeID; 
        if (typeID == 1)
        {
            wallJumpProgressID = 1;
        }
        else
        {
            normalJumpProgressID = 1;
        }
    }
    #endregion

    public void PickupMarble(GameObject marbleGetter, Vector3 positionToCheck)
    {
        if (marble == null)
            GetSpecificMarbleInScene(positionToCheck);
        marbleTravelProgressID = 1;
        marbleAnimTimer = 0;
        this.marbleGetter = marbleGetter;
        marble.GetComponent<SpriteRenderer>().enabled = true;
        marble.GetComponent<SpriteRenderer>().sortingOrder++;
        GetComponent<Movement>().quiterAudio.PlayOneShot(FindObjectOfType<AudioManager>().pickupMarble);
    }

    public void GetSpecificMarbleInScene(Vector2 gridPosition)
    {
        foreach (Transform transformInScene in FindObjectsOfType<Transform>())
        {
            if (transformInScene.gameObject.CompareTag("Special Marble") && (gameObject.GetComponent<Movement>().RequestGridPosition(gameObject.GetComponent<Movement>().currentDirectionID)
                + new Vector2(transform.position.x, transform.position.y) == gridPosition))
            {
                marble = transformInScene.gameObject;
                Debug.Log(marble);
                return;
            }
        }
    }
}
