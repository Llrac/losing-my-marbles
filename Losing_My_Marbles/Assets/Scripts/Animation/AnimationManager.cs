using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    public AnimationCurve jumpHeight;

    GameObject body;
    Vector2 destinationPosition;

    PlayerProperties pp;

    float timer = 1;

    private void Start()
    {
        pp = FindObjectOfType<PlayerProperties>();
    }

    public void Jump(GameObject character)
    {
        foreach (Transform child in character.transform)
        {
            if (child.gameObject.name == "Sprite")
                body = child.gameObject;
        }

        switch (pp.currentDirectionID)
        {
            case 0:
                destinationPosition = body.transform.position + new Vector3(pp.jumpLength, pp.jumpLength / 2, 0);
                Debug.Log("jump");
                break;

            case 1 or -3:
                destinationPosition = body.transform.position + new Vector3(pp.jumpLength, -pp.jumpLength / 2, 0);
                break;

            case 2 or -2:
                destinationPosition = body.transform.position + new Vector3(-pp.jumpLength, -pp.jumpLength / 2, 0);
                break;

            case 3 or -1:
                destinationPosition = body.transform.position + new Vector3(-pp.jumpLength, pp.jumpLength / 2, 0);
                break;

            default:
                break;
        }

        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 1)
        {
            body.transform.position = Vector2.Lerp(body.transform.position, destinationPosition,
                jumpProgress.Evaluate(timer));
        }
    }
}
