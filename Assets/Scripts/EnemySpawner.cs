using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPts;

    [SerializeField]
    int _bomberSpawnPercent = 15;

    [SerializeField]
    private float _minSpawnDelay;

    [SerializeField]
    private float _maxSpawnDelay;

    [SerializeField]
    private float _timeFromMaxToMin;

    [SerializeField]
    private GameObject _enemyToSpawn;

    [SerializeField]
    private GameObject _bomberToSpawn;

    private float _timeElapsedSinceStart = 0f;
    private float _timeElapsedSinceLastSpawn = 0f;
    private float _currentSpawnDelay;
    private float _nextEnemySpawnDelay;

    private List<GameObject> _allTargets;
    private List<GameObject> _enemyTargets;

    GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag("EnemyTarget");
        _allTargets = targetArray.ToList();
        _enemyTargets = targetArray.ToList();
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player2 != null )
        {
            _allTargets.Add(player2);
        }
        
        _nextEnemySpawnDelay = _maxSpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        LerpSpawnDelay();

        _timeElapsedSinceLastSpawn += Time.deltaTime;

        if (_timeElapsedSinceLastSpawn >= _nextEnemySpawnDelay)
        {
            _nextEnemySpawnDelay = SpawnEnemy();
            _timeElapsedSinceLastSpawn = 0f;
        }
    }

    protected float SpawnEnemy()
    {
        Transform randomSpawnPoint = _spawnPts[UnityEngine.Random.Range(0, _spawnPts.Length)];
        //Choose whether to spawn bomber or shooter enemy
        if (UnityEngine.Random.Range(1, 101) <= _bomberSpawnPercent)
        {
            Transform target = _enemyTargets.ElementAt(UnityEngine.Random.Range(0, _enemyTargets.Count)).transform;
            GameObject spawnedBomber = Instantiate(_bomberToSpawn, randomSpawnPoint.position, Quaternion.identity);
            spawnedBomber.GetComponent<BomberBehavior>().SetTarget(target);
        }
        else
        {
            Transform target = _allTargets.ElementAt(UnityEngine.Random.Range(0, _allTargets.Count)).transform;
            GameObject spawnedEnemy = Instantiate(_enemyToSpawn, randomSpawnPoint.position, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyBehavior>().SetTarget(target);
        }

        return UnityEngine.Random.Range(_minSpawnDelay, _currentSpawnDelay);
    }

    private void LerpSpawnDelay()
    {
        _timeElapsedSinceStart += Time.deltaTime;
        _currentSpawnDelay = Mathf.Lerp(_maxSpawnDelay, _minSpawnDelay+2, (_timeElapsedSinceStart / _timeFromMaxToMin));
    }

    public void OnEnemyTargetDestroyed(GameObject destroyedTarget)
    {
        _allTargets.Remove(destroyedTarget);
        _enemyTargets.Remove(destroyedTarget);
    }

    public Transform[] GetSpawnPts()
    {
        return _spawnPts;
    }
}
