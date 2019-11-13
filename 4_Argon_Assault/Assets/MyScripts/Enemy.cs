﻿using System.Collections.Generic;
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

    public AudioClip HitSfxClip;
    public float HitSoundDelay = 0.5f;

    private AudioSource _audioSource;
    private float _hitTime;

    ScoreBoard scoreBoard;
    SpawnManager _spawnManager;

    Vector3 storeTarget;
    Vector3 newTargetPos;    

    bool savePos;
    bool overrideTarget;
    bool hasBeenHit = false;

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
        //AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();

        _hitTime = 0f;
        SetupSound();

        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        target = GameObject.FindWithTag("Player").transform;
        enemyTran = this.transform;      
    }

    void Update()
    {
        _hitTime += Time.deltaTime;

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
        health -= GameObject.FindObjectOfType<PlayerShootControl>().CalculateWeaponDamage();
        //Debug.Log("Particle Collision");
        scoreBoard.ScoreHit(scorePerHit);

        if(_hitTime > HitSoundDelay)
        {
            PlayRandomHit();
        }              

        if (health <= 0)
        {
            KillEnemy();
        }       
    }

    public void PlayRandomHit()
    {
        //int index = Random.Range(0, HitSfxClips.Length);
        _audioSource.clip = HitSfxClip;
        _audioSource.Play();
    }

    private void KillEnemy()
    {
        hasBeenHit = false;
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        _spawnManager.EnemyDefeated();
        this.gameObject.SetActive(false);        
        
    }

    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.2f;
    }
}
