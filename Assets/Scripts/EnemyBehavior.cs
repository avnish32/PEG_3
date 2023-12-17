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
    private bool isShooting = false;

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

        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        _target = targets[Random.Range(0, targets.Length)].transform;
        _navMeshAgent.SetDestination(_target.position);

    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("remaining dist: " + navMeshAgent.remainingDistance);
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            //Debug.Log("Enemy Started shooting.");
            setRotationAndSprite();

            if (!isShooting)
            {
                InvokeRepeating("Shoot", 0f, 0.5f);
                isShooting = true;
            }
            
            //TODO place checks for when target is destroyed or P2 is nearby

        }
    }

    private void setRotationAndSprite()
    {
        Vector2 lookAtDir = _target.position - transform.position;
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
}
