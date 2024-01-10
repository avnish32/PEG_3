using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    int _minSpawnDelay = 5;

    [SerializeField]
    int _maxSpawnDelay = 30;

    [SerializeField]
    Transform[] _spawnPts;

    [SerializeField]
    GameObject _pickup;

    private int _delayUntilNextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SetDelayUntilNextSpawn();
        StartCoroutine(SpawnAfterDelay());
    }

    private void SetDelayUntilNextSpawn()
    {
        _delayUntilNextSpawn = UnityEngine.Random.Range(_minSpawnDelay, _maxSpawnDelay);
    }

    IEnumerator SpawnAfterDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayUntilNextSpawn);

            SpawnPickup();
            SetDelayUntilNextSpawn();
        }
    }

    private void SpawnPickup()
    {
        Transform randomSpawnPt = _spawnPts[UnityEngine.Random.Range(0, _spawnPts.Length)];
        Instantiate(_pickup, randomSpawnPt.position, Quaternion.identity);
    }
}
