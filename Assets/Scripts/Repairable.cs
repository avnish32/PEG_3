using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Repairable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject _interactMsgObject;
    
    private Health _health;

    [SerializeField]
    [Tooltip("Amount of health to replenish per second")]
    private float repairSpeed = 10f;

    void Start()
    {
        if (_interactMsgObject != null)
        {
            _interactMsgObject?.SetActive(false);
        }
    }

    public void Interact()
    {
        _health.SetHealth(_health.GetHealth() + (repairSpeed * Time.deltaTime));
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

    private void Awake()
    {
        _health = GetComponent<Health>();
    }    
}
