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

        UpdatePlayerProperties(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdatePlayerProperties(0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            UpdatePlayerProperties(-1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdatePlayerProperties(1, 1);
        }
    }

    void UpdatePlayerProperties(int increment, int whatToChange)
    {
        currentDirectionID += increment;
        if (currentDirectionID <= -4 || currentDirectionID >= 4)
        {
            currentDirectionID = 0;
        }

        // whatToChange 0 concerns transform position
        // whatToChange 1 concerns body transform rotation

        // Set transform position
        if (whatToChange == 0)
        {
            switch (currentDirectionID)
            {
                case -3:
                    transform.position += new Vector3(jumpLength, -jumpLength, 0);
                    break;
                case -2:
                    transform.position += new Vector3(-jumpLength, -jumpLength, 0);
                    break;
                case -1:
                    transform.position += new Vector3(-jumpLength, jumpLength, 0);
                    break;
                case 0:
                    transform.position += new Vector3(jumpLength, jumpLength, 0);
                    break;
                case 1:
                    transform.position += new Vector3(jumpLength, -jumpLength, 0);
                    break;
                case 2:
                    transform.position += new Vector3(-jumpLength, -jumpLength, 0);
                    break;
                case 3:
                    transform.position += new Vector3(-jumpLength, jumpLength, 0);
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
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case -2:
                    sr.sprite = sprites[1];
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case -1:
                    sr.sprite = sprites[0];
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 0:
                    sr.sprite = sprites[0];
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1:
                    sr.sprite = sprites[1];
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2:
                    sr.sprite = sprites[1];
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 3:
                    sr.sprite = sprites[0];
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }
}
