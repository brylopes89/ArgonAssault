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

    Transform target;
    Transform[] children;

    List<Transform> transforms = new List<Transform>();

    //public NavMeshAgent enemyAgent;

    float f_RotSpeed = 3.0f;
    public float f_MoveSpeed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        target = GameObject.FindWithTag("Player").transform;
        children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            var tran = GetComponent<Transform>();
            transforms.Add(tran);
        }      
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
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
       // foreach (Transform tran in transforms)
       //{
           
            var lookPos = target.position - transform.position;

            var rotation = Quaternion.LookRotation(lookPos, Vector3.up);

            //Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);

            transform.LookAt(target.position);

            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, f_RotSpeed * Time.deltaTime);

            //Move towards Player
            transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;

       // }                   

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
