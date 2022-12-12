using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve wallJumpProgress;

    // AnimateAction variables
    GameObject character;
    public float animTimer = 10f;

    Vector2 destination;

    Vector2 startPosition;
    bool wallJump = false;
    bool halfwayWallJump = false;
    bool hasWallJumpedHalfway = false;

    // Update is called once per frame
    void Update()
    {
        animTimer += Time.deltaTime;
        Debug.Log(animTimer);
        Debug.Log(jumpProgress.length);

        if (animTimer <= jumpProgress.length && !wallJump)
        {
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, jumpProgress.Evaluate(animTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y, jumpProgress.Evaluate(animTimer)));
        }
        // Before jumping of wall
        else if (animTimer <= (wallJumpProgress.length / 2) && wallJump)
        {
            if (halfwayWallJump)
            {
                halfwayWallJump = false;
            }
            if (hasWallJumpedHalfway)
            {
                hasWallJumpedHalfway = false;
            }
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, destination.x, wallJumpProgress.Evaluate(animTimer)),
            Mathf.Lerp(character.transform.position.y, destination.y, wallJumpProgress.Evaluate(animTimer)));
        }
        // After jumping from wall
        else if (animTimer > (wallJumpProgress.length / 2) && animTimer <= wallJumpProgress.length && wallJump)
        {
            if (!halfwayWallJump)
                halfwayWallJump = true;
            character.transform.position = new Vector2(Mathf.Lerp(character.transform.position.x, startPosition.x, wallJumpProgress.Evaluate(animTimer)),
            Mathf.Lerp(character.transform.position.y, startPosition.y, wallJumpProgress.Evaluate(animTimer)));
        }
        else if (animTimer > wallJumpProgress.length && wallJump)
        {
            wallJump = false;
            halfwayWallJump = false;
            hasWallJumpedHalfway = false;
        }

        if (halfwayWallJump && !hasWallJumpedHalfway)
        {
            Debug.Log("reset rotation");
            Movement m = GetComponent<Movement>();

            for (int i = 0; i < 2; i++)
            {
                if (m.currentDirectionID <= -4 || m.currentDirectionID >= 4)
                {
                    m.currentDirectionID = 0;
                }
                m.currentDirectionID++;
            }
            
            m.UpdateAnimation();
            hasWallJumpedHalfway = true;
        }
    }

    public void AnimateAction(GameObject character, Vector3 destination, bool wallJump = false)
    {
        startPosition = character.transform.position;
        this.character = character;
        this.destination = destination;
        animTimer = 0;

        if (wallJump)
        {
            this.wallJump = wallJump;
            Debug.Log("destination: " + this.destination + ". startPosition: " + startPosition + ".");
        }
    }
}
