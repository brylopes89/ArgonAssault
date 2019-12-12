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
    public Animator waveTrigger;
    public float TimeBetweenEnemies = 2f;
    public int _enemiesInWaveLeft;

    GameManager _gameManager;
    ScreenManager _screenManager;

    int _totalEnemiesInCurrentWave;    
    int _spawnedEnemies;

    [HideInInspector] public int _currentWave;
    [HideInInspector] public int _totalWaves;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponentInParent<GameManager>();
        _screenManager = FindObjectOfType<ScreenManager>();

        _currentWave = -1; // avoid off by 1
        _totalWaves = Waves.Length -1; // adjust, because we're using 0 index        
        
        StartNextWave();
    }

    void StartNextWave()
    {        
        _currentWave++;
        _screenManager.UpdateWaveText(_currentWave + 1, _totalWaves + 1);       

        if (_currentWave > _totalWaves)
        {
            _gameManager.Victory();
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
        GameObject _enemy = Waves[_currentWave].Enemy;
        //_screenManager.UpdateWaveText(_currentWave, _totalWaves);
        waveTrigger.SetBool("IsNextWave", true);

        yield return new WaitForSeconds(3f);

        waveTrigger.SetBool("IsWaveFade", true);
        waveTrigger.SetBool("IsNextWave", false);

        yield return new WaitForSeconds(1f);

        waveTrigger.SetBool("IsWaveFade", false);

        while (_spawnedEnemies < _totalEnemiesInCurrentWave)
        {
            _spawnedEnemies++;
            _enemiesInWaveLeft++;

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            if(!_gameManager.isVictory)
                Instantiate(_enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);         

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

