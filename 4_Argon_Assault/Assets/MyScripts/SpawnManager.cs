using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int EnemiesPerWave;
    public GameObject Enemy;
}

public class SpawnManager : MonoBehaviour
{
    public Wave[] Waves;
    public Transform[] spawnPoints;
    public float TimeBetweenEnemies = 2f;

    int _totalEnemiesInCurrentWave;
    int _enemiesInWaveLeft;
    int _spawnedEnemies;

    int _currentWave;
    int _totalWaves;

    private GameObject[] spawnee;
    public GameObject[] triggers;
    // Start is called before the first frame update
    void Start()
    {
        _currentWave = -1;
        _totalWaves = Waves.Length - 1;

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

            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            yield return new WaitForSeconds(TimeBetweenEnemies);
        }

        yield return null;

    }

    public void EnemyDefeated()
    {
        _enemiesInWaveLeft--;

        if(_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave)
        {
            StartNextWave();
        }
    }
}

