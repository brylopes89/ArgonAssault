using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip pickUpSound;
    public ParticleSystem pickUpParticles;

    private void OnTriggerEnter(Collider go) 
    {
        if (go.gameObject.tag == "Player")
        {
            //audioSource = GetComponent<AudioSource>();            

            audioSource.PlayOneShot(pickUpSound);

            pickUpParticles.Play();

            PlayerInventory.keyCount++;

            print("I have " + PlayerInventory.keyCount + " keys!");

            Destroy(gameObject);
        } 
    }


}
