using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += transform.forward * (speed * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
