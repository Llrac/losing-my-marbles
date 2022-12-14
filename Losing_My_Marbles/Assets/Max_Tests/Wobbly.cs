using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbly : MonoBehaviour
{
    private Vector3 endPos;
    private float randomActivation;
    private Wobbly wob;
   
    private void Start()
    {
        endPos = transform.position - new Vector3(0, 5f, 0);
        randomActivation = Random.Range(0, 2f);
        wob = GetComponent<Wobbly>();
    }
    private void Update()
    {
        randomActivation -= 1f * Time.deltaTime;

        if(randomActivation <= 0) // && true
        {
            transform.position += new Vector3(0, -4f, 0) * Time.deltaTime;

            if (transform.position.y <= endPos.y)
            {
                transform.position = endPos;
                wob.enabled = false;
            }
        }
       
    }
  
}
