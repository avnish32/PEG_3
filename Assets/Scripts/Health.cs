using System;
using System.Linq;
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

    public Color healthColor;

    public float GetHealth()
    {
        return _currentHealth;
    }

    public void SetHealth(float health)
    {
        _currentHealth = Math.Min(health, 100f);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        _currentHealth -= damage;
        UpdateHealthBar();

        if (_currentHealth <= 0)
        {
            if (gameObject.CompareTag("Player2"))
            {
                FindObjectOfType<EnemySpawner>().OnPlayer2Died();
            }
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        _healthBarSlider.value = _currentHealth / _maxHealth;
        //r = Mathf.Lerp(255f, 0f, _currentHealth / _maxHealth);
        //g = Mathf.Lerp(0f, 255f, _currentHealth / _maxHealth);
        healthColor = Color.Lerp(Color.red, Color.green, _currentHealth/_maxHealth);
        _healthBarColor.color = healthColor;
        //Debug.Log("Color: R: " + r + "| G: " + g);
    }
}
