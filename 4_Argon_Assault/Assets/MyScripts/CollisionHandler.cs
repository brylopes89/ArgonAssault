using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("In seconds")][SerializeField] float levelLoadDelay = 2f;
    [Tooltip("FX pefab on player")][SerializeField] GameObject deathFX;    

    void OnCollisionEnter(Collision other)
    {       
        StartDeathSequence();
        deathFX.SetActive(true);
        Invoke("ReloadScene", levelLoadDelay);           
    }

    void StartDeathSequence()
    {
        print("Wipe yourself off, you dead.");
        SendMessage("OnPlayerDeath");        
    }  

    void ReloadScene() // string referenced
    {
        SceneManager.LoadScene(1);
    }
}
