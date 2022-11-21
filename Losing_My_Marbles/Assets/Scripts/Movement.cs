using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(transform.up.x, transform.up.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(45, 0, 0);
        }
    }
}
