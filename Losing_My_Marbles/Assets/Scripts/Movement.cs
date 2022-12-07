using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public abstract class Movement : MonoBehaviour
{
    public Vector2 gridPosition = new(0, 0);
    [SerializeField] bool hasKey = false;
    public static List <Movement> enemies = new ();

    public int currentDirectionID = 0;

    GridManager grid;
 
    int multiplier;
    float timer = 1f;

    SkeletonAnimation frontSkeleton;
    SkeletonAnimation backSkeleton;
    bool useFrontSkeleton = false;
    public AnimationReferenceAsset frontIdle, frontJump, backIdle, backJump;
    public float jumpLength = 1;
    public float jumpSpeed = 1f;
    Spine.Animation nextIdleAnimation;
    Spine.Animation nextJumpAnimation;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        foreach (Transform child in transform)
        {
            if (child.name == "Front_Skeleton" && child.GetComponent<SkeletonAnimation>() != null)
            {
                frontSkeleton = child.GetComponent<SkeletonAnimation>();
            }
            else if (child.name == "Back_Skeleton" && child.GetComponent<SkeletonAnimation>() != null)
            {
                backSkeleton = child.GetComponent<SkeletonAnimation>();
            }
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        switch (currentDirectionID)
        {
            case 0:
                Turn(false, false);
                break;
            case 1 or -3:
                Turn(false, true);
                break;
            case 2 or -2:
                Turn(true, true);
                break;
            case 3 or -1:
                Turn(true, false);
                break;
            default:
                Turn(false, false);
                break;
        }
    }

    void Turn(bool facingLeft, bool front)
    {
        if (front)
        {
            nextIdleAnimation = frontIdle;
            nextJumpAnimation = frontJump;
            useFrontSkeleton = true;
        }
        else if (!front)
        {
            nextIdleAnimation = backIdle;
            nextJumpAnimation = backJump;
            useFrontSkeleton = false;
        }

        if (useFrontSkeleton)
        {
            frontSkeleton.GetComponent<SkeletonAnimation>().Skeleton.ScaleX = facingLeft ? -1f : 1f;
            frontSkeleton.gameObject.SetActive(true);
            backSkeleton.gameObject.SetActive(false);
        }
        else
        {
            backSkeleton.GetComponent<SkeletonAnimation>().Skeleton.ScaleX = facingLeft ? 1f : -1f;
            frontSkeleton.gameObject.SetActive(false);
            backSkeleton.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 1f;
            //enemies[0].DoAMove(1);
        }
    }
    public void TryMove(GameObject character, int dataID, int increment)
    {
        // Set transform position
        if (dataID == 0)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                if (grid == null)
                {
                    grid = FindObjectOfType<GridManager>();
                }
                switch (grid.IsSquareEmpty(character, RequestGridPosition(currentDirectionID)))
                {
                    case GridManager.EMPTY: // EMPTY (walls, void, etc)
                        TryMove(character, 1, 2); // lägg till recursion här'
                        break;

                    case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                        Move(character, 1);
                        break;

                    case GridManager.PLAYER: // PLAYER
                       
                        // Move(character, increment);
                        break;

                    case GridManager.ENEMY: // ENEMY
                        GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID)
                            + character.GetComponent<Movement>().gridPosition, enemies);
                        
                        TryMove(enemy, 0, 1);
                        TryMove(character, 0, 1);
                        break;

                    case GridManager.DOOR:
                        if (character.GetComponent<Movement>().hasKey == true)
                        {
                            Move(character, 1);
                            character.SetActive(false);
                            FindObjectOfType<ResetManager>().ResetLevel();
                        }
                        break;

                    case GridManager.KEY:
                        character.GetComponent<Movement>().hasKey = true;
                        GameObject.FindGameObjectWithTag("Key").SetActive(false);
                        FindObjectOfType<GridGenerator>().DestroyKeyGlitter();
                        Move(character, 1);
                        break;

                    case GridManager.HOLE:
                        character.SetActive(false);
                        grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0,0));
                        break;
                }
            }
        }

        // Set character rotation
        if (dataID == 1)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                multiplier = 1;
                if (increment < 0)
                {
                    multiplier *= -1;
                }
                currentDirectionID += multiplier;
                if (currentDirectionID <= -4 || currentDirectionID >= 4)
                {
                    currentDirectionID = 0;
                }
            }

            UpdateAnimation();
            if (useFrontSkeleton)
                frontSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
            else
                backSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
        }
    }

    public Vector2 RequestGridPosition(int currentDirectionID)
    {
        return currentDirectionID switch
        {
            0 => new Vector2(0, 1),
            1 or -3 => new Vector2(1, 0),
            2 or -2 => new Vector2(0, -1),
            3 or -1 => new Vector2(-1, 0),
            _ => new Vector2(0, 0),
        };
    }

    public void Move(GameObject character, int increment)
    {
        PlayerProperties pp;
        pp = FindObjectOfType<PlayerProperties>();

        multiplier = 1;
        if (increment < 0)
        {
            multiplier *= -1;
        }

        if (nextIdleAnimation == null || nextJumpAnimation == null)
        {
            UpdateAnimation();
        }

        switch (currentDirectionID)
        {
            case 0:
                pp.TransitionFromTo(character, new Vector3(character.transform.position.x + jumpLength, character.transform.position.y + jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 1 or -3:
                pp.TransitionFromTo(character, new Vector3(character.transform.position.x + jumpLength, character.transform.position.y - jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 2 or -2:
                pp.TransitionFromTo(character, new Vector3(character.transform.position.x - jumpLength, character.transform.position.y - jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 3 or -1:
                pp.TransitionFromTo(character, new Vector3(character.transform.position.x - jumpLength, character.transform.position.y + jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
        }

        if (useFrontSkeleton)
        {
            frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
            frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpSpeed;
            frontSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, false, nextJumpAnimation.Duration);
        }
        else
        {
            backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
            backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpSpeed;
            backSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, false, nextJumpAnimation.Duration);
        }
    }

    public abstract char ChangeTag();
    public abstract void DoAMove(int inc);
}
