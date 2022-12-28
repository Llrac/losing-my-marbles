using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    GridManager grid;

    [Header("Particle Effects")]
    [SerializeField] GameObject mysteryMarble = null;
    [SerializeField] GameObject hitEffect = null;

    [SerializeField] GameObject tileToCopy;
    [SerializeField] GameObject bomb;
    [SerializeField] Sprite[] tileSprites = new Sprite[17];
    [SerializeField] Sprite tileHoleSprite = null;
    readonly float tileSize = 1f;
    int tileSpriteChosen;
    GameObject newTile;
    GameObject newMysteryMarble;
    GameObject newHit;

    void Start()
    {
        tileToCopy.GetComponent<SpriteRenderer>().sortingOrder = 0;
        grid = transform.GetComponentInParent<GridManager>();

        for (int x = 0; x < grid.levels[GridManager.currentLevel].GetLength(0); x++)
        {
            for (int y = 0; y < grid.levels[GridManager.currentLevel].GetLength(1); y++)
            {
                if (grid.levels[GridManager.currentLevel][x, y] != GridManager.EMPTY) //&& grid.board[x, y] != GridManager.DOOR
                {
                    newTile = Instantiate(tileToCopy);
                    float posX = x * tileSize + y * tileSize + tileToCopy.transform.position.x -1; // this is the actual x position of the tile
                    float posY = ((-x * tileSize + y * tileSize) / 2) + tileToCopy.transform.position.y + .5f; // this is the actual y position of the tile
                    newTile.transform.position = new Vector3(posX, posY, 0);
                    newTile.transform.parent = gameObject.transform;
                    tileSpriteChosen = Random.Range(0, tileSprites.Length);
                    newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[tileSpriteChosen];
                    if (grid.levels[GridManager.currentLevel][x, y] == GridManager.HOLE && tileHoleSprite != null)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = tileHoleSprite;
                    }
                    else if (grid.levels[GridManager.currentLevel][x, y] == GridManager.MARBLE && mysteryMarble != null)
                    {
                        newMysteryMarble = Instantiate(mysteryMarble);
                        newMysteryMarble.transform.position = new Vector2(newTile.transform.position.x, newTile.transform.position.y + 1);
                        newMysteryMarble.GetComponent<MysteryMarble>().gridPosition = new Vector2(x, y); // maybe swap x & y if it doesn't work
                    }
                }
            }
        }
    }

    public void OnHitWall(GameObject characterToApplyEffect)
    {
        if (hitEffect != null)
        {
            newHit = Instantiate(hitEffect);
            newHit.transform.position = new Vector2(characterToApplyEffect.transform.position.x, characterToApplyEffect.transform.position.y);
        }
    }
    public void DeployBomb(Vector2 destination)
    {
        float posX = destination.x * tileSize + destination.y * tileSize + tileToCopy.transform.position.x - 1; // this is the actual x position of the tile
        float posY = ((-destination.x * tileSize + destination.y * tileSize) / 2) + tileToCopy.transform.position.y + 1.5f;
        GameObject newBomb = Instantiate(bomb, new Vector3(posX, posY, 0), Quaternion.identity);
        Destroy(newBomb, 1.5f);
    }

    public Vector2 GetRealWorldPosition(Vector2 gridPosition)
    {
        float posX = gridPosition.x * tileSize + gridPosition.y * tileSize + tileToCopy.transform.position.x - 1;
        float posY = ((-gridPosition.x * tileSize + gridPosition.y * tileSize) / 2) + tileToCopy.transform.position.y + 1.5f;

        return new Vector2(posX, posY);
    }
}
