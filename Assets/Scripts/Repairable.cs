using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Repairable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject _interactMsgObject;

    [SerializeField]
    private ParticleSystem _repairEffect;

    [SerializeField]
    [Tooltip("Amount of health to replenish per second")]
    private float repairSpeed = 10f;

    private Health _health;
    private bool _isSpriteRepairEffectRunning = false;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    void Start()
    {
        if (_interactMsgObject != null)
        {
            _interactMsgObject.SetActive(false);
        }
    }

    private IEnumerator ShowRepairSpriteEffect()
    {
        _isSpriteRepairEffectRunning = true;

        _repairEffect.Play();
        yield return new WaitForSeconds(0.5f);
        _repairEffect.Stop();
        
        _isSpriteRepairEffectRunning = false;
    }

    /*private void OnBulletHitBroadcast()
    {
        if (_repairEffectCoroutine != null)
        {
            StopCoroutine(_repairEffectCoroutine);
        }
        _isSpriteRepairEffectRunning = false;
    }*/

    public void Interact()
    {
        _health.SetCurrentHealth(_health.GetCurrentHealth() + (repairSpeed * Time.deltaTime));

        if (!_isSpriteRepairEffectRunning && _health.GetCurrentHealth() < _health.GetMaxHealth())
        {
            StartCoroutine(ShowRepairSpriteEffect());
        }
    }

    public void DisplayInteractMsg()
    {
        //Debug.Log("Inside Repairable::DisplayInteractMsg");
        if (_interactMsgObject != null)
        {
            _interactMsgObject.SetActive(true);
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
