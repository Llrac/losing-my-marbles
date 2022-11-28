using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private const char PLAYER = 'P';
    private const char WALKABLEGROUND = 'X';
    private float tileSize = 2f;
    [SerializeField] private GameObject prefab; // remember that the tiles position has its reference pint in the middle of the pillar. needs offset.

    public char[,] board;
    void Start()
    {
        board = new char[9, 9]
        {
            {'P','X','X','X','X','P','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'P','X','X','X','X','X','X','X','P'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'} // what happens to a player if a hazard turns on wwhile standing in it, GridLogic?
        };
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                switch (board[i, j])
                {
                    case PLAYER:
                        GameObject newTile = Instantiate(prefab);
                        float posX = ((i * tileSize + j * tileSize) / 2) + transform.position.x; // this is the actual x position of the tile
                        float posY = ((-i * tileSize + j * tileSize) / 4) + transform.position.y; // this is the actual y position of the tile
                        newTile.transform.position = new Vector3(posX, posY, 0);
                      //  newTile.transform.parent = gameObject.transform;
                        break;
                    default:
                        break;
                }
            }
           
        }
        
    }
    public void TryMove(GameObject player, Vector2 requestedTile)
    {
        Vector2 vector2 = (Vector2)player.transform.position + requestedTile;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                switch (board[i, j])
                {
                    case WALKABLEGROUND:
                        float posX = ((i * tileSize + j * tileSize) / 2) + prefab.transform.position.x; // this is the actual x position of the tile
                        float posY = ((-i * tileSize + j * tileSize) / 4) + prefab.transform.position.y; // this is the actual y position of the tile
                       
                        break;
                    default:
                        break;
                }
            }

        }
    }
    
}
