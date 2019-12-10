using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private AudioSource audioSource;    
    public ParticleSystem pickUpParticles;
    private ScreenManager _screenManager;    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _screenManager = GameObject.FindWithTag("ScreenManager").GetComponent<ScreenManager>();
        //amountLeft = FindObjectOfType<Interact>();
        
    }
    private void OnTriggerEnter(Collider go) 
    {
        if (go.gameObject.tag == "Player")
        {

            
            audioSource.PlayOneShot(audioSource.clip);

            pickUpParticles.Play();

            PlayerInventory.keyCount++;
            if(PlayerInventory.keyCount <= _screenManager.amountLeft.keysNeeded)
            {
                _screenManager.UpdateItemText(PlayerInventory.keyCount, _screenManager.amountLeft.keysNeeded);
            }            

            print("I have " + PlayerInventory.keyCount + " keys!");

            Destroy(gameObject, .1f);
        } 
    }


}
