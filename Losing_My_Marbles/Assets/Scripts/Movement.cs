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

    public float jumpLength = 1;
 
    int multiplier;

    float timer = 1f;

    SkeletonAnimation sa;
    public AnimationReferenceAsset frontIdle, frontJump, backIdle, backJump;
    public float jumpSpeed = 1f;
    Spine.Animation nextIdleAnimation;
    Spine.Animation nextJumpAnimation;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        sa = GetComponentInChildren<SkeletonAnimation>();

        UpdateNextAnimation();
    }

    private void UpdateNextAnimation()
    {
        switch (currentDirectionID)
        {
            case 0:
                nextIdleAnimation = backIdle;
                nextJumpAnimation = backJump;
                Turn(false);
                break;
            case 1 or -3:
                nextIdleAnimation = frontIdle;
                nextJumpAnimation = frontJump;
                Turn(false);
                break;
            case 2 or -2:
                nextIdleAnimation = frontIdle;
                nextJumpAnimation = frontJump;
                Turn(true);
                break;
            case 3 or -1:
                nextIdleAnimation = backIdle;
                nextJumpAnimation = backJump;
                Turn(true);
                break;
            default:
                nextIdleAnimation = backIdle;
                nextJumpAnimation = backJump;
                Turn(false);
                break;
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

            UpdateNextAnimation();
            sa.AnimationState.SetAnimation(0, nextIdleAnimation, true);
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
        pp.characterToAnimate = character;

        multiplier = 1;
        if (increment < 0)
        {
            multiplier *= -1;
        }

        if (nextIdleAnimation == null || nextJumpAnimation == null)
        {
            UpdateNextAnimation();
        }

        switch (currentDirectionID)
        {
            case 0:
                pp.JumpTo(new Vector3(character.transform.position.x + jumpLength, character.transform.position.y + jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 1 or -3:
                pp.JumpTo(new Vector3(character.transform.position.x + jumpLength, character.transform.position.y - jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 2 or -2:
                pp.JumpTo(new Vector3(character.transform.position.x - jumpLength, character.transform.position.y - jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
            case 3 or -1:
                pp.JumpTo(new Vector3(character.transform.position.x - jumpLength, character.transform.position.y + jumpLength / 2, 0) * multiplier);
                grid.MoveInGridMatrix(character.GetComponent<Movement>(),
                    RequestGridPosition(currentDirectionID));
                break;
        }

        sa.AnimationState.SetAnimation(0, nextJumpAnimation, false);
        sa.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpSpeed;
        sa.AnimationState.AddAnimation(0, nextIdleAnimation, false, nextJumpAnimation.Duration);
    }

    void Turn(bool facingLeft)
    {
        sa.Skeleton.ScaleX = facingLeft ? -1f : 1f;
    }

    public abstract char ChangeTag();
    public abstract void DoAMove(int inc);
}
