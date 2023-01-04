using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public const char WALKABLEGROUND = 'X';    // 1
    public const char PLAYER = 'P';            // 2
    public const char ENEMY = 'E';             // 3
    public const char DOOR = 'D';
    public const char MARBLE = 'M';
    public const char HOLE = 'H';
    public const char EMPTY ='?';
    public const char WATER = 'W';

    public static int currentLevel = 0;

    public static List<int> randomizedNumbers = new();
    public static char[,] level0 = new char[10, 10] // [column, row in column] or [y, x]
    {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','M','H','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'H','H','X','X','X','X','X','H','H','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
    };
    public static char[,] level1 = new char[10, 10]
    {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','H','X','M','X','H','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
    };
    public static char[,] level2 = new char[10, 10]
    {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','M','X','X','X','X','X','X','?'},
            {'X','X','H','X','X','X','X','M','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','M','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'}
    };
    public static char[,] level3 = new char[10, 10]
    {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','H','M','H','X','H','M','H','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
    };
    public static char[,] level4 = new char[10, 10]
   {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'H','X','X','X','X','X','X','X','H','?'},
            {'X','H','X','X','X','X','X','H','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
   };
    
    public char[][,] levels = new char[5][,]
    {
        level0, level1, level2, level3, level4
    };
   
    void Start()
    {
        ResetLevels();   
       
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        for (int i = 0; i < Movement.enemies.Count; i++)
        {
            levels[currentLevel][(int)Movement.enemies[i].gridPosition.x,
                (int)Movement.enemies[i].gridPosition.y] = ENEMY;
        }

        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            levels[currentLevel][(int)TurnManager.players[i].gridPosition.x,
              (int)TurnManager.players[i].gridPosition.y] = PLAYER;
        }
    }
   
    private void OnDrawGizmos()
    {
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        if (levels[currentLevel] == null) return;
        if (levels[currentLevel].Length == 0) return;

        for (int y = 0; y < levels[currentLevel].GetLength(0); y++)
        {
            for (int x = 0; x < levels[currentLevel].GetLength(1); x++)
            {
                switch (levels[currentLevel][y, x])
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
                    case MARBLE:
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

        if (x >= levels[currentLevel].GetLength(0) || x < 0 || y >= levels[currentLevel].GetLength(0) || y < 0)
            return EMPTY;

        return levels[currentLevel][x, y] switch
        {
            WALKABLEGROUND => WALKABLEGROUND,
            PLAYER => PLAYER,
            ENEMY => ENEMY,
            DOOR => DOOR,
            MARBLE => MARBLE,
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

        levels[currentLevel][newX, newY] = character.ChangeTag();
        levels[currentLevel][(int)oldX, (int)oldY] = character.savedTile;

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
   
    private void ResetLevels()
    {
        level0 = new char[10, 10] // [column, row in column] or [y, x]
        {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','M','H','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'H','H','X','X','X','X','X','H','H','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
        };
        level1 = new char[10, 10]
        {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','H','X','M','X','H','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
        };
        level2 = new char[10, 10]
        {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','M','X','X','X','X','X','X','?'},
            {'X','X','H','X','X','X','X','M','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','M','?'},
            {'X','X','X','X','X','H','X','X','X','?'},
            {'X','X','X','X','X','H','X','X','X','?'}
        };
        level3 = new char[10, 10]
        {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','H','M','H','X','H','M','H','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
        };
        level4 = new char[10, 10]
        {
            {'?','?','?','?','?','?','?','?','?','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','H','X','X','X','X','?'},
            {'H','X','X','X','X','X','X','X','H','?'},
            {'X','H','X','X','X','X','X','H','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'},
            {'X','X','X','X','M','X','X','X','X','?'},
            {'X','X','X','X','X','X','X','X','X','?'}
        };
    }
}
