using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator GameOverAnimator;
    public Animator VictoryAnimator;

    private GameObject _player;
    private GameObject _shootController;
    private GameObject[] _enemy;
    private EnemySpawnManager _spawnManager;
    private List<GameObject> _enemies = new List<GameObject>();

    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isVictory = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _shootController = GameObject.FindGameObjectWithTag("ShootManager");
        
        _spawnManager = FindObjectOfType<EnemySpawnManager>();
    }

    private void Update()
    {
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void GameOver()
    {
        if (isVictory)
        {
            return;            
        }
        else
        {
            isGameOver = true;
            GameOverAnimator.SetTrigger("GameOver");
            DisableGame();
        }
    }

    public void Victory()
    {        
        isVictory = true;              

        if(isVictory)
            isGameOver = false;
            VictoryAnimator.SetTrigger("GameOver");

        DisableGame();
    }

    private void DisableGame()
    {
        if (isGameOver)
        {
            _player.GetComponent<PlayerFlightControl>().defeated = true;
            _player.GetComponent<PlayerFlightControl>().thrust_exists = false;
            _player.GetComponent<PlayerFlightControl>().afterburner_Active = false;
            _player.GetComponent<AudioSource>().enabled = false;
            _player.GetComponentInChildren<CollisionHandler>().enabled = false;
            _player.GetComponentInChildren<ParticleEmission>().CurveIncrease(false);
            _player.GetComponent<Rigidbody>().mass = 100;            
            _player.GetComponent<Rigidbody>().useGravity = true;

            _shootController.GetComponent<PlayerShootControl>().enabled = false;
        }

        else
        {
            for(int i = 0; i < _enemy.Length; i++)
            {
                _player.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(_enemy[i].gameObject);                      
            }
        }         
              
        //Cursor.lockState = CursorLockMode.None;
    }
}
