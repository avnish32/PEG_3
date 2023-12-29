using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Repairable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject _interactMsgObject;

    [SerializeField]
    [Tooltip("Amount of health to replenish per second")]
    private float repairSpeed = 10f;

    [SerializeField]
    private float _spriteRepairEffectBlinkDuration = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Health _health;
    private bool _isSpriteRepairEffectRunning = false;
    private Coroutine _repairEffectCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();
    }

    void Start()
    {
        if (_interactMsgObject != null)
        {
            _interactMsgObject?.SetActive(false);
        }
    }

    private IEnumerator _ShowRepairSpriteEffect(float coroutineCallTime)
    {
        _isSpriteRepairEffectRunning = true;
        float sineAngle;
        do
        {
            sineAngle = ((Time.time - coroutineCallTime) / _spriteRepairEffectBlinkDuration) * Mathf.PI;
            _spriteRenderer.color = Color.Lerp(Color.white, Color.green, Mathf.Sin(sineAngle));

            yield return null;
        } while (sineAngle < Mathf.PI);
        
        _isSpriteRepairEffectRunning = false;
    }

    private void OnBulletHitBroadcast()
    {
        if (_repairEffectCoroutine != null)
        {
            StopCoroutine(_repairEffectCoroutine);
        }
        _isSpriteRepairEffectRunning = false;
    }

    public void Interact()
    {
        _health.SetCurrentHealth(_health.GetCurrentHealth() + (repairSpeed * Time.deltaTime));

        if (_spriteRenderer != null && !_health.IsBulletHitSpriteEffectRunning() 
            && !_isSpriteRepairEffectRunning && _health.GetCurrentHealth() < _health.GetMaxHealth())
        {
            _repairEffectCoroutine = StartCoroutine(_ShowRepairSpriteEffect(Time.time));
        }
    }

    public void DisplayInteractMsg()
    {
        //Debug.Log("Inside Repairable::DisplayInteractMsg");
        //TODO Do we want to display interact msg every time or only when health is not full?
        if (_interactMsgObject != null)
        {
            _interactMsgObject?.SetActive(true);
        }
    }

    public void HideInteractMsg()
    {
        if (_interactMsgObject != null)
        {
            _interactMsgObject.SetActive(false);
        }
    }
}
