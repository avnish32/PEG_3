using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private bool _isShooting = false;
    private bool _isPlayerWithinShootingRange = false;
    private Transform _player2;

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform _bulletSpawnPt;

    [SerializeField]
    private Sprite down;
    [SerializeField]
    private Sprite up;
    [SerializeField]
    private Sprite left;
    [SerializeField]
    private Sprite right;

    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        /*GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        _target = targets[Random.Range(0, targets.Length)].transform;*/
        //_navMeshAgent.SetDestination(_target.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            FindNewTarget();
        }
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("remaining dist: " + navMeshAgent.remainingDistance);
        if (_isPlayerWithinShootingRange)
        {
            RotateTowardsTarget(_player2);
            TriggerShooting();
        } else if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            //Debug.Log("Enemy Started shooting.");
            RotateTowardsTarget(_target);
            TriggerShooting();
        } else
        {
            RotateTowardsTarget(_target);
        }
    }

    private void TriggerShooting()
    {
        if (!_isShooting)
        {
            InvokeRepeating("Shoot", 0f, 0.5f);
            _isShooting = true;
        }
    }

    private void FindNewTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        if (targets.Length ==0)
        {
            Debug.Log("GAME OVER. Enemy won.");
        } else
        {
            SetTarget(targets[Random.Range(0, targets.Length)].transform);
        }
        
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector2 lookAtDir = target.position - transform.position;
        float lookAtAngleRadians = Mathf.Atan2(lookAtDir.y, lookAtDir.x);
        float lookAtAngleDegrees = (lookAtAngleRadians * Mathf.Rad2Deg) - 90f;
        _rb.rotation = lookAtAngleDegrees;

        
        //TODO make all sprites face down like player. Until then, commented this block.
        /*if (lookAtAngleDegrees < -45f || lookAtAngleDegrees >= 225f)
        {
            _spriteRenderer.sprite = right;
        }
        else if (lookAtAngleDegrees >= -45f && lookAtAngleDegrees < 45f)
        {
            _spriteRenderer.sprite = up;
        }
        else if (lookAtAngleDegrees >= 45f && lookAtAngleDegrees < 135f)
        {
            _spriteRenderer.sprite = left;
        }
        else
        {
            _spriteRenderer.sprite = down;
        }*/
    }

    private void Shoot()
    {
        Instantiate(_bullet, _bulletSpawnPt.position, _bulletSpawnPt.rotation);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        GameObject otherObject = collision.gameObject;
        if (otherObject != null && otherObject.CompareTag("Player2") &&
            _target != null && otherObject != _target.gameObject)
        {
            Debug.Log("OnTriggerEnter2D fired.");
            _isPlayerWithinShootingRange = true;
            if (_player2 == null)
            {
                _player2 = otherObject.transform;
            }
            /*RotateTowardsTarget(otherObject.transform);
            TriggerShooting();*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        GameObject otherObject = collision.gameObject;
        if (otherObject != null && otherObject.CompareTag("Player2") && otherObject != _target.gameObject)
        {
            Debug.Log("OnTriggerExit2D fired.");
            _isPlayerWithinShootingRange = false;
            /*if (_target != null)
            {
                RotateTowardsTarget(_target);
                TriggerShooting();
            } else
            {
                FindNewTarget();
            }*/
            
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _navMeshAgent.SetDestination(_target.position);
    }
}
