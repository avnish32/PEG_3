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

    [SerializeField]
    AudioClip _deathSound;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    float _spriteHitEffectBlinkDuration = 0.05f;

    private void Awake()
    {
        //Debug.Log("Audio clip on awake: " + _deathSound);
        Debug.Log("Sine pi/2: " + Mathf.Sin(Mathf.PI / 2));
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    public void OnBulletHit(float damage)
    {
        ReduceHealth(damage);
        StartCoroutine(StartBulletHitSpriteEffect(Time.time));
    }

    IEnumerator StartBulletHitSpriteEffect(float coroutineCallTime)
    {
        float sineAngle;
        do
        {
            sineAngle = ((Time.time - coroutineCallTime) / _spriteHitEffectBlinkDuration)*Mathf.PI;
            _spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(sineAngle));

            yield return null;
        } while (sineAngle < Mathf.PI);
    }

    private void OnDeath()
    {
        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position);
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
