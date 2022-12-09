using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public AnimationCurve jumpProgress;
    [HideInInspector] public GameObject characterToAnimate;
    [HideInInspector] public Vector2 destination;
    [HideInInspector] public float animTimer = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animTimer += Time.deltaTime;
        if (animTimer <= jumpProgress.length)
        {
            characterToAnimate.transform.position = new Vector2(Mathf.Lerp(characterToAnimate.transform.position.x, destination.x, jumpProgress.Evaluate(animTimer)),
            Mathf.Lerp(characterToAnimate.transform.position.y, destination.y, jumpProgress.Evaluate(animTimer)));
        }
        
    }

    public void TransitionFromTo(GameObject character, Vector3 position)
    {
        characterToAnimate = character;
        destination = position;
        animTimer = 0;
    }
}
