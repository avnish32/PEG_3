using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private TowerInfo[] _towersInfo;

    private Dictionary<Towers, Vector3> _towerToLocationMap;
    private Towers _currentTower;

    private void Awake()
    {
        _towerToLocationMap = new Dictionary<Towers, Vector3>();
        foreach (TowerInfo towerInfo in _towersInfo)
        {
            _towerToLocationMap.Add(towerInfo.tower, towerInfo.towerPosition);
        }

    }

    private void Start()
    {   
        _teleportToTower(Towers.TOWER_2);
    }
    private void _teleportToTower(Towers destinationTower)
    {
        if (_currentTower == destinationTower)
            return;

        Vector3 destinationPosition = _towerToLocationMap[destinationTower];
        transform.position = destinationPosition;
        _currentTower = destinationTower;
    }

    public void teleportToTower1()
    {
        _teleportToTower(Towers.TOWER_1);
    }

    public void teleportToTower2()
    {
        _teleportToTower(Towers.TOWER_2);
    }

    public void teleportToTower3()
    {
        _teleportToTower(Towers.TOWER_3);
    }
}

enum Towers
{
    TOWER_1, TOWER_2, TOWER_3
};

[Serializable]
struct TowerInfo
{
    public Towers tower;
    public Vector3 towerPosition;
}
