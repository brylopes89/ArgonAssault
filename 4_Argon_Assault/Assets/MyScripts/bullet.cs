using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet : MonoBehaviour {
   
    public GameObject explo;   

    // Use this for initialization
    private void Start()
    {
       /* ps = GetComponent<ParticleSystem>();       
        collisionEvents = new ParticleCollisionEvent[8];*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Instantiate(explo, pos, rot);
    }

    /* public void OnParticleCollision(GameObject other)
     {
         int collCount = ps.GetSafeCollisionEventSize();
         Debug.Log("Hit: " + other.gameObject);

        // Rigidbody rb = other.GetComponent<Rigidbody>();

         int eventCount = ps.GetCollisionEvents(other, collisionEvents);      

         for (int i = 0; i < eventCount; i++)
         {
             //TODO: Do your collision stuff here. 
             // You can access the CollisionEvent[i] to obtaion point of intersection, normals that kind of thing
             // You can simply use "other" GameObject to access it's rigidbody to apply force, or check if it implements a class that takes damage or whatever            
             Vector3 pos = collisionEvents[eventCount].intersection;
             Collider[] colliders = Physics.OverlapSphere(pos, radius);    

             Instantiate(explo, pos, Quaternion.identity);
         }        
     }*/
}
