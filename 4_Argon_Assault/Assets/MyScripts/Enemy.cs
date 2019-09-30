using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject deathFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 10;
    [SerializeField] int health = 20;

    ScoreBoard scoreBoard;
    bool hasBeenHit = false;

    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
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
