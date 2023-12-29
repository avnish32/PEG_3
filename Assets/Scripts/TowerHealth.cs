using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Health
{
    [SerializeField]
    private Sprite _damagedTower;

    [SerializeField]
    private Sprite _healthyTower;

    public override void ReduceHealth(float damage)
    {
        base.ReduceHealth(damage);

        if (_currentHealth / _maxHealth < 0.5f)
        {
            _spriteRenderer.sprite = _damagedTower;
        }
    }

    public override void SetHealth(float health)
    {
        base.SetHealth(health);

        if (_currentHealth / _maxHealth >= 0.5f)
        {
            _spriteRenderer.sprite = _healthyTower;
        }
    }
}
