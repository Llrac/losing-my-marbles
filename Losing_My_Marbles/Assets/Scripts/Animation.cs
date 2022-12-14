using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    [HideInInspector] public float jumpProgressLength; // temp variable, unless we don't get time of last key

    // AnimateAction variables
    Vector2 startPosition;
    GameObject character;
    Vector2 destination;
    [HideInInspector] public float animationTimer = 10f;

    bool wallJump = false;
    bool halfwayWallJump = false;
    bool hasWallJumpedHalfway = false;

    GridGenerator gridGen;


    void Start()
    {
        gridGen = FindObjectOfType<GridGenerator>();

        if (jumpProgress != null)
        {
            jumpProgressLength = jumpProgress[jumpProgress.length - 1].time;
        }
    }

    void Update()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer <= jumpProgressLength && !wallJump)
        {
            if (halfwayWallJump)
                halfwayWallJump = false;
            if (hasWallJumpedHalfway)
                hasWallJumpedHalfway = false;
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(animationTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(animationTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Jumping INTO wall
        else if (animationTimer <= (jumpProgressLength / 2) && wallJump)
        {
            if (halfwayWallJump)
                halfwayWallJump = false;
            if (hasWallJumpedHalfway)
                hasWallJumpedHalfway = false;
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(animationTimer * 2)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(animationTimer * 2)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        // Jumping FROM wall
        else if (animationTimer > (jumpProgressLength / 2) && animationTimer <= jumpProgressLength && wallJump)
        {
            if (!halfwayWallJump)
                halfwayWallJump = true;
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, startPosition.x, jumpProgress.Evaluate(animationTimer)),
            Mathf.Lerp(character.transform.position.y, startPosition.y, jumpProgress.Evaluate(animationTimer)));
            if (character.GetComponent<Movement>().hasKey)
                gridGen.UpdateGlitter();
        }
        else if (animationTimer > jumpProgressLength && wallJump)
        {
            wallJump = false;
            halfwayWallJump = false;
            hasWallJumpedHalfway = false;
        }

        if (halfwayWallJump && !hasWallJumpedHalfway)
        {
            Movement m = GetComponent<Movement>();

            for (int i = 0; i < 2; i++)
            {
                if (m.currentDirectionID <= -4 || m.currentDirectionID >= 4)
                {
                    m.currentDirectionID = 0;
                }
                m.currentDirectionID++;
            }
            
            m.UpdateSkeleton();
            //m.SetAnimation(0, character, true);
            hasWallJumpedHalfway = true;
        }
    }

    public void AnimateAction(GameObject character, Vector3 destination, bool wallJump = false)
    {
        startPosition = character.transform.position;
        this.character = character;
        this.destination = destination;
        animationTimer = 0;

        if (wallJump)
        {
            this.wallJump = wallJump;
        }
    }

    public void TakeFromGiveTo(GameObject takeFromCharacter, GameObject giveToCharacter)
    {
        takeFromCharacter.GetComponent<PlayerProperties>().hasKey = false;
        giveToCharacter.GetComponent<PlayerProperties>().hasKey = true;
    }
}
