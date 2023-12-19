using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private List<GameObject> _enemyTargets;
    GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag("EnemyTarget");
        _enemyTargets = targetArray.ToList<GameObject>();
        player2 = GameObject.FindGameObjectWithTag("Player2");
        _enemyTargets.Add(player2);
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
        Transform randomSpawnPoint = _spawnPts[UnityEngine.Random.Range(0, _spawnPts.Length)];
        Transform target = _enemyTargets.ElementAt(UnityEngine.Random.Range(0, _enemyTargets.Count)).transform;

        GameObject spawnedEnemy = Instantiate(_enemyToSpawn, randomSpawnPoint.position, Quaternion.identity);
        spawnedEnemy.GetComponent<EnemyBehavior>().SetTarget(target);
        //spawnedEnemy.GetComponent<NavMeshAgent>().SetDestination(target.position);
    }

    private void LerpSpawnDelay()
    {
        _timeElapsedSinceStart += Time.deltaTime;
        _currentSpawnDelay = Mathf.Lerp(_maxSpawnDelay, _minSpawnDelay, (_timeElapsedSinceStart / _timeFromMaxToMin));
    }

    public void OnPlayer2Died()
    {
        _enemyTargets.Remove(player2);
    }
}
