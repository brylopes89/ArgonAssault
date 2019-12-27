using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;   
    [SerializeField] int scorePerHit = 10;
    [SerializeField] int health = 20;
    public float m_dropChance = 1f / 10f;

    [Space][Header("Enemy Weapons")]
    public Transform hardpoint;
    public GameObject projectile;
    public int enemyDamage = 10;

    [Space][Header("Enemy Position")]
    public float fireDist = 30f;
    public float maxDist;
    public int rotationSpeed = 5;
    public float moveSpeed = 3.0f;

    [Space]
    public GameObject itemDrop;

    ScoreBoard scoreBoard;
    EnemySpawnManager _spawnManager;
    GameObject[] waypoints;
    List<Transform> wayTrans = new List<Transform>();
    Transform target;
    Transform enemyTran;
    GameObject weapon;

    bool hasBeenHit = false;
    public bool attacking = false;

    float newSpeed;
    float timer = 0.0f;
    float currentSpeed;
    float approachSpeed;
    float initSpeed;   
    int waypointId = 0;

    // Start is called before the first frame update
    void Start()
    {        

        scoreBoard = FindObjectOfType<ScoreBoard>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        _spawnManager = FindObjectOfType<EnemySpawnManager>();        

        foreach (GameObject wayPointTarget in waypoints)
           {                
               Transform trans = wayPointTarget.transform;                
               wayTrans.Add(trans);                
           }

        target = GameObject.FindWithTag("Player").transform;        
        
        enemyTran = GetComponent<Transform>();
        initSpeed = moveSpeed;

        Patrol();
    }  

    void FixedUpdate()
    {             
        PatrolAndChase();
    }

    void Patrol()
    {
        
        // if no waypoints have been assigned
        if (waypoints.Length == 0)
        {
            //Debug.LogError("You need to assign some waypoints within the Inspector!");
            return;
        }

        // if distance to waypoint is less than 2 metres then start heading toward next waypoint
        if (Vector3.Distance(wayTrans[waypointId].position, enemyTran.position) < 10)
        {
            // increase waypoint id
            waypointId++;

            // make sure new waypointId isn't greater than number of waypoints
            // if it is then set waypointId to 0 to head towards first waypoint again
            if (waypointId >= wayTrans.Count) waypointId = 0;
        }

        // move towards the current waypointId's position
        MoveTowards(wayTrans[waypointId].position);

        attacking = false;
    }

    void PatrolAndChase()
    {
        // calculate the distance to the player
        int distanceToPlayer = (int)Vector3.Distance(target.position, enemyTran.position);

        // calculate vector direction to the player
        Vector3 directionToPlayer = enemyTran.position - target.position;

        // calculate the angle between AI forward vector and direction toward player
        // we use Mathf.Abs to store the absolute value (i.e. always positive)
        //int angle = (int)Mathf.Abs(Vector3.Angle(transform.forward, directionToPlayer));

        var approachDis = IsPlayerWithinApproachRange();
        var attackDis = IsPlayerWithinAttackRange();

        // if player is within 30m and angle is greater than 130 then begin chasing the player
        if (approachDis)// && angle > 130
        {
            //Debug.Log("Within approach distance");
            // move towards the players position
            MoveTowards (target.position);

            // attack the player ONLY if not already attacking
            if (!attacking && attackDis)
            {
                //Debug.Log("Within Attack Range");
                StartCoroutine(ApproachSpeed());

                Fire();
            }
            else if(!attacking)
            {                                          
                MoveTowards(target.position);                
            }
            
        }
        else //if(!approachDis)
        {            
            Patrol();            
            // if attacking, then toggle to stop
        }
    }

    bool IsPlayerWithinApproachRange()
    {
        var distance = (target.position - enemyTran.position).magnitude;
        return distance < maxDist;
    }
    
    bool IsPlayerWithinAttackRange()
    {
        var distance = (target.position - enemyTran.position).magnitude;
        return distance < fireDist;
    }

    void MoveTowards(Vector3 targetPosition)
    {
        var lookPos = target.position - enemyTran.position;
        var rotation = Quaternion.LookRotation(lookPos);
        //Find Distance to target        
        
        // calculate the direction to waypoint
        Vector3 direction = targetPosition - enemyTran.position;

        // rotate over time to face the target rotation - Quaternion.LookRotation(direction)
        enemyTran.rotation = Quaternion.Slerp(enemyTran.rotation, Quaternion.LookRotation(lookPos), rotationSpeed * Time.deltaTime);       

        // set the x and z axis of rotation to 0 so the agent stands upright (otherwise equals REALLY bad leaning)
       // enemyTran.eulerAngles = new Vector3(0, enemyTran.eulerAngles.y, 0);

        enemyTran.position += enemyTran.forward * moveSpeed * Time.fixedDeltaTime;       
    }

    void Fire()
    {
        weapon = Instantiate(projectile, hardpoint.transform.position, Quaternion.identity);

        weapon.transform.LookAt(target.position);

        Rigidbody weaponRigid = weapon.GetComponentInChildren<Rigidbody>();

        weaponRigid.AddForce((transform.forward) * 1000f);
    }

    IEnumerator ApproachSpeed()
    {
        // toggle attacking on

        attacking = true;
        currentSpeed = moveSpeed;
        newSpeed = 1.0f;
        approachSpeed = 4.0f;
        
        while (timer < approachSpeed)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / approachSpeed;

            if (ratio > 1.0f)//reaches %100
            {
                break;
            }

            moveSpeed = Mathf.SmoothStep(currentSpeed, newSpeed, ratio);            
        }

        yield return new WaitForSeconds(1f);

        attacking = false;

    }

    /*IEnumerator SlowSpeedLeave()
    {
        currentSpeed = moveSpeed;
        newSpeed = initSpeed;
        timer = 0.0f;
        approachSpeed = 2.0f;

        while (timer < approachSpeed)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / approachSpeed;

            if (ratio > 1.0f)//reaches %100
            {
                break;
            }
            moveSpeed = Mathf.SmoothStep(currentSpeed, newSpeed, ratio);
        }

        yield return new WaitForEndOfFrame();

        //GetComponent<PlayerFlightControl>().speed = initSpeed;
    }*/

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

        if(Random.Range(0f,1f) <= m_dropChance)
        {
            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }        
        Instantiate(deathFX, transform.position, Quaternion.identity);
        
        _spawnManager.EnemyDefeated();
        gameObject.SetActive(false);                
    }

    public int EnemyWeaponDamage()
    {
        int damageDealt = enemyDamage;       
           
        return damageDealt;
    }

}
