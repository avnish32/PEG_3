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
        TeleportToTower(Towers.TOWER_2);
    }
    private void TeleportToTower(Towers destinationTower)
    {
        if (!this.enabled || _currentTower == destinationTower)
            return;

        Vector3 destinationPosition = _towerToLocationMap[destinationTower];
        transform.position = destinationPosition;
        _currentTower = destinationTower;
    }

    private void OnDeath()
    {
        this.enabled = false;
    }

    public void TeleportToTower1()
    {
        TeleportToTower(Towers.TOWER_1);
    }

    public void TeleportToTower2()
    {
        TeleportToTower(Towers.TOWER_2);
    }

    public void TeleportToTower3()
    {
        TeleportToTower(Towers.TOWER_3);
    }

    public Towers GetCurrentTower()
    {
        return _currentTower;
    }
}

public enum Towers
{
    TOWER_1, TOWER_2, TOWER_3
};

[Serializable]
struct TowerInfo
{
    public Towers tower;
    public Vector3 towerPosition;
}
