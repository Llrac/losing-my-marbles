using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbly : MonoBehaviour
{
    private Vector3 hoover;
    void Update()
    {
        hoover = Vector3.up * Mathf.Cos(Time.time);
        transform.position = new Vector3 (transform.position.x,hoover.y,0);
    }
}
