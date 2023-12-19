using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float _health;

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float health)
    {
        _health = Math.Min(health, 100f);
    }

    public void ReduceHealth(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            if (gameObject.CompareTag("Player2"))
            {
                FindObjectOfType<EnemySpawner>().OnPlayer2Died();
            }
            Destroy(gameObject);
        }
    }
}
