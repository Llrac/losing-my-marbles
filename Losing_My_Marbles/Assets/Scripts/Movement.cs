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
    GridGenerator gg;
 
    int jumpMultiplier;
    float wallJumpMultiplier;

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
            switch (pp.playerID)
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

    public bool TryMove(GameObject character, int dataID, int increment, bool wallJump = false)
    {
        // Set transform position
        if (dataID == 0)
        {
            if (grid == null)
            {
                grid = FindObjectOfType<GridManager>();
            }
            if (gg == null)
            {
                gg = FindObjectOfType<GridGenerator>();
            }
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().playerJump);
            switch (grid.GetNexTile(character, RequestGridPosition(currentDirectionID, increment)))
            {
                case GridManager.EMPTY: // EMPTY (walls, void, etc)
                    FindObjectOfType<GridGenerator>().OnHitWall(character);
                    Move(character, 1, 1);
                    TryMove(character, 1, 2, true);
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
                    return false;

                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Move(character, 1);
                    savedTile = 'X';
                    return true;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);

                    if (player.GetComponent<PlayerProperties>().hasKey == true)
                    {
                        player.GetComponent<Animation>().StealKey(character, player); // character steals from player
                    }
                    
                    if (player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID) == true)
                    {
                        Move(gameObject, 1);
                        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().pushHit);

                        return true;
                    }
                    
                    return false;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    enemy.GetComponent<RatProperties>().DoAMove(0, 1, currentDirectionID);
                    
                    break;

                case GridManager.DOOR:
                   
                    if (character.GetComponent<Movement>().hasKey == true)
                    {
                        character.GetComponent<Movement>().hasKey = false;
                        character.SetActive(false);
                        gg.UpdateGlitter();
                        ResetManager.PlayerWin(gameObject.GetComponent<PlayerProperties>().playerID);
                        // players should not be able to send more actions
                      //  FindObjectOfType<ResetManager>().ResetLevel();
                    }
                    else
                    {
                        Move(character, 1);
                        savedTile = 'D';
                        return true;
                    }
                    break;

                case GridManager.KEY:
                    character.GetComponent<Animation>().PickupKey(character);
                    Move(character, 1);
                    return true;

                case GridManager.HOLE:
                    if (gameObject.GetComponent<Movement>().hasKey == true)
                    {
                        character.GetComponent<Animation>().DropKey(character);
                        savedTile = GridManager.KEY;
                    }
                    
                    grid.MoveInGridMatrix(gameObject.GetComponent<Movement>(), new Vector2(0, 0));
                    
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<PlayerProperties>().Death();
                    }
                    else
                    {
                        enemies.Clear(); //is a rat jumps down a hole every rat dies
                    }
                    return true;
      

                case GridManager.WATER:
                    // do water stuff
                    Move(character, increment);
                    savedTile = GridManager.WATER;
                    break;
            }

        }
        if (dataID == 2)
        {
            if (grid == null)
            {
                grid = FindObjectOfType<GridManager>();
            }
            if (gg == null)
            {
                gg = FindObjectOfType<GridGenerator>();
            }
            switch (grid.GetNexTile(character, RequestGridPosition(currentDirectionID, increment)))
            {
                case GridManager.EMPTY: // EMPTY (walls, void, etc)
                    for(int i = increment-1; i > 0; i--)
                    {
                        if(grid.GetNexTile(character, RequestGridPosition(currentDirectionID, i)) != GridManager.EMPTY)
                        {
                            TryMove(character, 2, i);
                            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
                            return true;
                        }
                    }
                    
                    return false;

                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Blink(increment);
                    savedTile = 'X';
                    return true;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);

                    if (player.GetComponent<PlayerProperties>().hasKey == true)
                    {
                        player.GetComponent<Animation>().StealKey(character, player);
                    }

                    if (player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID) == true)
                    {
                        Blink(increment); // else blink increment--
                        GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().pushHit);
                        return true;
                    }
                    else
                    {
                        if(RequestGridPosition(currentDirectionID, increment) == gridPosition)
                        {
                            Blink(increment);
                            return true;
                        }
                        TryMove(character, 2, increment - 1); // it sees itself if standing next to the player
                        // if there is a
                    }
                    return false;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    enemy.GetComponent<RatProperties>().DoAMove(0, 1, currentDirectionID);

                    break;

                case GridManager.DOOR:
                    Blink(increment);
                    savedTile = GridManager.DOOR;
                    if (character.GetComponent<Movement>().hasKey == true)
                    {
                        character.GetComponent<Movement>().hasKey = false;
                        character.SetActive(false);
                        gg.UpdateGlitter();
                        ResetManager.PlayerWin(gameObject.GetComponent<PlayerProperties>().playerID);
                        // players should not be able to send more actions
                        //  FindObjectOfType<ResetManager>().ResetLevel();
                    }
                    
                    break;

                case GridManager.KEY:
                    character.GetComponent<Animation>().PickupKey(character);
                    Blink(increment);
                    gg.UpdateGlitter();
                    
                    return true;

                case GridManager.HOLE:
                   
                    if (gameObject.GetComponent<Movement>().hasKey == true)
                    {
                        character.GetComponent<Animation>().DropKey(character); // add a special fix to dropp key maybe the key should be dropped based on the holes gp and its transform position
                        
                    }
                    grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0, 0));
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<PlayerProperties>().Death();
                    }
                    else
                    {
                        enemies.Remove(this); //is a rat jumps down a hole every rat dies
                    }
                    return true;

                case GridManager.WATER:
                    // do water stuff
                    Blink(increment);
                    savedTile = GridManager.WATER;
                    break;
            }

        }
        // Set character rotation
        if (dataID == 1)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                jumpMultiplier = 1;
                if (increment < 0)
                {
                    jumpMultiplier *= -1;
                }
                currentDirectionID += jumpMultiplier;
                if (currentDirectionID <= -4 || currentDirectionID >= 4)
                {
                    currentDirectionID = 0;
                }
            }

            UpdateSkeleton();
            SetAnimation(dataID, character, wallJump);
        }
        return false;
    }

    public void UpdateSkeleton(int addToDirectionID = 0)
    {
        switch (currentDirectionID + addToDirectionID)
        {
            case 0:
                SetSkeleton(false, false);
                break;
            case 1 or -3:
                SetSkeleton(false, true);
                break;
            case 2 or -2:
                SetSkeleton(true, true);
                break;
            case 3 or -1:
                SetSkeleton(true, false);
                break;
            default:
                SetSkeleton(false, false);
                break;
        }
    }

    void SetSkeleton(bool facingLeft, bool front)
    {
        if (CompareTag("Player"))
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

    public void SetAnimation(int typeID, GameObject character = null, bool wallJump = false)
    {
        if (frontSkeleton == null || backSkeleton == null)
        {
            return;
        }
        if (wallJump)
            typeID = 0;
        switch (typeID)
        {
            case 0: // Jump forward
                Animation animation = character.GetComponent<Animation>();
                if (usingFrontSkeleton)
                {
                    frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                    if (wallJump)
                    {
                        frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationStart = animation.jumpAnimTimer;
                    }
                    frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpAnimationSpeed;
                    frontSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, true, animation.jumpProgressLength);
                }
                else
                {
                    backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                    if (wallJump)
                    {
                        backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationStart = animation.jumpAnimTimer;
                    }
                    backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).TimeScale = jumpAnimationSpeed;
                    backSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, true, animation.jumpProgressLength);
                }
                break;

            case 1: // Rotate sideways
                if (usingFrontSkeleton)
                {
                    frontSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
                }
                else
                {
                    backSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
                }
                break;

            default:
                break;
        }
    }

    public Vector2 RequestGridPosition(int currentDirectionID, float distance = 1) // change this to give take a float as well to change the x and y
    {
        return currentDirectionID switch
        {
            0 => new Vector2(0, distance),
            1 or -3 => new Vector2(distance, 0),
            2 or -2 => new Vector2(0, -distance),
            3 or -1 => new Vector2(-distance, 0),
            _ => new Vector2(0, 0),
        };
    }

    public void Move(GameObject character, int increment, int typeID = 0)
    // typeID 0 = Normal Jump
    // typeID 1 = Wall Jump
    // typeID 2 = Ghost Jump
    {
        jumpMultiplier = 1;
        if (increment < 0)
        {
            jumpMultiplier *= -1;
        }

        wallJumpMultiplier = 1;
        if (typeID == 0)
        {
            grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
        }
        else if (typeID == 1)
        {
            wallJumpMultiplier *= 0.5f;
        }

        Animation animation = character.GetComponent<Animation>();
        switch (currentDirectionID)
        {
            case 0:
                animation.AnimateAction(character, new Vector3(character.transform.position.x + jumpLength * wallJumpMultiplier,
                    character.transform.position.y + jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 1 or -3:
                animation.AnimateAction(character, new Vector3(character.transform.position.x + jumpLength * wallJumpMultiplier,
                    character.transform.position.y - jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 2 or -2:
                animation.AnimateAction(character, new Vector3(character.transform.position.x - jumpLength * wallJumpMultiplier,
                    character.transform.position.y - jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 3 or -1:
                animation.AnimateAction(character, new Vector3(character.transform.position.x - jumpLength * wallJumpMultiplier,
                    character.transform.position.y + jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
        }

        UpdateSkeleton();
        SetAnimation(typeID, character);
    }
    public void Blink(int blinkDistanceMultiplier)
    {
       
        grid?.MoveInGridMatrix(this, RequestGridPosition(currentDirectionID, blinkDistanceMultiplier));
        float blinkDistance = jumpLength * blinkDistanceMultiplier;

        Animation animation = gameObject.GetComponent<Animation>();
        switch (currentDirectionID)
        {
            case 0:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + (blinkDistance),
                    gameObject.transform.position.y + (blinkDistance) / 2, 0);
                break;
            case 1 or -3:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + (blinkDistance),
                    gameObject.transform.position.y - (blinkDistance) / 2, 0);
                break;
            case 2 or -2:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - (blinkDistance),
                    gameObject.transform.position.y - (blinkDistance) / 2, 0);
                break;
            case 3 or -1:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - (blinkDistance),
                    gameObject.transform.position.y + (blinkDistance) / 2, 0);
                break;
        }
        gg.UpdateGlitter();
    }
    //public void DropKey()
    //{
    //    savedTile = 'K';
    //    hasKey = false;
    //    Vector2 keyPos;
    //    keyPos.x = ((gridPosition.x * 1 + gridPosition.y * 1)) + -7 - 1;
    //    keyPos.y = ((-gridPosition.x * 1 + gridPosition.y * 1) / 2) + 1.5f;
    //    GameObject.FindGameObjectWithTag("Key").GetComponent<SpriteRenderer>().enabled = true;
    //    GameObject.FindGameObjectWithTag("Key").transform.position = keyPos;
    //    gg.UpdateGlitter(keyPos.x, keyPos.y);
    //}
    public abstract char ChangeTag();
    public abstract void DoAMove(int id, int inc, int dir);
}
