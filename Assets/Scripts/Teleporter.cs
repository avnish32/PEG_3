using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private TowerInfo[] _towersInfo;

    [SerializeField]
    private InputActionReference _leftMouseButton;

    private Dictionary<Towers, Vector3> _towerToLocationMap;
    private Towers _currentTower;
    private Tower _towerUnderCursor;
    private Tower _towerOnLMBDown;
    float _timeOnLastLMBPressOnTower;

    private void Awake()
    {
        _towerToLocationMap = new Dictionary<Towers, Vector3>();
        foreach (TowerInfo towerInfo in _towersInfo)
        {
            _towerToLocationMap.Add(towerInfo.tower, towerInfo.towerPosition);
        }
        _currentTower = Towers.TOWER_3;

    }

    private void Start()
    {
        //Debug.Log("Teleporter start.");
        TeleportToTower(Towers.TOWER_1);
    }

    private void Update()
    {
        CheckForCursorOnTower();
    }

    private void CheckForCursorOnTower()
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, LayerMask.GetMask("EnemyTarget"));

        if (!hit || hit.transform.gameObject.GetComponent<Tower>() == null)
        {
            if (_towerUnderCursor != null)
            {
                _towerUnderCursor.GetComponent<SpriteRenderer>().size = new Vector2(2.5f, 2.5f);
                _towerUnderCursor = null;
            }
            return;
        }

        _towerUnderCursor = hit.transform.gameObject.GetComponent<Tower>();
        if (_towerUnderCursor.GetThisTower() == _currentTower) 
        {
            return;
        }

        //Debug.Log("Clicked on " + hit.collider);
            
        _towerUnderCursor.GetComponent<SpriteRenderer>().size = new Vector2(3, 3);

        if (_leftMouseButton.action.WasPressedThisFrame())
        {
            _timeOnLastLMBPressOnTower = Time.time;
            _towerOnLMBDown = _towerUnderCursor;
            //Debug.Log("Clicked on " + hit.collider + " | Time: " + _timeOnLastLMBPressOnTower);
            return;
        }

        if (_leftMouseButton.action.WasReleasedThisFrame())
        {
            if (_towerOnLMBDown != null && _towerOnLMBDown == _towerUnderCursor && Time.time - _timeOnLastLMBPressOnTower <= 1.0f)
            {
                //Debug.Log("Teleporting.");
                TeleportToTower(_towerOnLMBDown.GetThisTower());
            }
            return;
        }
    }


    private void TeleportToTower(Towers destinationTower)
    {
        if (!this.enabled || _currentTower == destinationTower || LevelController.isGamePaused)
        {
            Debug.Log("Teleporter: Not enabled or current tower is same as destination or game is paused.");
            return;
        }

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
    TOWER_1, TOWER_2, TOWER_3, TOWER_4
};

[Serializable]
struct TowerInfo
{
    public Towers tower;
    public Vector3 towerPosition;
}
