using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet : MonoBehaviour {

    private ParticleSystem ps;
    //public List<ParticleCollisionEvent> CollisionEvents; 
    public ParticleCollisionEvent[] collisionEvents;

    public GameObject explo;
    public AudioClip[] _missileSFX;
    public float radius = 5f;

    private AudioSource _audioSource;
    private LayerMask _shootableMask;

    // Use this for initialization
    private void Start()
    {
        SetupSound();

        ps = GetComponent<ParticleSystem>();
        //CollisionEvents = new List<ParticleCollisionEvent>();
        collisionEvents = new ParticleCollisionEvent[8];
    }

    private void SetupSound()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.2f;
        _shootableMask = LayerMask.GetMask("Player");

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
            //Explosions(collisionEvents[i].intersection);
            Vector3 pos = collisionEvents[i].intersection;
            Collider[] colliders = Physics.OverlapSphere(pos, radius);
            //Vector3 force = collisionEvents[i].velocity * 10;
            //rb.AddForce(force);
           
            Instantiate(explo, pos, Quaternion.identity);
            Debug.Log(eventCount);           
            
            foreach (Collider hit in colliders)
            {
                _audioSource.PlayOneShot(_missileSFX[0]);               
            }
        }
    }

    /*void OnCollisionEnter(Collision col)
    {
        Physics.IgnoreLayerCollision(2, 8);

        GameObject otherObj = col.gameObject;
        Debug.Log("Collided with: " + otherObj);

        ContactPoint contact = col.contacts[0];
        Vector3 pos = contact.point;
        
        _audioSource.PlayOneShot(_missileSFX[0]);
        explo = Instantiate(explo, transform.position, Quaternion.identity) as GameObject;     
    }*/

}
