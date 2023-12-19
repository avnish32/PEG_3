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

    private void FixedUpdate()
    {
        _rb.velocity = _movementInput * _movementSpeed;
        /*float xMovement = _movementInput.x * _movementSpeed * Time.deltaTime;
        float yMovement = _movementInput.y * _movementSpeed * Time.deltaTime;
        transform.Translate(xMovement, yMovement, 0);*/
    }

    void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
        //Debug.Log("Input value from system: " + _movementInput);
    }
}
