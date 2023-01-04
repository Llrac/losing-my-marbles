using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public abstract class Movement : MonoBehaviour
{
    // Grid Stuff
    public char savedTile = 'X';
    public int currentDirectionID = 0;
    public Vector2 gridPosition = new(0, 0);
    public static List <Movement> enemies = new ();
    GridManager grid;
    GridGenerator gg;

    public AudioSource mediumAudio;
    public AudioSource quiterAudio;
    public AudioSource louderAudio;

    public GameObject deathPoof = null;
    public GameObject blinkEffect = null;
    public GameObject swapEffect = null;
 
    public float jumpLength = 1;
    public float jumpAnimationSpeed = 1f;

    int jumpMultiplier;
    float wallJumpMultiplier;

    [HideInInspector] public SkeletonAnimation frontSkeleton;
    [HideInInspector] public SkeletonAnimation backSkeleton;
    [HideInInspector] public bool usingFrontSkeleton = false;

    [Header("Obligatory")]
    public AnimationReferenceAsset frontIdle;
    public AnimationReferenceAsset frontJump;
    public AnimationReferenceAsset backIdle;
    public AnimationReferenceAsset backJump;

    [Header("Players Only")]
    public AnimationReferenceAsset frontIdle2 = null;
    public AnimationReferenceAsset frontIdle3 = null;
    public AnimationReferenceAsset backIdle2 = null;
    public AnimationReferenceAsset backIdle3 = null;
    public AnimationReferenceAsset frontWinJump = null;

    [Header("Rats Only")]
    public AnimationReferenceAsset frontAttack = null;
    public AnimationReferenceAsset backAttack = null;

    Animator turnAnimator;
    public float turnAnimatorSpeed = 1.5f;
    int newAnimationDirectionID = 0;
    int lastAnimationDirectionID = 0;
    [HideInInspector] public Spine.Animation nextIdleAnimation;
    [HideInInspector] public Spine.Animation nextJumpAnimation;
    [HideInInspector] public Spine.Animation nextWinAnimation; // players only
    [HideInInspector] public Spine.Animation nextAttackAnimation; // rats only

    #region Animation
    public void UpdateSkinBasedOnPlayerID()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SkeletonAnimation>() != null && child.name == "Front_Skeleton")
            {
                frontSkeleton = child.GetComponent<SkeletonAnimation>();
            }
            else if (child.GetComponent<SkeletonAnimation>() != null && child.name == "Back_Skeleton")
            {
                backSkeleton = child.GetComponent<SkeletonAnimation>();
            }
        }
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Animator>() != null)
            {
                turnAnimator = child.GetComponent<Animator>();
            }
        }
        newAnimationDirectionID = currentDirectionID;
        lastAnimationDirectionID = newAnimationDirectionID;
        RatProperties rp = GetComponent<RatProperties>();
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

    public void SetAnimation(int dataID, GameObject character = null, bool wallJump = false, bool ratAttack = false, bool winAnim = false)
    {
        if (frontSkeleton == null || backSkeleton == null)
        {
            return;
        }
        if (wallJump)
            dataID = 1;
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
        if (winAnim)
        {
            nextWinAnimation = frontWinJump;
            frontSkeleton.AnimationState.SetAnimation(0, nextWinAnimation, true);
            return;
        }
        switch (dataID)
        {
            case 0: // Jump Forward
                //AnimationCurveHandler animation = character.GetComponent<AnimationCurveHandler>();
                if (usingFrontSkeleton)
                {
                    frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                    if (wallJump)
                    {
                        //frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationStart = animation.jumpAnimTimer;
                    }
                    frontSkeleton.timeScale = jumpAnimationSpeed;
                    StartCoroutine(PrepareIdleAnimation(frontSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationEnd / frontSkeleton.timeScale));
                }
                else
                {
                    backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false);
                    if (wallJump)
                    {
                        //backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationStart = animation.jumpAnimTimer;
                    }
                    backSkeleton.timeScale = jumpAnimationSpeed;
                    StartCoroutine(PrepareIdleAnimation(backSkeleton.AnimationState.SetAnimation(0, nextJumpAnimation, false).AnimationEnd / backSkeleton.timeScale));
                }
                break;

            case 1:
                if (CompareTag("Enemy") || wallJump)
                {
                    return;
                }

                // disable skeletons, enable monke ;)
                frontSkeleton.gameObject.SetActive(false);
                backSkeleton.gameObject.SetActive(false);
                turnAnimator.gameObject.SetActive(true);
                turnAnimator.speed = turnAnimatorSpeed;
                if (newAnimationDirectionID < lastAnimationDirectionID) // turn left
                {
                    switch (Mathf.Abs(newAnimationDirectionID % 4)) // gives currentDirectionID
                    {
                        case 0:
                            turnAnimator.SetTrigger("front_back_left");
                            break;

                        case 1:
                            turnAnimator.SetTrigger("front_left");
                            break;

                        case 2:
                            turnAnimator.SetTrigger("back_front_left");
                            break;

                        case 3:
                            turnAnimator.SetTrigger("back_left");
                            break;

                        default:
                            Debug.Log(newAnimationDirectionID + " was not used correctly.");
                            break;
                    }
                }
                else if (newAnimationDirectionID > lastAnimationDirectionID) // turn right
                {
                    switch (Mathf.Abs(newAnimationDirectionID % 4)) // gives currentDirectionID
                    {
                        case 0:
                            turnAnimator.SetTrigger("back_right");
                            break;

                        case 1:
                            turnAnimator.SetTrigger("back_front_right");
                            break;

                        case 2:
                            turnAnimator.SetTrigger("front_right");
                            break;

                        case 3:
                            turnAnimator.SetTrigger("front_back_right");
                            break;

                        default:
                            Debug.Log(newAnimationDirectionID + " was not used correctly.");
                            break;
                    }
                }
                else
                {
                    Debug.Log("animationDirectionID = lastAnimationDirectionID");
                }

                StartCoroutine(PrepareIdleAnimation(0.25f / turnAnimatorSpeed));
                break;

            default:
                break;
        }
        GetComponent<RandomAnimationHandler>().StartRandomizeIdleAnimation(character);
    }

    public void RepeatIdleRandomizer(GameObject character)
    {
        GetComponent<RandomAnimationHandler>().StartRandomizeIdleAnimation(character);
    }

    IEnumerator PrepareIdleAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (CompareTag("Player"))
            turnAnimator.gameObject.SetActive(false);
        if (usingFrontSkeleton)
        {
            frontSkeleton.gameObject.SetActive(true);
            frontSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
            frontSkeleton.timeScale = 1;
        }
        else
        {
            backSkeleton.gameObject.SetActive(true);
            backSkeleton.AnimationState.SetAnimation(0, nextIdleAnimation, true);
            backSkeleton.timeScale = 1;
        }
    }
    #endregion

    #region Movement
    public bool TryMove(GameObject character, int dataID, int increment, int typeID = 0)
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
                    TryMove(character, 1, 2, 1);
                    if (mediumAudio != null)
                        mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
                    return false;

                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Move(character, 1);
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
                        TryMove(gameObject, 0, 1);
                        if (mediumAudio != null)
                            mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().pushHit);

                        return true;
                    }

                    return false;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    if(enemy.GetComponent<RatProperties>().DoAMove(0, 1, currentDirectionID) == true)
                    {
                        TryMove(character, 0, 1, forcedJump);
                    }

                    break;

                case GridManager.DOOR:

                    break;

                case GridManager.MARBLE:
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<AnimationCurveHandler>().PickupMarble(character, increment);
                        Move(character, 1);
                    }

                    return true;

                case GridManager.HOLE:
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
                  
                    TryMove(character, 2, increment - 1);
                    if (mediumAudio != null)
                        mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().wallHit);
                    
                    return false;

                case GridManager.WALKABLEGROUND: // WALKABLEGROUND
                    Blink(increment);
                    savedTile = 'X';
                    return true;

                case GridManager.PLAYER: // PLAYER rat is able to push player
                    GameObject player = grid.FindPlayerInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, TurnManager.players);

                    if ( gridPosition + RequestGridPosition(currentDirectionID, increment) == gridPosition)
                    {
                        Blink(increment);
                        grid.levels[GridManager.currentLevel][(int)gridPosition.x, (int)gridPosition.y] = GridManager.PLAYER;
                        return true;
                    }

                    if (player.GetComponent<PlayerProperties>().Pushed(character.GetComponent<Movement>().currentDirectionID) == true)
                    {
                        Blink(increment); // else blink increment--
                        if (mediumAudio != null)
                            mediumAudio.PlayOneShot(FindObjectOfType<AudioManager>().pushHit);
                        return true;
                    }
                    else
                    {
                        
                        TryMove(character, 2, increment - 1); // it sees itself if standing next to the player
                        // if there is a
                    }
                    return false;

                case GridManager.ENEMY: // ENEMY
                    GameObject enemy = grid.FindInMatrix(RequestGridPosition(currentDirectionID, increment)
                        + character.GetComponent<Movement>().gridPosition, enemies);

                    if(enemy.GetComponent<RatProperties>().DoAMove(0, 1, currentDirectionID) == true)
                    {
                        Blink(increment);
                    }
                    else
                    {
                        TryMove(character, 2, increment - 1);
                    }


                    break;

                case GridManager.DOOR:
                    break;

                case GridManager.MARBLE:
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<AnimationCurveHandler>().PickupMarble(character, increment);
                        Blink(increment);
                    }

                    return true;

                case GridManager.HOLE:
                    grid.MoveInGridMatrix(character.GetComponent<Movement>(), new Vector2(0, 0));
                    if (CompareTag("Player"))
                    {
                        character.GetComponent<PlayerProperties>().Death(gg.GetRealWorldPosition(gridPosition +
                            RequestGridPosition(currentDirectionID, 3)) + new Vector2(0, 0.7f));
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
            lastAnimationDirectionID = newAnimationDirectionID;
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                jumpMultiplier = 1;
                if (increment < 0)
                {
                    jumpMultiplier *= -1;
                }
                currentDirectionID += jumpMultiplier;
                newAnimationDirectionID += jumpMultiplier;
                if (currentDirectionID <= -4 || currentDirectionID >= 4)
                {
                    currentDirectionID = 0;
                }
            }
            UpdateSkeleton();
            if (typeID == 1) // if wall jump
                SetAnimation(dataID, character, true);
            else
                SetAnimation(dataID, character);
        }
        return false;
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
            //wallJumpMultiplier *= 0.5f;
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
        if (gameObject.GetComponent<Animator>() != null)
        {
            gameObject.GetComponent<Animator>().SetTrigger("shrink");
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
        if (gameObject.GetComponent<Animator>() != null)
        {
            gameObject.GetComponent<Animator>().SetTrigger("grow");
        }

        yield return null;
    }
    #endregion

    public abstract char ChangeTag();
    public abstract bool DoAMove(int id, int inc, int dir);
}
