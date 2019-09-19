using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float Speed = 50.0f;
    //public GameObject Planetoid;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Speed * Time.deltaTime);
        //transform.RotateAround(Planetoid.transform.position, Vector3.up, Speed * Time.deltaTime);
    }
}
