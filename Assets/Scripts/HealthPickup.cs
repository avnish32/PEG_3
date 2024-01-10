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
    private int _minDuration = 2;

    [SerializeField]
    private int _maxDuration = 5;

    private int _healAmountPercentage;
    private float _lifetime;

    // Start is called before the first frame update
    void Start()
    {
        _healAmountPercentage = UnityEngine.Random.Range(1, 101);

        //pickup needs to last longer if amount to heal is low, and vice-versa
        _lifetime = _minDuration + ((1 - ((float)_healAmountPercentage / 100)) * (_maxDuration - _minDuration));
        _healAmountText.text = _healAmountPercentage.ToString() + "%";

        StartCoroutine(DestroyAfterDuration());
    }

    public void Pick(GameObject picker)
    {
        Health pickerHealth = picker.GetComponent<Health>();

        if (pickerHealth == null)
        {
            return;
        }

        float amountToHeal = ((float)_healAmountPercentage / 100) * pickerHealth.GetMaxHealth();
        pickerHealth.SetCurrentHealth(pickerHealth.GetCurrentHealth() + amountToHeal);

        Destroy(gameObject);
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
