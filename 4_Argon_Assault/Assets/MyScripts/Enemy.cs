using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Approach, Attack };
    private EnemyState _state;

    [SerializeField] GameObject deathFX;
    [SerializeField] Transform parent;

    [SerializeField] int scorePerHit = 10;
    [SerializeField] int health = 20;

    ScoreBoard scoreBoard;
    bool hasBeenHit = false;

    Vector3 storeTarget;
    Vector3 newTargetPos;

    bool savePos;
    bool overrideTarget;

    Transform target;
    Transform enemyTran;
   // Transform[] children;
    Transform obstacle;

    float f_RotSpeed = 3.0f;
    public float moveSpeed = 3.0f;
    public float minDist;
    public float maxDist;
    public float fireDist;

    List<Transform> transforms = new List<Transform>();
    public List<Vector3> EscapeDirections = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        target = GameObject.FindWithTag("Player").transform;
        enemyTran = this.transform;
        //children = GetComponentsInChildren<Transform>();

        /*foreach (Transform child in children)
        {
            var tran = child.GetComponent<Transform>();
            transforms.Add(tran);
        } */     
    }

    void Update()
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

       // ObstacleAvoidance(enemyTran.forward, 0);                         
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

    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }
}
