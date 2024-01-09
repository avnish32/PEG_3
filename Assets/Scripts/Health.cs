using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] 
    protected float _currentHealth;

    [SerializeField]
    private Slider _healthBarSlider;

    [SerializeField]
    private Image _healthBarColor;

    [SerializeField]
    protected float _maxHealth = 100f;

    [SerializeField]
    protected float _damageModifier = 1f;

    [SerializeField]
    private float _deathAnimationLength = 0f;

    [SerializeField]
    private AudioClip _deathSound;

    [SerializeField]
    float _spriteHitEffectBlinkDuration = 0.05f;

    [SerializeField]
    AudioPlayer _audioPlayer;

    protected SpriteRenderer _spriteRenderer;
    protected bool _isBulletHitSpriteEffectRunning = false;

    private void Awake()
    {
        //Debug.Log("Audio clip on awake: " + _deathSound);
        //Debug.Log("Sine pi/2: " + Mathf.Sin(Mathf.PI / 2));
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_audioPlayer == null)
        {
            _audioPlayer = GameObject.FindFirstObjectByType<AudioPlayer>();
        }
    }

    private void Start()
    {
        SetCurrentHealth(_maxHealth);
    }

    private void UpdateHealthBar()
    {
        _healthBarSlider.value = _currentHealth / _maxHealth;
        _healthBarColor.color = Color.Lerp(Color.red, Color.green, _currentHealth / _maxHealth); ;
    }    

    private IEnumerator ShowBulletHitSpriteEffect(float coroutineCallTime)
    {
        _isBulletHitSpriteEffectRunning = true;
        float sineAngle;
        do
        {
            sineAngle = ((Time.time - coroutineCallTime) / _spriteHitEffectBlinkDuration)*Mathf.PI;
            _spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(sineAngle));

            yield return null;
        } while (sineAngle < Mathf.PI);

        _isBulletHitSpriteEffectRunning = false;
    }

    private void OnDeath()
    {
        _audioPlayer.PlaySFX(_deathSound);

        Destroy(_healthBarSlider.gameObject);
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Death");
        } 

        this.enabled = false;
        Destroy(gameObject, _deathAnimationLength);
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public virtual void SetCurrentHealth(float health)
    {
        _currentHealth = Math.Min(health, _maxHealth);
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        UpdateHealthBar();
    }

    public virtual void ReduceHealth(float damage)
    {
        SetCurrentHealth(_currentHealth - damage);

        if (_currentHealth == 0)
        {
            if (gameObject.CompareTag("Player2"))
            {
                FindObjectOfType<EnemySpawner>().OnEnemyTargetDestroyed(gameObject);
            }
            BroadcastMessage("OnDeath");
        }
    }

    public virtual void OnBulletHit(float damage)
    {
        BroadcastMessage("OnBulletHitBroadcast", SendMessageOptions.DontRequireReceiver);

        ReduceHealth(damage * _damageModifier);
        StartCoroutine(ShowBulletHitSpriteEffect(Time.time));
    }

    public bool IsBulletHitSpriteEffectRunning()
    {
        return _isBulletHitSpriteEffectRunning;
    }
}
