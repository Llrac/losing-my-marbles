using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    const char WALKABLEGROUND = 'X';    // 1
    const char PLAYER = 'P';            // 2
    const char ENEMY = 'E';             // 3

    PlayerProperties pp;

    public char[,] board;

    void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
        board = new char[9, 9]
        {
            // what happens to a player if a hazard turns on while standing in it, GridLogic?
            {'P','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','E','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'}
        };
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
                        Gizmos.color = Color.green;
                        break;
                    case 'X':
                        Gizmos.color = Color.blue;
                        break;
                    case 'E':
                        Gizmos.color = Color.red;
                        break;
                }
                Gizmos.DrawSphere(Vector3.down * (y - 5f ) + Vector3.right * x, 0.5f);
            }
        }
    }

    public int IsSquareEmpty(Vector2 requestedTile)
    {
        int x = (int)pp.gridPosition.x + (int)requestedTile.x;
        int y = (int)pp.gridPosition.y + (int)requestedTile.y;
        if (x >= board.GetLength(0) || x < 0 || y >= board.GetLength(0) || y < 0)
            return 0;
        return board[x, y] switch
        {
            WALKABLEGROUND => 1,
            PLAYER => 2,// maybe should return something else which could be used in another switch statement
            ENEMY => 3,
            _ => 0,
        };
    }

    public void MoveInGridMatrix(Vector2 requestedTile)
    {
        float oldX = pp.gridPosition.x;
        float oldY = pp.gridPosition.y;

        int newX = (int)pp.gridPosition.x + (int)requestedTile.x;
        int newY = (int)pp.gridPosition.y + (int)requestedTile.y;

        board[newX, newY] = PLAYER;
        board[(int)oldX, (int)oldY] = WALKABLEGROUND;

        pp.gridPosition += requestedTile;
    }
}
