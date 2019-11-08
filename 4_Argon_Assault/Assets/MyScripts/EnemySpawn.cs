using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject[] spawnee;

    private float spawnTime;

    public float spawnDelay;    
    public float minSpawnTime, maxSpawnTime;     
    public float maxEnemies = 20;

    public bool stopSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnee = GameObject.FindGameObjectsWithTag("Enemy");
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);       
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        int currentNumEnemies = spawnee.Length;     
                               
        for (int i = 0; i < maxEnemies; i++)
        {
            foreach (GameObject enemy in spawnee)
            {
                Instantiate(enemy, transform.position, transform.rotation);                           
            }               
        }                     
       
        if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }
    }

}
