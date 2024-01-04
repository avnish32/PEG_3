using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBehavior;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class BomberBehavior : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private Vector2 _lookAtDir;
    private bool _isReadyToThrow = false;
    private bool _hasThrownBomb = false;
    private bool _startedExitingArena = false;

    [SerializeField]
    GameObject _bomb;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.isGamePaused)
            return;

        if (_target == null || (_target.GetComponent<Health>() != null && !_target.GetComponent<Health>().enabled))
        {
            FindNewTarget();
        }
        //_navMeshAgent.SetDestination(_target.position);
        /*Debug.Log(gameObject.name + " path pending: "+_navMeshAgent.pathPending 
            + " remaining dist <= stopping dist: "+ (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance));*/
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (_startedExitingArena)
            {
                Destroy(gameObject);
            }
            //Debug.Log("Nav mesh remaining distance: "+_navMeshAgent.remainingDistance);
            RotateTowardsTarget(_target);
            _isReadyToThrow = true;
        }
    }

    private void FindNewTarget()
    {
        //Debug.Log("Find new target called.");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        if (targets.Length == 0)
        {
            Debug.Log("GAME OVER. Enemy won.");
        }
        else
        {
            SetTarget(targets[Random.Range(0, targets.Length)].transform);
        }

    }

    private void RotateTowardsTarget(Transform target)
    {
        _lookAtDir = target.position - transform.position;
        float lookAtAngleRadians = Mathf.Atan2(_lookAtDir.y, _lookAtDir.x);
        float lookAtAngleDegrees = (lookAtAngleRadians * Mathf.Rad2Deg) - 90f;
    }

    private void OnDeath()
    {
        this.enabled = false;
        _navMeshAgent.isStopped = true;
        BoxCollider2D bomberBoxCollider = GetComponent<BoxCollider2D>();
        if (bomberBoxCollider != null)
        {
            bomberBoxCollider.enabled = false;
        }
    }

    public void ExitArena()
    {
        _startedExitingArena = true;
        Transform[] spawnPts = GameObject.FindObjectOfType<EnemySpawner>().GetSpawnPts();
        _navMeshAgent.stoppingDistance = 0;
        SetTarget(spawnPts[Random.Range(0, spawnPts.Length)]);
    }

    public void ThrowBomb()
    {
        if (_hasThrownBomb)
        {
            return;
        }
        //Debug.Log("Bomb thrown.");
        _hasThrownBomb = true;
        Instantiate(_bomb, transform.position, Quaternion.identity);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("Target changed. remaining distance: " + _navMeshAgent.remainingDistance);
    }

    public Vector2 GetLookAtDir()
    {
        return _lookAtDir;
    }

    public bool IsReadyToThrow()
    {
        return _isReadyToThrow;
    }

    public bool HasThrownBomb()
    {
        return _hasThrownBomb;
    }

}
