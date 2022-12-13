using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public const char WALKABLEGROUND = 'X';    // 1
    public const char PLAYER = 'P';            // 2
    public const char ENEMY = 'E';             // 3
    public const char DOOR = 'D';
    public const char KEY = 'K';
    public const char HOLE = 'H';
    public const char EMPTY='?';
    public const char WATER = 'W';
    public char[,] board = new char[10, 10] // [column, row in column] or [y, x]
    {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'H','X','X','X','X','X','X','X','H','?'},
            {'X','X','X','H','X','H','X','X','X','?'},
            {'X','X','X','X','K','X','X','X','X','D'},
            {'X','X','X','H','X','H','X','X','X','?'},
            {'H','X','X','X','X','X','X','X','H','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
    };

    void Start()
    {
        for (int i = 0; i < Movement.enemies.Count; i++)
        {
            board[(int)Movement.enemies[i].gridPosition.x,
                (int)Movement.enemies[i].gridPosition.y] = ENEMY;

            
        }
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            board[(int)TurnManager.players[i].gridPosition.x,
              (int)TurnManager.players[i].gridPosition.y] = PLAYER;
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
                    case PLAYER:
                        Gizmos.color = Color.green;
                        break;
                    case WALKABLEGROUND:
                        Gizmos.color = Color.blue;
                        break;
                    case ENEMY:
                        Gizmos.color = Color.red;
                        break;
                    case DOOR:
                        Gizmos.color = Color.white;
                        break;
                    case KEY:
                        Gizmos.color = Color.yellow;
                        break;
                    case HOLE:
                        Gizmos.color = Color.black;
                        break;
                    case EMPTY:
                        Gizmos.color = Color.clear;
                        break;
                    case WATER:
                        Gizmos.color = Color.cyan;
                        break;
                }
                Gizmos.DrawSphere(Vector3.down * (y - 5f) + Vector3.right * x, 0.5f);
            }
        }
    }

    public char GetNexTile(GameObject character, Vector2 requestedTile)
    {

        Movement moveScript = character.GetComponent<Movement>();

        int x = (int)moveScript.gridPosition.x + (int)requestedTile.x;
        int y = (int)moveScript.gridPosition.y + (int)requestedTile.y;

        if (x >= board.GetLength(0) || x < 0 || y >= board.GetLength(0) || y < 0)
            return EMPTY;

        return board[x, y] switch
        {
            WALKABLEGROUND => WALKABLEGROUND,
            PLAYER => PLAYER,
            ENEMY => ENEMY,
            DOOR => DOOR,
            KEY => KEY,
            HOLE => HOLE,
            WATER => WATER,
            _ => EMPTY,
        };
    }

    public void MoveInGridMatrix(Movement character, Vector2 requestedTile)
    {
        float oldX = character.gridPosition.x;
        float oldY = character.gridPosition.y;

        int newX = (int)character.gridPosition.x + (int)requestedTile.x;
        int newY = (int)character.gridPosition.y + (int)requestedTile.y;

        board[newX, newY] = character.ChangeTag();
        board[(int)oldX, (int)oldY] = character.savedTile;

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
    public GameObject FindPlayerInMatrix(Vector2 tile, List<PlayerProperties> list)
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
