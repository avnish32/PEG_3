using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickable
{
    [SerializeField]
    private TextMeshProUGUI _healAmountText;

    [SerializeField]
    private int _minDuration = 1;

    [SerializeField]
    private int _maxDuration = 3;

    private int _healAmountPercentage;
    private float _lifetime;

    // Start is called before the first frame update
    void Start()
    {
        _healAmountPercentage = UnityEngine.Random.Range(1, 101);

        //pickup needs to last longer if amount to heal is low, and vice-versa
        _lifetime = _minDuration + ((1 - ((float)_healAmountPercentage / 100)) * (_maxDuration - _minDuration));
        _healAmountText.text = _healAmountPercentage.ToString() + "\n%";

        StartCoroutine(DestroyAfterDuration());
    }

    public void Pick(GameObject picker)
    {
        Health pickerHealth = picker.GetComponent<Health>();

        if (pickerHealth == null)
        {
            return;
        }

        try
        {
            Player2Health p2Health = (Player2Health)pickerHealth;
            p2Health.CheckToStartHealingEffect();
        }
        catch (InvalidCastException)
        {
            Debug.Log("Invalid cast exception : Object that picked pickup is not Player .");
        }

        float amountToHeal = ((float)_healAmountPercentage / 100) * pickerHealth.GetMaxHealth();
        pickerHealth.SetCurrentHealth(pickerHealth.GetCurrentHealth() + amountToHeal);

        DestroyPickup();
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(_lifetime);

        Animator thisAnimator = GetComponent<Animator>();
        thisAnimator.Play("Death");

        //Destroy(gameObject, 0.5f);
    }
}
