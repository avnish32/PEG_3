using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private TowerInfo[] _towersInfo;

    [SerializeField]
    private InputActionReference _leftMouseButton;

    [SerializeField]
    private AudioClip _teleportSound;

    [SerializeField]
    private AudioPlayer _audioPlayer;

    private Dictionary<Towers, Vector3> _towerToLocationMap;
    private Towers _currentTower;
    private Tower _towerUnderCursor;
    private Tower _towerOnLMBDown;
    private float _timeOnLastLMBPressOnTower;

    private void Awake()
    {
        if (_audioPlayer == null)
        {
            _audioPlayer = GameObject.FindFirstObjectByType<AudioPlayer>();
        }

        _towerToLocationMap = new Dictionary<Towers, Vector3>();
        foreach (TowerInfo towerInfo in _towersInfo)
        {
            _towerToLocationMap.Add(towerInfo.tower, towerInfo.towerPosition);
        }
        _currentTower = _towerToLocationMap.ElementAt(_towerToLocationMap.Count-1).Key;

    }

    private void Start()
    {
        Debug.Log("Teleporter start.");
        TeleportToTower(_towerToLocationMap.ElementAt(0).Key);
    }

    private void Update()
    {
        if (LevelController.isGamePaused)
        {
            return;
        }

        CheckForCursorOnTower();
    }

    private void CheckForCursorOnTower()
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f, LayerMask.GetMask("EnemyTarget"));

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
        if (!this.enabled)
        {
            Debug.Log("Teleporter: Not enabled.");
            return;
        } 

        if  (_currentTower == destinationTower)
        {
            Debug.Log("Teleporter: current tower is same as destination.");
            return;
        } 

        Vector3 destinationPosition = _towerToLocationMap[destinationTower];
        transform.position = destinationPosition;
        _currentTower = destinationTower;

        _audioPlayer.PlaySFX(_teleportSound);
        //_audioSource.PlayOneShot(_teleportSound);

        foreach (var bomb in FindObjectsOfType<Bomb>())
        {
            bomb.BroadcastMessage("OnPlayerTeleport", destinationTower);
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
    }

    public void TeleportToTower1()
    {
        TeleportToTower(Towers.TOWER_RED);
    }

    public void TeleportToTower2()
    {
        TeleportToTower(Towers.TOWER_BLUE);
    }

    public void TeleportToTower3()
    {
        TeleportToTower(Towers.TOWER_YELLOW);
    }

    public Towers GetCurrentTower()
    {
        return _currentTower;
    }
}

public enum Towers
{
    TOWER_RED, TOWER_BLUE, TOWER_YELLOW, TOWER_GREEN
};

[Serializable]
struct TowerInfo
{
    public Towers tower;
    public Vector3 towerPosition;
}
