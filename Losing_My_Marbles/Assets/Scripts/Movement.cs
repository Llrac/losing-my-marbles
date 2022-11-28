using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2 gridPosition = new(1, 0);
    public Sprite[] sprites;
    SpriteRenderer sr;

    public float jumpLength = 1;

    int currentDirectionID = 0;

    GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Sprite")
                body = child.gameObject;
        }
        
        sr = body.GetComponent<SpriteRenderer>();

        TryMove(gameObject, 1, 0);
    }

    public void TryMove(GameObject character, int dataID, int increment)
    {
        Move(character, dataID, increment);
    }

    void Move(GameObject character, int dataID, int increment)
    {
        // increment should not change when moving
        if (dataID != 0)
            currentDirectionID += increment;

        if (currentDirectionID <= -4 || currentDirectionID >= 4)
        {
            currentDirectionID = 0;
        }

        // dataID 0 concerns transform position
        // dataID 1 concerns character rotation

        // Set transform position
        if (dataID == 0)
        {
            for (int i = 0; i < Mathf.Abs(increment); i++)
            {
                switch (currentDirectionID)
                {
                    case 1 or -3:
                        character.transform.position += new Vector3(jumpLength, -jumpLength / 2, 0);
                        break;
                    case 2 or -2:
                        character.transform.position += new Vector3(-jumpLength, -jumpLength / 2, 0);
                        break;
                    case 3 or -1:
                        character.transform.position += new Vector3(-jumpLength, jumpLength / 2, 0);
                        break;
                    case 0:
                        character.transform.position += new Vector3(jumpLength, jumpLength / 2, 0);
                        break;
                }
            }
        }

        // Set character rotation
        if (dataID == 1)
        {
            switch (currentDirectionID)
            {
                case 1 or -3:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2 or -2:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3 or -1:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 0:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
            }
        }
    }
}
