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

    [SerializeField]
    private ParticleSystem _healingEffect;

    [SerializeField]
    private float _healthDepletionSpriteEffectBlinkDuration = 2f;

    private bool _isInHealthRefillRange = false;
    private bool _isHealingEffectRunning = false;
    private bool _isHealthDepletionSpriteEffectRunning = false;

    private Coroutine _healthDepletionCoroutine;

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
            _damageModifier = 2.25f;
            SetCurrentHealth(GetCurrentHealth() + (_healthRefillRate * Time.deltaTime));
            CheckToStartHealingEffect();
        }
        else
        {
            _damageModifier = 1f;
            ReduceHealth(_healthDepletionRate * Time.deltaTime);
            CheckToStartHealthDepletionEffect();
        }
    }

    private void CheckToStartHealthDepletionEffect()
    {
        if (_spriteRenderer != null && !_isHealthDepletionSpriteEffectRunning && !_isBulletHitSpriteEffectRunning)
        {
            //Debug.Log("Started depletion coroutine.");
            _healthDepletionCoroutine = StartCoroutine(ShowHealthDepletionSpriteEffect(Time.time));
        }
    }

    private void CheckToStartHealingEffect()
    {
        if (!_isHealingEffectRunning && _currentHealth < _maxHealth)
        {
            StartCoroutine(ShowHealingEffect());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("P2 health triggered.");
        if (collision != null && collision.gameObject.CompareTag("Player1"))
        {
            //Debug.Log("This gameobject is now in P1's replenish range.");
            _isInHealthRefillRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player1"))
        {
            //Debug.Log("This gameobject is not in P1's replenish range anymore.");
            _isInHealthRefillRange = false;
        }
    }

    private IEnumerator ShowHealingEffect()
    {
        _isHealingEffectRunning = true;

        /*float sineAngle;
        do
        {
            sineAngle = ((Time.time - coroutineCallTime) / _healthReplenishSpriteEffectBlinkDuration) * Mathf.PI;
            _spriteRenderer.color = Color.Lerp(Color.white, Color.green, Mathf.Sin(sineAngle));

            yield return null;
        } while (sineAngle <= Mathf.PI);*/

        _healingEffect.Play();
        yield return new WaitForSeconds(0.5f);
        _healingEffect.Stop();

        _isHealingEffectRunning = false;
    }

    private IEnumerator ShowHealthDepletionSpriteEffect(float coroutineCallTime)
    {
        _isHealthDepletionSpriteEffectRunning = true;
        float sineAngle;
        do
        {
            sineAngle = ((Time.time - coroutineCallTime) / _healthDepletionSpriteEffectBlinkDuration) * Mathf.PI;
            _spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(sineAngle));

            yield return null;
        } while (sineAngle <= Mathf.PI);

        _isHealthDepletionSpriteEffectRunning = false;
    }

    public override void OnBulletHit(float damage)
    {
        if (_healthDepletionCoroutine != null)
        {
            StopCoroutine(_healthDepletionCoroutine);
        }        
        
        _isHealthDepletionSpriteEffectRunning = false;
        base.OnBulletHit(damage);
    }
}
