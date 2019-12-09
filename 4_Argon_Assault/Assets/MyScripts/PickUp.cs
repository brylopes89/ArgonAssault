using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private AudioSource audioSource;    
    public ParticleSystem pickUpParticles;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }
    private void OnTriggerEnter(Collider go) 
    {
        if (go.gameObject.tag == "Player")
        {

            
            audioSource.PlayOneShot(audioSource.clip);

            pickUpParticles.Play();

            PlayerInventory.keyCount++;

            print("I have " + PlayerInventory.keyCount + " keys!");

            Destroy(gameObject, .1f);
        } 
    }


}
