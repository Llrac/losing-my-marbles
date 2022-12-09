using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public abstract class Movement : MonoBehaviour
{
    public char savedTile = 'X';

    public Vector2 gridPosition = new(0, 0);
    public bool hasKey = false;
    public static List <Movement> enemies = new ();

    public int currentDirectionID = 0;

    GridManager grid;
 
    int multiplier;

    SkeletonAnimation frontSkeleton;
    SkeletonAnimation backSkeleton;
    bool usingFrontSkeleton = false;
    public AnimationReferenceAsset frontIdle, frontJump, backIdle, backJump; // TODO: implement idle2 and idle3
    public float jumpLength = 1;
    public float jumpAnimationSpeed = 5f;
    Spine.Animation nextIdleAnimation;
    Spine.Animation nextJumpAnimation;

    public void UpdateSkinBasedOnPlayerID()
    {
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
        PlayerProperties pp = GetComponent<PlayerProperties>();
        if (pp != null)
        {
            switch (pp.playerId)
            {
                case 1:
                    frontSkeleton.initialSkinName = "red";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "red";
                    backSkeleton.Initialize(true);

                    break;
                case 2:
                    frontSkeleton.initialSkinName = "purple";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "purple";
                    backSkeleton.Initialize(true);
                    break;
                case 3:
                    frontSkeleton.initialSkinName = "turquoise";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "turquoise";
                    backSkeleton.Initialize(true);
                    break;
                case 4:
                    frontSkeleton.initialSkinName = "yellow";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "yellow";
                    backSkeleton.Initialize(true);
                    break;
                default:
                    frontSkeleton.initialSkinName = "default";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "default";
                    backSkeleton.Initialize(true);
                    break;
            }
        }
    }

    public void UpdateAnimation()
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
        if(CompareTag("Player"))
        {
            if (frontSkeleton == null || backSkeleton == null)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == "Front_Skeleton" && child.GetComponent<SkeletonAnimation>())
                    {
                        frontSkeleton = child.GetComponent<SkeletonAnimation>();
                    }
                    else if (child.name == "Back_Skeleton" && child.GetComponent<SkeletonAnimation>())
                    {
                        backSkeleton = child.GetComponent<SkeletonAnimation>();
                    }
                }
            }

            if (front)
            {
                nextIdleAnimation = frontIdle;
                nextJumpAnimation = frontJump;
                usingFrontSkeleton = true;
            }
            else
            {
                nextIdleAnimation = backIdle;
                nextJumpAnimation = backJump;
                usingFrontSkeleton = false;
            }

            if (usingFrontSkeleton)
            {
                frontSkeleton.Skeleton.ScaleX = facingLeft ? -1f : 1f;
                frontSkeleton.gameObject.SetActive(true);
                backSkeleton.gameObject.SetActive(false);
            }
            else
            {
                backSkeleton.Skeleton.ScaleX = facingLeft ? 1f : -1f;
                frontSkeleton.gameObject.SetActive(false);
                backSkeleton.gameObject.SetActive(true);
            }
        }
       
    }

    public bool TryMove(GameObject character, int dataID, int increment) // into bool?
    {
        // Set transform position
        if (dataID == 0)
        {
            if (grid == null)
            {
                grid = FindObjectOfType<GridManager>();
            }
            switch (grid.GetNexTile(character, RequestGridPosition(currentDirectionID)))
            {
                case GridManager.EMPTY: // EMPTY (walls, void, etc)
                    TryMove(character, 1, 2);
                    return false;// l�gg till recursion h�r'


                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Move(character, 1);
                    savedTile = 'X';
                    break;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);
                    player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID);
                    // Move(character, increment);
                    break;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID)
                        + character.GetComponent<Movement>().gridPosition, enemies);
                    enemy.GetComponent<RatProperties>().DoAMove(1, currentDirectionID);
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
                    Move(character, 1);
                    break;

                case GridManager.HOLE:
                    character.SetActive(false);
                    grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0, 0));
                    if (CompareTag("Player"))
                    {
                        TurnManager.players.Remove(character.GetComponent<PlayerProperties>());
                    }
                    else
                    {
                        enemies.Clear();
                    }
                    break;

                case GridManager.WATER:
                    // do water stuff
                    Move(character, 1);
                    savedTile = GridManager.WATER;
                    break;
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
            
            if (usingFrontSkeleton)
            {
                frontSkeleton?.AnimationState.SetAnimation(0, nextIdleAnimation, true);
            }
            else
            {
                backSkeleton?.AnimationState.SetAnimation(0, nextIdleAnimation, true);
            }
            
        }
        
        return true;
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
        PlayerProperties pp = character.GetComponent<PlayerProperties>();

        multiplier = 1;
        if (increment < 0)
        {
            multiplier *= -1;
        }

        UpdateAnimation();

        switch (currentDirectionID)
        {
            case 0:
                character.transform.position = new Vector3(character.transform.position.x + jumpLength,
                    character.transform.position.y + jumpLength / 2, 0) * multiplier;
                break;
            case 1 or -3:
                character.transform.position = new Vector3(character.transform.position.x + jumpLength,
                    character.transform.position.y - jumpLength / 2, 0) * multiplier;
                break;
            case 2 or -2:
                character.transform.position = new Vector3(character.transform.position.x - jumpLength,
                    character.transform.position.y - jumpLength / 2, 0) * multiplier;
                break;
            case 3 or -1:
                character.transform.position = new Vector3(character.transform.position.x - jumpLength,
                    character.transform.position.y + jumpLength / 2, 0) * multiplier;
                break;
        }
        grid.MoveInGridMatrix(character.GetComponent<Movement>(),
            RequestGridPosition(currentDirectionID));

        if(frontSkeleton != null || backSkeleton != null)
        {
            if (usingFrontSkeleton)
            {
                frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpAnimationSpeed;
                frontSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, false, pp.jumpProgress.length);
            }
            else
            {
                backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpAnimationSpeed;
                backSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, false, pp.jumpProgress.length);
            }
        }
        
    }

    public abstract char ChangeTag();
    public abstract void DoAMove(int inc, int dir);
}
