using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    const char WALKABLEGROUND = 'X';    // 1
    const char PLAYER = 'P';            // 2
    const char ENEMY = 'E';             // 3

    public char[,] board;

    void Awake()
    {
        board = new char[9, 9]
        {
            // what happens to a player if a hazard turns on while standing in it, GridLogic?
            {'P','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'},
            {'X','X','X','X','X','X','X','X','X'} // sortinglayer 8
        };
        for (int i = 0; i < Movement.enemies.Count; i++)
        {
            board[(int)Movement.enemies[i].gridPosition.x, (int)Movement.enemies[i].gridPosition.y] = ENEMY;
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
                        Gizmos.color = Color.green;
                        break;
                    case 'X':
                        Gizmos.color = Color.blue;
                        break;
                    case 'E':
                        Gizmos.color = Color.red;
                        break;
                }
                Gizmos.DrawSphere(Vector3.down * (y - 5f) + Vector3.right * x, 0.5f);
            }
        }
    }

    public int IsSquareEmpty(GameObject character, Vector2 requestedTile)
    {
        Movement moveScript = character.GetComponent<Movement>();

        int x = (int)moveScript.gridPosition.x + (int)requestedTile.x;
        int y = (int)moveScript.gridPosition.y + (int)requestedTile.y;

        if (x >= board.GetLength(0) || x < 0 || y >= board.GetLength(0) || y < 0)
            return 0;

        return board[x, y] switch
        {
            WALKABLEGROUND => 1,
            PLAYER => 2,
            ENEMY => 3,
            _ => 0,
        };
    }

    public void MoveInGridMatrix(Movement character, Vector2 requestedTile)
    {
        float oldX = character.gridPosition.x;
        float oldY = character.gridPosition.y;

        int newX = (int)character.gridPosition.x + (int)requestedTile.x;
        int newY = (int)character.gridPosition.y + (int)requestedTile.y;

        board[newX, newY] = character.ChangeTag();
        board[(int)oldX, (int)oldY] = WALKABLEGROUND;

        character.gridPosition += requestedTile;
    }
    public GameObject FindInMatrix(Vector2 tile, List<Movement> list)
    {
        for (int i = 0; i <= list.Count - 1; i++)
        {
            Movement movement = list[i];
            if (movement.gridPosition == tile)
            {
                return movement.gameObject;
            }
        }
        return null;
    }

}
