using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject deathFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 10;
    [SerializeField] int health = 20;

    ScoreBoard scoreBoard;
    bool hasBeenHit = false;

    GameObject target;
    //public NavMeshAgent enemyAgent;

    float f_RotSpeed = 3.0f;
    public float f_MoveSpeed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        target = GameObject.FindWithTag("Player");
        
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        followPlayer();
    }
    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    void followPlayer()
    {
        if (target != null)
        {
            //enemyAgent.SetDestination(target.transform.position);

            // Look at Player
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(target.transform.position - transform.position), 
                f_RotSpeed * Time.deltaTime);

            //Move towards Player
            transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;
    
        }

    }

    void OnParticleCollision(GameObject other)
    {
        
        hasBeenHit = true;
        health -= GameObject.FindObjectOfType<PlayerController>().CalculateWeaponDamage();
        ProcessHit();

        if (health <= 0)
        {
            KillEnemy();
        }       
    }

    private void ProcessHit()
    {        
        scoreBoard.ScoreHit(scorePerHit);        
        print(health);
    }

    private void KillEnemy()
    {
        hasBeenHit = false;
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        Destroy(gameObject);       
        
    }
}
