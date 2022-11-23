using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
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

        UpdatePlayerProperties(gameObject, 0, 1);
    }

    public void UpdatePlayerProperties(GameObject character, int increment, int whatToChange)
    {
        currentDirectionID += increment;
        if (currentDirectionID <= -4 || currentDirectionID >= 4)
        {
            currentDirectionID = 0;
        }

        // whatToChange 0 concerns transform position
        // whatToChange 1 concerns animation rotation

        // Set transform position
        if (whatToChange == 0)
        {
            switch (currentDirectionID)
            {
                case -3:
                    character.transform.position += new Vector3(jumpLength, -jumpLength/2, 0);
                    break;
                case -2:
                    character.transform.position += new Vector3(-jumpLength, -jumpLength/2, 0);
                    break;
                case -1:
                    character.transform.position += new Vector3(-jumpLength, jumpLength/2, 0);
                    break;
                case 0:
                    character.transform.position += new Vector3(jumpLength, jumpLength/2, 0);
                    break;
                case 1:
                    character.transform.position += new Vector3(jumpLength, -jumpLength/2, 0);
                    break;
                case 2:
                    character.transform.position += new Vector3(-jumpLength, -jumpLength/2, 0);
                    break;
                case 3:
                    character.transform.position += new Vector3(-jumpLength, jumpLength/2, 0);
                    break;
            }
        }

        // Set animation state
        if (whatToChange == 1)
        {
            switch (currentDirectionID)
            {
                case -3:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case -2:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case -1:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 0:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2:
                    sr.sprite = sprites[1];
                    character.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3:
                    sr.sprite = sprites[0];
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }
}
