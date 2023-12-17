using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPts;

    [SerializeField]
    private float _minSpawnDelay;

    [SerializeField]
    private float _maxSpawnDelay;

    [SerializeField]
    private float _timeFromMaxToMin;

    [SerializeField]
    private GameObject _enemyToSpawn;

    private float _timeElapsedSinceStart = 0f;
    private float _timeElapsedSinceLastSpawn = 0f;
    private float _currentSpawnDelay;

    private GameObject[] _enemyTargets;

    // Start is called before the first frame update
    void Start()
    {
        _enemyTargets = GameObject.FindGameObjectsWithTag("EnemyTarget");
    }

    // Update is called once per frame
    void Update()
    {
        LerpSpawnDelay();

        _timeElapsedSinceLastSpawn += Time.deltaTime;

        if (_timeElapsedSinceLastSpawn >= _currentSpawnDelay)
        {
            SpawnEnemy();
            _timeElapsedSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Transform randomSpawnPoint = _spawnPts[Random.Range(0, _spawnPts.Length)];
        Transform target = _enemyTargets[Random.Range(0, _enemyTargets.Length)].transform;

        GameObject spawnedEnemy = Instantiate(_enemyToSpawn, randomSpawnPoint.position, Quaternion.identity);
        spawnedEnemy.GetComponent<NavMeshAgent>().SetDestination(target.position);
    }

    private void LerpSpawnDelay()
    {
        _timeElapsedSinceStart += Time.deltaTime;
        _currentSpawnDelay = Mathf.Lerp(_maxSpawnDelay, _minSpawnDelay, (_timeElapsedSinceStart / _timeFromMaxToMin));
    }
}
