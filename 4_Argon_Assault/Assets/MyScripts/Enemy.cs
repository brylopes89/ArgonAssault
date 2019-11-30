using UnityEngine;
using System.Collections;


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
    bool attacking = false;

    Transform target;
    Transform enemyTran; 
    float f_RotSpeed = 3.0f;

    public int rotationSpeed = 5;
    public float moveSpeed = 3.0f;    
    public float maxDist;
    public float fireDist = 30f;
    public Transform[] waypoints;
    public float newSpeed;    

    private int waypointId = 0;
    float timer = 0.0f;
    float currentSpeed;
    float approachSpeed;
    float initSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //AddBoxCollider();        

        scoreBoard = FindObjectOfType<ScoreBoard>();
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        target = GameObject.FindWithTag("Player").transform;
        enemyTran = GetComponent<Transform>();
        initSpeed = moveSpeed;
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
        if (Vector3.Distance(waypoints[waypointId].position, enemyTran.position) < 2)
        {
            // increase waypoint id
            waypointId++;

            // make sure new waypointId isn't greater than number of waypoints
            // if it is then set waypointId to 0 to head towards first waypoint again
            if (waypointId >= waypoints.Length) waypointId = 0;
        }
     
        // move towards the current waypointId's position
        MoveTowards(waypoints[waypointId].position);
        attacking = false;
    }

    void FixedUpdate()
    {        
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }      

        //followPlayer();
        PatrolAndChase();
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
            Debug.Log("Within approach distance");
            // move towards the players position
            MoveTowards (target.position);

            // attack the player ONLY if not already attacking
            if (!attacking && attackDis)
            {                
                Debug.Log("Within Attack Range");
                StartCoroutine(Attack()); 
            }
            else if (!attacking)
            {
                // player is "out of sight"
                MoveTowards(target.position);
            }
        }
        else
        {
            //StartCoroutine(SlowSpeedLeave());
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

        print(targetPosition);
    }

    IEnumerator Attack()
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
        
        // odds of player being attacked successfully
        float odds = Random.Range(0.0f, 1.0f);

        // was the player attacked?
        if (odds >= 0.0f)
        {
            Debug.Log("player has been attacked");            
        }
        // create delay before attacking again (else would be constant every frame = dead player)
        yield return new WaitForSeconds(1);

        // allow attacking again
        attacking = false;
    }

    IEnumerator SlowSpeedLeave()
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
