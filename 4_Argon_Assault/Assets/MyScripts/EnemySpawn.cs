using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave1
{
    public int EnemiesPerWave;
    public GameObject[] Enemy;
}

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] Waves;
    public Transform[] spawnPoints;
    public float TimeBetweenEnemies = 2f;

    int _totalEnemiesInCurrentWave;
    int _enemiesInWaveLeft;
    int _spawnedEnemies;

    int _currentWave;
    int _totalWaves;

    private GameObject[] spawnee;
    public GameObject[] triggers;


    List<GameObject> enemyArray = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        _currentWave = -1;
        _totalWaves = Waves.Length - 1;

       // StartNextWave();

        //spawnee = GameObject.FindGameObjectsWithTag("Enemy");
        /*foreach(GameObject enemy in spawnee)
        {
            enemyArray.Add(enemy);
        }
        
        //spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);*/

    }

    /*void StartNextWave()
    {
        _currentWave++;

        if (_currentWave > _totalWaves)
        {
            return;
        }
       // _totalEnemiesInCurrentWave = Waves[_currentWave].EnemiesPerWave;
        _enemiesInWaveLeft = 0;
        _spawnedEnemies = 0;

        StartCoroutine(SpawnEnemies());
    }*/
    public void PullTrigger(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //SpawnObject();
        }        
    }

    /*IEnumerator SpawnEnemies()
    {
        //var enemyIndex = Random.Range(0, enemyArray.Count);
        // var currentEnemyCount = spawnee.Length;   
        //GameObject enemy = Waves[_currentWave].Enemy;

        while (_spawnedEnemies < _totalEnemiesInCurrentWave)
        {
            _spawnedEnemies++;
            _enemiesInWaveLeft++;

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }

        yield return null;

        /*if(enemyArray.Count < maxEnemies)
        {           
            

            if (triggers[0])
            {
                Instantiate(enemy[enemyArray], spawnPoints[0].position, spawnPoints[0].rotation);
            }
            else if (triggers[1])
            {
                Instantiate(spawnee[enemyIndex], spawnPoints[1].position, spawnPoints[1].rotation);
            }

            enemyArray.Add(enemy);
        }
               

        /*if (currentEnemyCount == maxEnemies)
        {
            CancelInvoke("SpawnObject");
        }*/
    //}

    /*public void EnemyDefeated()
    {
        _enemiesInWaveLeft--;

        if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave)
        {
            StartNextWave();
        }
    }*/

}
