using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class EnemyAnimator : MonoBehaviour
{
    private enum EnemyState
    {
        IDLE, WALKING
    };

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EnemyState _currentState;
    private EnemyBehavior _enemyBehavior;
    private NavMeshAgent _navMeshAgent;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyBehavior = GetComponent<EnemyBehavior>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(EnemyState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 navMeshVelocity = _navMeshAgent.velocity / _navMeshAgent.speed;
        //Debug.Log("Enemy navmesh velocity: " + navMeshVelocity);

        float xMovement, yMovement;
        if (Mathf.Abs(navMeshVelocity.x) > 0.1f || Mathf.Abs(navMeshVelocity.y) > 0.1f)
        {
            SetState(EnemyState.WALKING);
            xMovement = navMeshVelocity.x;
            yMovement = navMeshVelocity.y;
        }
        else
        {
            SetState(EnemyState.IDLE);
            Vector2 lookAtdir = _enemyBehavior.GetLookAtDir();
            xMovement = lookAtdir.x;
            yMovement = lookAtdir.y;
        }

        _animator.SetFloat("xMovement", xMovement);
        _animator.SetFloat("yMovement", yMovement);

        if (xMovement > 0)
        {
            //facing to the right
            _spriteRenderer.flipX = true;
            _enemyBehavior.SetBulletSpawnPt(EnemyBehavior.BulletSpawnPt.RIGHT);
        }
        else
        {
            //facing to the left
            _spriteRenderer.flipX = false;
            _enemyBehavior.SetBulletSpawnPt(EnemyBehavior.BulletSpawnPt.LEFT);
        }

        if (yMovement > 0.95f)
        {
            _enemyBehavior.SetBulletSpawnPt(EnemyBehavior.BulletSpawnPt.UP);
        } else if (yMovement < -0.95f)
        {
            _enemyBehavior.SetBulletSpawnPt(EnemyBehavior.BulletSpawnPt.DOWN);
        }
    }

    private void SetState(EnemyState enemyState)
    {
        _currentState = enemyState;

        switch (_currentState)
        {
            case EnemyState.IDLE:
                _animator.Play("Idle");
                break;
            case EnemyState.WALKING:
                _animator.Play("Walk");
                break;
            default:
                _currentState = EnemyState.IDLE;
                _animator.Play("Idle");
                break;
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
    }
}
