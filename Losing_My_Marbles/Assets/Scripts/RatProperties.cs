using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RatProperties : Movement
{
    public int enemyID = 0;

    public GameObject deathPoof = null;

    int savedDir;

    GridManager gridManager;

    readonly List <Vector2> killZone = new() 
    { 
        new Vector2 (0, 1), new Vector2( 0, -1), new Vector2 ( 1, 0), new Vector2 (-1, 0)
    };
    public List<Vector2> moves = new();
    
    private void Awake()
    {
        enemies.Add(this);
    }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>().GetComponent<GridManager>();
        UpdateSkeleton();
    }

    private void Update()
    {
        if (enemyID == DebugManager.characterToControl)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                TryMove(gameObject, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TryMove(gameObject, 1, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TryMove(gameObject, 1, 1);
            }
        }
    }

    private void OnDestroy()
    {
        Movement.enemies.Remove(this);
    }

    public override char ChangeTag()
    {
        return 'E';
    }

    public void Death()
    {
        gridManager.board[(int)gridPosition.x, (int)gridPosition.y] = savedTile;
        enemies.Remove(this);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SkeletonAnimation>() && (child.name == "Front_Skeleton" || child.name == "Back_Skeleton"))
            {
                child.gameObject.SetActive(false);
            }
        }
        if (deathPoof == null)
            return;
        GameObject newPoof = Instantiate(deathPoof, transform.position, transform.rotation);
        Destroy(newPoof, 1f);
    }

    public override void DoAMove(int dataID, int increment , int dir)
    {
        savedDir = gameObject.GetComponent<RatProperties>().currentDirectionID;
        gameObject.GetComponent<RatProperties>().currentDirectionID = dir;
         
        if (TryMove(gameObject, dataID, increment) == true)
        {
            gameObject.GetComponent<RatProperties>().currentDirectionID = savedDir;
        }
        if (gameObject.GetComponent<RatProperties>().isActiveAndEnabled == true)
        {
            StartCoroutine(CheckForKills());
        }
    }
    
    public IEnumerator CheckForKills() 
    {
        for(int i = 0; i < killZone.Count; i++)
        {
            if(gridManager == null)
            {
                gridManager.GetComponent<GridManager>();
            }
            if (gridManager.GetNexTile(gameObject, killZone[i]) == 'P')
            {
                // add animation
                yield return new WaitForSeconds(0.5f); // viktigt att notera, du kan inte ha denna timern för seg för då missar du den andra coroutinen.
                GameObject player = gridManager.FindPlayerInMatrix(killZone[i] // find player with your grid pos and the tile you detected player on
                        + gridPosition, TurnManager.players);
                player.GetComponent<PlayerProperties>().Death(player.transform.position);
            }
        }
    }
}