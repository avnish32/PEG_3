using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float _currentHealth;

    [SerializeField]
    private Slider _healthBarSlider;

    [SerializeField]
    private Image _healthBarColor;

    [SerializeField]
    private float _maxHealth = 100f;

    [SerializeField]
    private float _deathAnimationLength = 0f;

    private void Start()
    {
        SetHealth(_maxHealth);
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    public void SetHealth(float health)
    {
        _currentHealth = Math.Min(health, _maxHealth);
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        SetHealth(_currentHealth - damage);

        if (_currentHealth == 0)
        {
            if (gameObject.CompareTag("Player2"))
            {
                FindObjectOfType<EnemySpawner>().OnEnemyTargetDestroyed(gameObject);
            }
            BroadcastMessage("OnDeath");
        }
    }

    private void UpdateHealthBar()
    {
        _healthBarSlider.value = _currentHealth / _maxHealth;
        _healthBarColor.color = Color.Lerp(Color.red, Color.green, _currentHealth / _maxHealth); ;
    }

    private void OnDeath()
    {
        Destroy(_healthBarSlider.gameObject);
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Death");
        } 

        this.enabled = false;
        Destroy(gameObject, _deathAnimationLength);
    }
}
