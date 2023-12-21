using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Animator : MonoBehaviour
{
    private enum PlayerState
    {
        IDLE, WALKING
    };
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movementInput;
    private PlayerState _currentState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetState(PlayerState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementInput != Vector2.zero)
        {
            SetState(PlayerState.WALKING);
            _animator.SetFloat("xMovement", _movementInput.x);
            _animator.SetFloat("yMovement", _movementInput .y);

            if (_movementInput.x > 0)
            {
                _spriteRenderer.flipX = true;
            } else
            {
                _spriteRenderer.flipX= false;
            }
        }
        else
        {
            SetState(PlayerState.IDLE);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void SetState(PlayerState playerState)
    {
        _currentState = playerState;

        switch (_currentState)
        {
            case PlayerState.IDLE:
                _animator.Play("Idle");
                break;
            case PlayerState.WALKING:
                _animator.Play("Walk");
                break;
            default:
                _currentState = PlayerState.IDLE;
                _animator.Play("Idle");
                break;
        }
    }
}
