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
    public static List <Movement> enemies = new ();

    public int currentDirectionID = 0;

    public AudioSource mediumAudio;
    public AudioSource quiterAudio;
    public AudioSource louderAudio;

    public GameObject deathPoof = null;
    public GameObject blinkEffect = null;
    public GameObject swapEffect = null;

    GridManager grid;
    GridGenerator gg;
 
    int jumpMultiplier;
    float wallJumpMultiplier;

    SkeletonAnimation frontSkeleton;
    SkeletonAnimation backSkeleton;
    bool usingFrontSkeleton = false;
    public AnimationReferenceAsset frontIdle, frontJump, backIdle, backJump; // TODO: implement idle2 and idle3
    public AnimationReferenceAsset frontAttack = null, backAttack = null; // rats only

    public float jumpLength = 1;
    public float jumpAnimationSpeed = 5f;
    Spine.Animation nextIdleAnimation;
    Spine.Animation nextJumpAnimation;
    Spine.Animation nextAttackAnimation; // rats only

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
        RatProperties rp = GetComponent<RatProperties>();
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
        else if (rp != null)
        {
            switch (rp.enemyID)
            {
                case 5:
                    frontSkeleton.initialSkinName = "Blå front";
                    frontSkeleton.Initialize(true);
                    backSkeleton.initialSkinName = "Blå back";
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

    public bool TryMove(GameObject character, int dataID, int increment, int  forcedJump = 0)
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
            switch (grid.GetNexTile(character, RequestGridPosition(currentDirectionID, increment)))
            {
                case GridManager.EMPTY: // EMPTY (walls, void, etc)
                    FindObjectOfType<GridGenerator>().OnHitWall(character);
                    Move(character, 1, 1);
                    TryMove(character, 1, 2);
                    if (mediumAudio != null)
                        mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
                    return false;

                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Move(character, 1, forcedJump);
                    savedTile = 'X';
                    return true;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);

                    //if (player.GetComponent<PlayerProperties>().specialMarbleCount == true)
                    //{
                    //    // player.GetComponent<Animation>().StealKey(character, player); // character steals from player
                    //}
                    
                    if (player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID) == true)
                    {
                        TryMove(gameObject, 0, 1, forcedJump);
                        if (mediumAudio != null)
                            mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().pushHit);

                        return true;
                    }
                    
                    return false;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    enemy.GetComponent<RatProperties>().DoAMove(0, 1, currentDirectionID);
                    
                    break;

                case GridManager.DOOR:
                   
                    //if (character.GetComponent<Movement>().specialMarbleCount == true)
                    //{
                    //    character.GetComponent<Movement>().specialMarbleCount = false;
                    //    character.SetActive(false);
                    //    gg.UpdateGlitter();
                    //    ResetManager.PlayerWin(gameObject.GetComponent<PlayerProperties>().playerID);
                    //    // players should not be able to send more actions
                    //  //  FindObjectOfType<ResetManager>().ResetLevel();
                    //}
                    //else
                    //{
                    //    Move(character, 1);
                    //    savedTile = 'D';
                    //    return true;
                    //}
                    break;

                case GridManager.MARBLE:
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<AnimationCurveHandler>().PickupMarble(character, increment);
                        Move(character, 1);
                    }
                    else if (CompareTag("Enemy"))
                    {

                    }
                    
                    return true;

                case GridManager.HOLE:
                    //if (gameObject.GetComponent<Movement>().specialMarbleCount == true)
                    //{
                    //    //character.GetComponent<Animation>().DropKey(character);
                    //    //savedTile = GridManager.KEY;
                    //}
                    
                    grid.MoveInGridMatrix(gameObject.GetComponent<Movement>(), new Vector2(0, 0));
                    
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<PlayerProperties>().Death(gg.GetRealWorldPosition(gridPosition + RequestGridPosition(currentDirectionID)) + new Vector2(0, 0.7f));
                    }
                    else if (CompareTag("Enemy"))
                    {
                        character.GetComponent<RatProperties>().Death();
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
                            if (mediumAudio != null)
                                mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
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

                    if (character.GetComponent<PlayerProperties>().specialMarbleCount >= TurnManager.marblesToWin)
                    {
                        ResetManager.PlayerWin(gameObject.GetComponent<PlayerProperties>().playerID);
                    }

                    //if (player.GetComponent<PlayerProperties>().specialMarbleCount == true)
                    //{
                    //    // player.GetComponent<Animation>().StealKey(character, player);
                    //}

                    if (player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID) == true)
                    {
                        Blink(increment); // else blink increment--
                        if (mediumAudio != null)
                            mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().pushHit);
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
                    //if (character.GetComponent<Movement>().specialMarbleCount == true)
                    //{
                    //    character.GetComponent<Movement>().specialMarbleCount = false;
                    //    character.SetActive(false);
                    //    gg.UpdateGlitter();
                    //    ResetManager.PlayerWin(gameObject.GetComponent<PlayerProperties>().playerID);
                    //    // players should not be able to send more actions
                    //    //  FindObjectOfType<ResetManager>().ResetLevel();
                    //}
                    //else
                    //{
                    //    Blink(increment);
                    //    savedTile = 'D';
                    //    return true;
                    //}
                    
                    break;

                case GridManager.MARBLE:
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<AnimationCurveHandler>().PickupMarble(character, increment);
                        Blink(increment);
                    }
                    
                    return true;

                case GridManager.HOLE:
                   
                    //if (gameObject.GetComponent<Movement>().specialMarbleCount == true)
                    //{
                    //    //character.GetComponent<Animation>().DropKey(character); // add a special fix to dropp key maybe the key should be dropped based on the holes gp and its transform position
                        
                    //}
                    grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0, 0));
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<PlayerProperties>().Death(gg.GetRealWorldPosition(gridPosition + RequestGridPosition(currentDirectionID, 3)) + new Vector2(0, 0.7f));
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
            SetAnimation(dataID, character);
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

        if (CompareTag("Player"))
        {
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
        else if (CompareTag("Enemy"))
        {
            if (usingFrontSkeleton)
            {
                if (frontAttack != null)
                {
                    nextAttackAnimation = frontAttack;
                }
                frontSkeleton.gameObject.SetActive(true);
                backSkeleton.gameObject.SetActive(false);
                frontSkeleton.Skeleton.ScaleX = facingLeft ? -1f : 1f;
                if (facingLeft)
                    frontSkeleton.transform.eulerAngles = new Vector3(0, 0, -13);
                else
                    frontSkeleton.transform.eulerAngles = new Vector3(0, 0, 13);
            }
            else
            {
                if (backAttack != null)
                {
                    nextAttackAnimation = backAttack;
                }
                frontSkeleton.gameObject.SetActive(false);
                backSkeleton.gameObject.SetActive(true);
                backSkeleton.Skeleton.ScaleX = facingLeft ? 1f : -1f;
                if (facingLeft)
                    backSkeleton.transform.eulerAngles = new Vector3(0, 0, 13);
                else
                    backSkeleton.transform.eulerAngles = new Vector3(0, 0, -13);
            }
        }
    }

    public void SetAnimation(int dataID, GameObject character = null, bool wallJump = false, bool ratAttack = false)
    {
        if (frontSkeleton == null || backSkeleton == null)
        {
            return;
        }
        if (wallJump)
            dataID = 0;
        if (ratAttack)
        {
            AnimationCurveHandler animation = character.GetComponent<AnimationCurveHandler>();

            if (usingFrontSkeleton)
            {
                frontSkeleton.AnimationState.SetAnimation(0, nextAttackAnimation, false).TimeScale = jumpAnimationSpeed;
                frontSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, true, animation.jumpProgressLength);
            }
            else
            {
                backSkeleton.AnimationState.SetAnimation(0, nextAttackAnimation, false).TimeScale = jumpAnimationSpeed;
                backSkeleton.AnimationState.AddAnimation(0, nextIdleAnimation, true, animation.jumpProgressLength);
            }
            return;
        }
        switch (dataID)
        {
            case 0: // Jump forward
                AnimationCurveHandler animation = character.GetComponent<AnimationCurveHandler>();
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
    // typeID 2 = Forced jump
    {
        if (mediumAudio != null)
            mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().playerJump);

        jumpMultiplier = 1;
        if (increment < 0)
        {
            jumpMultiplier *= -1;
        }

        wallJumpMultiplier = 1;
        if (typeID == 0 || typeID == 2)
        {
            grid.MoveInGridMatrix(character.GetComponent<Movement>(), RequestGridPosition(currentDirectionID));
        }
        else if (typeID == 1)
        {
            wallJumpMultiplier *= 0.5f;
        }

        AnimationCurveHandler animation = character.GetComponent<AnimationCurveHandler>();
       
        switch (currentDirectionID)
        {
            case 0:
                animation.ForwardJump(character, new Vector3(character.transform.position.x + jumpLength * wallJumpMultiplier,
                    character.transform.position.y + jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 1 or -3:
                animation.ForwardJump(character, new Vector3(character.transform.position.x + jumpLength * wallJumpMultiplier,
                    character.transform.position.y - jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 2 or -2:
                animation.ForwardJump(character, new Vector3(character.transform.position.x - jumpLength * wallJumpMultiplier,
                    character.transform.position.y - jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
            case 3 or -1:
                animation.ForwardJump(character, new Vector3(character.transform.position.x - jumpLength * wallJumpMultiplier,
                    character.transform.position.y + jumpLength * wallJumpMultiplier / 2, 0) * jumpMultiplier, typeID);
                break;
        }

        UpdateSkeleton();
        if(typeID == 0)
        {
            SetAnimation(typeID, character);
        }
        else if (typeID == 1)
        {
            SetAnimation(typeID, character, true);
        }
    }
    public void Blink(int blinkDistanceMultiplier) // you could think of this as part 1/2 of the whole blink process
    {
        if (quiterAudio != null)
            quiterAudio.PlayOneShot(FindObjectOfType<AudioManager>().triggerBlink);
        if (blinkEffect != null)
        {
            GameObject newBlinkEffect = Instantiate(blinkEffect, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            Destroy(newBlinkEffect, 1f);
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("shrink");
        }

        grid?.MoveInGridMatrix(this, RequestGridPosition(currentDirectionID, blinkDistanceMultiplier));
        float blinkDistance = jumpLength * blinkDistanceMultiplier;

        StartCoroutine(BlinkAnimation(blinkDistance));
    }

    IEnumerator BlinkAnimation(float blinkDistance)
    {
        yield return new WaitForSeconds(0.25f);
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

        if (blinkEffect != null)
        {
            GameObject newBlinkEffect = Instantiate(blinkEffect, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            newBlinkEffect.GetComponent<Animator>().SetTrigger("blink_to");
            Destroy(newBlinkEffect, 1f);
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("grow");
        }

        yield return null;
    }
    public abstract char ChangeTag();
    public abstract void DoAMove(int id, int inc, int dir);
}
