using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public ParticleSystem pickUpParticles;

    private GameManager _gameManager;
    private AudioSource audioSource;        
    private ScreenManager _screenManager;    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _screenManager = FindObjectOfType<ScreenManager>();
        _gameManager = FindObjectOfType<GameManager>();        
        
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

                if(PlayerInventory.keyCount == _screenManager.amountLeft.keysNeeded)
                {
                    _gameManager.Victory();
                }
            }                    

            Destroy(gameObject, .1f);
        } 
    }


}
