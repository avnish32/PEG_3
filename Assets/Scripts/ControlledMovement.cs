using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlledMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _movementInput;

    [SerializeField]
    private float _movementSpeed = 10f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (horizontalAxisBinding.)
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movementInput * _movementSpeed;
    }

    void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
        //Debug.Log("Input value from system: " + _movementInput);
    }
}
