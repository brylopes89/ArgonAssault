using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int EnemiesPerWave;
    public GameObject Enemy;
}

public class EnemySpawnManager : MonoBehaviour
{
    public Wave[] Waves; // class to hold information per wave    

    public Transform[] spawnPoints;
    public float TimeBetweenEnemies = 2f;

    public int _enemiesInWaveLeft;
    int _totalEnemiesInCurrentWave;    
    int _spawnedEnemies;

    int _currentWave;
    int _totalWaves;

    // Start is called before the first frame update
    void Start()
    {
        _currentWave = -1; // avoid off by 1
        _totalWaves = Waves.Length - 1; // adjust, because we're using 0 index        
        
        StartNextWave();
    }

    void StartNextWave()
    {
        _currentWave++;

        if (_currentWave > _totalWaves)
        {
            return;
        }
        _totalEnemiesInCurrentWave = Waves[_currentWave].EnemiesPerWave;
        _enemiesInWaveLeft = 0;
        _spawnedEnemies = 0;

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        //var enemyIndex = Random.Range(0, enemyArray.Count);
        // var currentEnemyCount = spawnee.Length;   
        GameObject enemy = Waves[_currentWave].Enemy;

        while (_spawnedEnemies < _totalEnemiesInCurrentWave)
        {
            _spawnedEnemies++;
            _enemiesInWaveLeft++;

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            yield return new WaitForSeconds(TimeBetweenEnemies);
        }
        yield return null;
    }

    public void EnemyDefeated() // called by an enemy when they're defeated
    {
        _enemiesInWaveLeft--;
        
        // We start the next wave once we have spawned and defeated them all
        if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave)
        {            
            StartNextWave();
        }
    }
}

