using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BomberAnimator : MonoBehaviour
{
    private enum BomberState
    {
        THROWING, WALKING
    };

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BomberState _currentState;
    private BomberBehavior _bomberBehavior;
    private NavMeshAgent _navMeshAgent;

    private float _xMovement, _yMovement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bomberBehavior = GetComponent<BomberBehavior>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(BomberState.WALKING);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 navMeshVelocity = _navMeshAgent.velocity / _navMeshAgent.speed;
        //Debug.Log("Enemy navmesh velocity: " + navMeshVelocity);

        if (Mathf.Abs(navMeshVelocity.x) > 0.1f || Mathf.Abs(navMeshVelocity.y) > 0.1f)
        {
            SetState(BomberState.WALKING);
            _xMovement = navMeshVelocity.x;
            _yMovement = navMeshVelocity.y;
        }
        else if (!_bomberBehavior.HasThrownBomb() && _bomberBehavior.IsReadyToThrow() && _currentState != BomberState.THROWING)
        {
            SetState(BomberState.THROWING);
            Vector2 lookAtdir = _bomberBehavior.GetLookAtDir();
            _xMovement = lookAtdir.x;
            _yMovement = lookAtdir.y;
        }

        _animator.SetFloat("xMovement", _xMovement);
        _animator.SetFloat("yMovement", _yMovement);

        if (_xMovement > 0)
        {
            //facing to the right
            _spriteRenderer.flipX = true;
        }
        else
        {
            //facing to the left
            _spriteRenderer.flipX = false;
        }
    }

    private void SetState(BomberState bomberState)
    {
        _currentState = bomberState;

        switch (_currentState)
        {
            case BomberState.THROWING:
                _animator.Play("Throwing");
                break;
            case BomberState.WALKING:
                _animator.Play("Walking");
                break;
            default:
                _currentState = BomberState.WALKING;
                _animator.Play("Walking");
                break;
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
    }
}
