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

    public char[,] board;
    void Start()
    {
        board = new char[9, 9]
        {
            {'X','P','X','X','X','X','X','X','P'},
                {'X','X','X','X','X','X','X','X','X'},
                    {'X','X','X','X','X','X','X','X','X'},
                        {'X','X','X','X','X','X','X','X','X'},
                            {'X','X','X','X','X','X','X','X','X'},
                                {'X','X','X','X','X','X','X','X','X'},
                                    {'X','X','X','X','X','X','X','X','X'},
                                        {'X','X','X','X','X','X','X','X','X'},
                                            {'X','X','X','X','X','X','X','X','X'}
            // what happens to a player if a hazard turns on wwhile standing in it, GridLogic?
        };
        for(int x = 0; x < board.GetLength(0); x++)
        {
            for(int y = 0; y < board.GetLength(1); y++)
            {
                switch (board[x, y])
                {
                    case WALKABLEGROUND:
                        GameObject newTile = Instantiate(prefab);
                        float posX = ((x * tileSize + y * tileSize) / 2) + transform.position.x; // this is the actual x position of the tile
                        float posY = ((-x * tileSize + y * tileSize) / 4) + transform.position.y; // this is the actual y position of the tile
                        newTile.transform.position = new Vector3(posX, posY, 0);
                      //  newTile.transform.parent = gameObject.transform;
                        break;
                    default:
                        break;
                }
            }
           
        }
        
        switch (IsSquareEmpty(0,0))
        {
            case 0: //wall
                //do wall stuff
                break;
            case 1: 
                //Move
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
    private void Move(GameObject character, int x, int y)
    {
        int savedX ;
        int savedY ;

        
    }
    public int IsSquareEmpty (int x, int y)
    {
        if(x > board.GetLength(0) || x < 0 || y > board.GetLength(0) || y < 0)
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
