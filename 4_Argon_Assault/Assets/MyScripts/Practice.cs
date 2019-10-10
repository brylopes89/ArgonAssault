using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            {
            MoveUpFunction();
            }
    }

    void MoveUpFunction()
    {
        transform.position += Vector3.up * speed;
            
    }
}
