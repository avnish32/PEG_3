using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Health : Health
{
    [SerializeField]
    [Tooltip("Amount of health to refill per second")]
    private float _healthRefillRate = 10f;

    [SerializeField]
    [Tooltip("Amount of health to deplete per second")]
    private float _healthDepletionRate = 5f;

    [SerializeField]
    [Tooltip("This vlaue should be the same as P1's circle collider")]
    private float _healthRefillRadius = 2f;

    private bool _isInHealthRefillRange = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        float distanceFromP1 = Vector3.Distance(transform.position, player1.transform.position);
        if (distanceFromP1 <= _healthRefillRadius)
        {
            _isInHealthRefillRange = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInHealthRefillRange)
        {
            SetHealth(GetHealth() + (_healthRefillRate * Time.deltaTime));
        } else
        {
            SetHealth(GetHealth() - (_healthDepletionRate * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("P2 health triggered.");
        if (collision != null && collision.gameObject.CompareTag("Player1"))
        {
            Debug.Log("This gameobject is now in P1's replenish range.");
            _isInHealthRefillRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player1"))
        {
            Debug.Log("This gameobject is not in P1's replenish range anymore.");
            _isInHealthRefillRange = false;
        }
    }


}
