using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnimationCurveHandler : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve jumpHeight;
    public AnimationCurve marbleTravelProgress;
    public AnimationCurve marbleTravelHeight;
    public AnimationCurve fallHeight = null;

    [HideInInspector] public float jumpProgressLength;
    [HideInInspector] public float marbleTravelLength;
    [HideInInspector] public float fallLength;

    float jumpHeightLength;
    float jumpCurveDiff;

    // ForwardJump variables
    [HideInInspector] public float jumpAnimTimer = 10f;
    Vector2 startPosition;
    GameObject character;
    [HideInInspector] public Vector2 destination;

    int normalJumpProgressID = 0;
    int wallJumpProgressID = 0;
    int marbleTravelProgressID = 0;
    int fallProgressID = 0;

    // PickupMarble variables
    [HideInInspector] public float marbleAnimTimer = 10f;
    GameObject marble;
    GameObject marbleGetter;
    Vector2 marbleDestination;

    // Respawn variables
    [HideInInspector] public float fallAnimTimer = 10f;

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
        if (fallHeight != null)
        {
            fallLength = fallHeight[fallHeight.length - 1].time;
        }
    }

    void Update()
    {
        jumpAnimTimer += Time.deltaTime;
        marbleAnimTimer += Time.deltaTime;
        fallAnimTimer += Time.deltaTime;

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
        else if (jumpAnimTimer >= jumpProgressLength && normalJumpProgressID == 1)
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
            Mathf.Lerp(character.transform.position.y, startPosition.y + jumpHeight.Evaluate(jumpAnimTimer / jumpCurveDiff) * Time.deltaTime * Application.targetFrameRate, jumpProgress.Evaluate(jumpAnimTimer) * Time.deltaTime * Application.targetFrameRate));
        }
        // End of Walljump
        else if (jumpAnimTimer >= jumpProgressLength && wallJumpProgressID == 2)
        {
            wallJumpProgressID = 0;
            character.transform.position = startPosition;
        }
        #endregion

        #region Marble Animations
        // During
        if (marbleAnimTimer <= marbleTravelLength && marbleTravelProgressID > 0 && marble != null)
        {
            if (marbleGetter != null)
            {
                marbleDestination = marbleGetter.transform.position;
            }
            marble.transform.position = new Vector2(Mathf.Lerp(marble.transform.position.x, marbleDestination.x, marbleTravelProgress.Evaluate(marbleAnimTimer) * Time.deltaTime * Application.targetFrameRate),
            Mathf.Lerp(marble.transform.position.y, marbleDestination.y + marbleTravelHeight.Evaluate(marbleAnimTimer / marbleTravelLength) * Time.deltaTime * Application.targetFrameRate, marbleTravelProgress.Evaluate(marbleAnimTimer) * Time.deltaTime * Application.targetFrameRate));
        }
        // End
        else if (marbleAnimTimer > marbleTravelLength && marbleTravelProgressID > 0 && marble != null)
        {
            EnableMarbleVisuals(false);
            marbleTravelProgressID = 0;
            marble.transform.position = marbleDestination;
            marble = null;
            if (marbleGetter != null)
            {
                marbleGetter.GetComponent<PlayerProperties>().specialMarbleCount++;
                FindObjectOfType<UIDesktop>().UpdatePickupMarbles(marbleGetter);
                marbleGetter = null;
            }
        }
        #endregion

        // Fall from sky
        if (fallAnimTimer <= fallLength && fallProgressID == 1)
        {
            character.transform.position = new Vector2(startPosition.x, Mathf.Lerp(startPosition.y + fallHeight.Evaluate(fallAnimTimer) * Time.deltaTime * Application.targetFrameRate,
            destination.y + fallHeight.Evaluate(fallAnimTimer) * Time.deltaTime * Application.targetFrameRate, fallHeight.Evaluate(fallAnimTimer) * Time.deltaTime * Application.targetFrameRate));
        }
        else if (fallAnimTimer > fallLength && fallProgressID == 1)
        {
            fallProgressID = 0;
            character.transform.position = destination;
        }
    }

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

    public void Respawn(GameObject character, Vector2 destination)
    {
        fallAnimTimer = 0;
        startPosition = destination;
        this.destination = destination;
        this.character = character;
        fallProgressID = 1;
    }

    #region Marble Functions
    public GameObject GetSpecificMarbleInScene(GameObject character, int increment = 1)
    {
        foreach (Transform transformInScene in FindObjectsOfType<Transform>())
        {
            if (transformInScene.gameObject.CompareTag("Special Marble") && transformInScene.gameObject.GetComponent<MysteryMarble>().gridPosition ==
                character.GetComponent<Movement>().gridPosition + character.GetComponent<Movement>().RequestGridPosition(character.GetComponent<Movement>().currentDirectionID, increment))
            {
                marble = transformInScene.gameObject;
                return marble;
            }
        }
        return null;
    }

    void EnableMarbleVisuals(bool enable)
    {
        foreach (Transform child in marble.transform)
        {
            if (child.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                if (enable)
                {
                    child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    child.gameObject.GetComponent<SpriteRenderer>().sortingOrder++;
                }
                else
                {
                    Destroy(marble);
                   
                    child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    child.gameObject.GetComponent<SpriteRenderer>().sortingOrder--;
                }
            }
            else if (child.gameObject.GetComponent<ParticleSystem>() != null && child.gameObject.name == "Marble Glitter")
            {
                if (enable)
                {
                    child.gameObject.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    child.gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
        StartCoroutine(NewLevel());
    }
    private IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(1f);
        if (FindObjectOfType<MysteryMarble>() == null) //should not happen if there is someone who won
        {
            Debug.Log("Hej");
            FindObjectOfType<ResetManager>().NextLevel();
        }
    }

    public void PickupMarble(GameObject marbleGetter, int increment = 1)
    {
        if (marble == null)
            marble = GetSpecificMarbleInScene(marbleGetter, increment);

        marbleTravelProgressID = 1;
        marbleAnimTimer = 0;
        this.marbleGetter = marbleGetter;
        EnableMarbleVisuals(true);
        GetComponent<Movement>().quiterAudio.PlayOneShot(FindObjectOfType<AudioManager>().pickupMarble);
    }
    #endregion
}
