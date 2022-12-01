using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    GridManager grid;

    [SerializeField] GameObject tileToImitate;
    [SerializeField] Sprite[] tileSprites = new Sprite[17];
    float tileSize = 1f;
    int tileSpriteChosen;

    void Start()
    {
        //grid = FindObjectOfType<GridManager>();
        //if (grid == null)
        //{
        //    grid = FindObjectOfType<GridManager>();
        //}
        tileToImitate.GetComponent<SpriteRenderer>().sortingOrder = 0;
        grid = transform.GetComponentInParent<GridManager>();

        for (int i = 0; i < grid.board.GetLength(0); i++)
        {
            for (int j = 0; j < grid.board.GetLength(1); j++)
            {
                GameObject newTile = Instantiate(tileToImitate);
                float posX = ((i * tileSize + j * tileSize)) + tileToImitate.transform.position.x; // this is the actual x position of the tile
                float posY = ((-i * tileSize + j * tileSize) / 2) + tileToImitate.transform.position.y; // this is the actual y position of the tile
                newTile.transform.position = new Vector3(posX, posY, 0);
                newTile.transform.parent = gameObject.transform;
                tileSpriteChosen = Random.Range(0, tileSprites.Length);
                newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[tileSpriteChosen];
            }
        }
    }
}
