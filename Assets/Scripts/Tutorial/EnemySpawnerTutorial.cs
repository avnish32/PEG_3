using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerTutorial : EnemySpawner
{
    [SerializeField]
    Transform[] _shooterSpawnPts;

    [SerializeField]
    Transform[] _bomberSpawnPts;
    /*new public void Start()
    {
        //Debug.Log("espawner tutorial start");
        base.Start();
    }*/

    public void SpawnShooter()
    {
        Transform randomSpawnPoint = _shooterSpawnPts[UnityEngine.Random.Range(0, _shooterSpawnPts.Length)];
        Transform target = _allTargets.ElementAt(UnityEngine.Random.Range(0, _allTargets.Count)).transform;
        GameObject spawnedEnemy = Instantiate(_enemyToSpawn, randomSpawnPoint.position, Quaternion.identity);
        spawnedEnemy.GetComponent<EnemyBehavior>().SetTarget(target);
    }
    public void SpawnBomber()
    {
        Transform randomSpawnPoint = _bomberSpawnPts[UnityEngine.Random.Range(0, _bomberSpawnPts.Length)];
        Transform target = _enemyTargets.ElementAt(UnityEngine.Random.Range(0, _enemyTargets.Count)).transform;
        GameObject spawnedBomber = Instantiate(_bomberToSpawn, randomSpawnPoint.position, Quaternion.identity);
        spawnedBomber.GetComponent<BomberBehavior>().SetTarget(target);
        spawnedBomber.GetComponent<Health>().SetMaxHealth(100);
        spawnedBomber.GetComponent<Health>().SetCurrentHealth(100);
    }
}
