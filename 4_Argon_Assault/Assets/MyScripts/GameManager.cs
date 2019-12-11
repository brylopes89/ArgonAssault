using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator GameOverAnimator;

    private GameObject _player;
    private GameObject _shootController;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _shootController = GameObject.FindGameObjectWithTag("ShootManager");
    }

    public void GameOver()
    {
        GameOverAnimator.SetBool("IsGameOver", true);
       
        _player.GetComponent<PlayerFlightControl>().defeated = true;
        _player.GetComponent<PlayerFlightControl>().thrust_exists = false;
        _player.GetComponent<PlayerFlightControl>().afterburner_Active = false;
        _player.GetComponent<AudioSource>().enabled = false;           
        _player.GetComponentInChildren<CollisionHandler>().enabled = false;
        _player.GetComponentInChildren<ParticleEmission>().CurveIncrease(false);

        _player.GetComponentInChildren<Rigidbody>().mass = 100;
        //_player.GetComponentInChildren<Rigidbody>().isKinematic = true;
        _player.GetComponentInChildren<Rigidbody>().useGravity = true;

        _shootController.GetComponent<PlayerShootControl>().enabled = false;        
        //Cursor.lockState = CursorLockMode.None;
    }
}
