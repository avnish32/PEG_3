using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public enum BulletSpawnPt
    {
        UP, DOWN, LEFT, RIGHT
    };

    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private bool _isShooting = false;
    private bool _isPlayerWithinShootingRange = false;
    private Transform _player2;

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform _bulletSpawnPtLeft;
    [SerializeField]
    private Transform _bulletSpawnPtRight;
    [SerializeField]
    private Transform _bulletSpawnPtUp;
    [SerializeField]
    private Transform _bulletSpawnPtDown;
    [SerializeField]
    private Transform _currentBulletSpawnPt;

    private Vector2 _lookAtDir;

    // Start is called before the first frame update
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _currentBulletSpawnPt = _bulletSpawnPtDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.isGamePaused)
            return;

        if (_target == null || !_target.GetComponent<Health>().enabled)
        {
            FindNewTarget();
        }
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("remaining dist: " + navMeshAgent.remainingDistance);
        if (_isPlayerWithinShootingRange && _player2.GetComponent<Health>().enabled)
        {
            RotateTowardsTarget(_player2);
            //Debug.Log("P1 is within shooting range.");
            TriggerShooting();
        } else if(!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Debug.Log("Nav mesh remaining distance: "+_navMeshAgent.remainingDistance);
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
            Debug.Log("Trigger shooting called.");
            InvokeRepeating("Shoot", 0f, 0.25f);
            _isShooting = true;
        }
    }

    private void FindNewTarget()
    {
        Debug.Log("Find new target called.");
        CancelInvoke("Shoot");
        _isShooting = false;

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
        _lookAtDir = target.position - transform.position;
        float lookAtAngleRadians = Mathf.Atan2(_lookAtDir.y, _lookAtDir.x);
        float lookAtAngleDegrees = (lookAtAngleRadians * Mathf.Rad2Deg) - 90f;
        _currentBulletSpawnPt.rotation = Quaternion.Euler(0f, 0f, lookAtAngleDegrees);

        
        //Sprites controlled using animation now. Still left this here for ref.
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
        Instantiate(_bullet, _currentBulletSpawnPt.position, _currentBulletSpawnPt.rotation);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;
        if (otherObject != null && otherObject.CompareTag("Player2") && _target!=null && otherObject != _target.gameObject)
        {
            Debug.Log("OnTriggerExit2D fired.");
            _isPlayerWithinShootingRange = false;            
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
        BoxCollider2D enemyBoxCollider = GetComponent<BoxCollider2D>();
        if (enemyBoxCollider != null)
        {
            enemyBoxCollider.enabled = false;
        }
        CancelInvoke("Shoot");
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _navMeshAgent.SetDestination(_target.position);
        Debug.Log("Target changed. remaining distance: " + _navMeshAgent.remainingDistance);
    }

    public void SetBulletSpawnPt(BulletSpawnPt bulletSpawnPt)
    {
        switch(bulletSpawnPt)
        {
            case BulletSpawnPt.UP:
                _currentBulletSpawnPt = _bulletSpawnPtUp;
                break;
            case BulletSpawnPt.RIGHT:
                _currentBulletSpawnPt = _bulletSpawnPtRight;
                break;
            case BulletSpawnPt.LEFT:
                _currentBulletSpawnPt = _bulletSpawnPtLeft;
                break;
            default:
                _currentBulletSpawnPt = _bulletSpawnPtDown;
                break;
        }
    }

    public Vector2 GetLookAtDir()
    {
        return _lookAtDir;
    }
}
