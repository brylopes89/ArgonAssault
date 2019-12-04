using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet : MonoBehaviour {

    private ParticleSystem ps;
    //public List<ParticleCollisionEvent> CollisionEvents; 
    public ParticleCollisionEvent[] collisionEvents;

    public GameObject explo;   

    public float radius = 5f;       

    private LayerMask _shootableMask;

    // Use this for initialization
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();       
        collisionEvents = new ParticleCollisionEvent[8];
    }

    public void OnParticleCollision(GameObject other)
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
            Vector3 pos = collisionEvents[i].intersection;
            Collider[] colliders = Physics.OverlapSphere(pos, radius);    
                  
            Instantiate(explo, pos, Quaternion.identity);                   
        }

        Destroy(gameObject);
    }
}
