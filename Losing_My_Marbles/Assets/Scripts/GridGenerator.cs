using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    GridManager grid;

    [SerializeField] GameObject tileToCopy;
    [SerializeField] Sprite[] tileSprites = new Sprite[17];
    [SerializeField] Sprite tileHoleSprite = null;
    readonly float tileSize = 1f;
    int tileSpriteChosen;

    void Start()
    {
        tileToCopy.GetComponent<SpriteRenderer>().sortingOrder = 0;
        grid = transform.GetComponentInParent<GridManager>();

        for (int x = 0; x < grid.board.GetLength(0); x++)
        {
            for (int y = 0; y < grid.board.GetLength(1); y++)
            {
                if(grid.board[x, y] != GridManager.EMPTY && grid.board[x, y] != GridManager.DOOR)
                {
                    GameObject newTile = Instantiate(tileToCopy);
                    float posX = ((x * tileSize + y * tileSize)) + tileToCopy.transform.position.x + 2f; // this is the actual x position of the tile
                    float posY = ((-x * tileSize + y * tileSize) / 2) + tileToCopy.transform.position.y + 1.5f; // this is the actual y position of the tile
                    newTile.transform.position = new Vector3(posX, posY, 0);
                    newTile.transform.parent = gameObject.transform;
                    tileSpriteChosen = Random.Range(0, tileSprites.Length);
                    newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[tileSpriteChosen];
                    if (grid.board[x, y] == GridManager.HOLE && tileHoleSprite != null)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = tileHoleSprite;
                    }
                }
            }
        }
    }
}
