using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float _health;

    public float getHealth()
    {
        return _health;
    }

    public void setHealth(float health)
    {
        _health = health;
    }

    public void reduceHealth(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
