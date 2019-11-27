using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Approach, Attack };
    private EnemyState _state;

    [SerializeField] GameObject deathFX;    

    [SerializeField] int scorePerHit = 10;
    [SerializeField] int health = 20;   

    ScoreBoard scoreBoard;
    SpawnManager _spawnManager; 

    bool hasBeenHit = false;

    Transform target;
    Transform enemyTran; 

    float f_RotSpeed = 3.0f;
    public float moveSpeed = 3.0f;
    public float minDist;
    public float maxDist;
    public float fireDist;     

    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();        

        scoreBoard = FindObjectOfType<ScoreBoard>();
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        target = GameObject.FindWithTag("Player").transform;
        enemyTran = this.transform;                 
    }

    void FixedUpdate()
    {        
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }      

        followPlayer();
    }

    void followPlayer()
    {       
        var lookPos = target.position - enemyTran.position;
        var rotation = Quaternion.LookRotation(lookPos);

        //Find Distance to target
        var distance = lookPos.magnitude;           

        var approachDis = IsPlayerWithinApproachRange();

        if (approachDis)
        {
            _state = EnemyState.Approach;

            //Rotate Towards Player
            enemyTran.rotation = Quaternion.Slerp(enemyTran.rotation, rotation, f_RotSpeed * Time.deltaTime);

            //Move towards Player
            enemyTran.position += enemyTran.forward * moveSpeed * Time.deltaTime;
        }                         
    }

    bool IsPlayerWithinApproachRange()
    {
        var distance = (target.position - enemyTran.position).magnitude;
        return distance < maxDist;
    }
    
    bool IsPlayerWithinAttackRange()
    {
        var distance = (target.position - transform.position).magnitude;
        return distance < fireDist;
    }

    void OnParticleCollision(GameObject other)
    {        
        hasBeenHit = true;        

        health -= GameObject.FindObjectOfType<PlayerShootControl>().CalculateWeaponDamage();                
        scoreBoard.ScoreHit(scorePerHit);                               

        if (health <= 0)
        {
            KillEnemy();
        }       
    }    

    private void KillEnemy()
    {
        hasBeenHit = false;
        //GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        //fx.transform.parent = parent;
        _spawnManager.EnemyDefeated();
        this.gameObject.SetActive(false);                
    }

    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }
    
}
