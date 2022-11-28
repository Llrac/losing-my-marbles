using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private const char PLAYER = 'P';
    private const char WALKABLEGROUND = 'X';
    private const char ENEMY = 'E';
    private float tileSize = 2f;
    [SerializeField] private GameObject prefab; // remember that the tiles position has its reference pint in the middle of the pillar. needs offset.
    private GameObject character;
    public char[,] board;
    void Start()
    {
        character = FindObjectOfType<PlayerProperties>().gameObject;
        board = new char[9, 9]
        {
            {'X','P','X','X','X','X','X','X','P'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','P','X','X','X'},
            {'X','X','X','X','E','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'}
            // what happens to a player if a hazard turns on wwhile standing in it, GridLogic?
        };
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                switch (board[y, x])
                {
                    case WALKABLEGROUND:
                        GameObject newTile = Instantiate(prefab);
                        float posX = ((y * tileSize + x * tileSize) / 2) + transform.position.x; // this is the actual x position of the tile
                        float posY = ((-y * tileSize + x * tileSize) / 4) + transform.position.y; // this is the actual y position of the tile
                        newTile.transform.position = new Vector3(posX, posY, 0);
                        //  newTile.transform.parent = gameObject.transform;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void Update()
    {
        Vector2 worldToGrid = new Vector2 (0,0);
        switch (IsSquareEmpty(worldToGrid)) // should happen before moving
        {
            case 0: //wall
                //do wall stuff
                break;
            case 1:
                //Move
                MoveInGridMatrix(character, worldToGrid);
                break;
            case 2:
                // player
                //Push that player && move to square
                break;
            case 3:
                //Push enemy
                break;
        }

    }

    private void OnDrawGizmos()
    {
        if (board == null) return;
        if (board.Length == 0) return;

        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                switch (board[y, x])
                {
                    case 'P':
                        Gizmos.color = Color.red;
                        break;
                    case 'X':
                        Gizmos.color = Color.blue;
                        break;
                    case 'E':
                        Gizmos.color = Color.green;
                        break;
                }
                Gizmos.DrawSphere(Vector3.down * (y + -5f )+ Vector3.right * x, 0.5f);
            }


        }
    }


    private void MoveInGridMatrix(GameObject character, Vector2 requestedTile) // should be player properties grid position
    {
        PlayerProperties pp = character.GetComponent<PlayerProperties>();

        float savedX = pp.gridPosition.x;
        float savedY = pp.gridPosition.y;

        int x = (int)requestedTile.x; // viktigt att hålla koll på x och y
        int y = (int)requestedTile.y;

        board[x, y] = board[(int)savedX, (int)savedY] = PLAYER;
        board[(int)savedX, (int)savedY] = WALKABLEGROUND;
    }
    public int IsSquareEmpty(Vector2 requestedTile)
    {
        int x = (int)requestedTile.x;
        int y = (int)requestedTile.y;
        if (x > board.GetLength(0) || x < 0 || y > board.GetLength(0) || y < 0)
            return 0;
        switch (board[x, y])
        {
            case WALKABLEGROUND:
                return 1;

            case PLAYER:
                return 2; // maybe should return something else which could be used in another switch statement

            case ENEMY:
                return 3;

            default:
                return 0;
        }
    }
}
