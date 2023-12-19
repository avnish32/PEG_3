using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairable : MonoBehaviour, IInteractable
{
    private Health _health;

    [SerializeField]
    [Tooltip("Amount of health to replenish per second")]
    private float repairSpeed = 10f;

    public void Interact()
    {
        _health.SetHealth(_health.GetHealth() + (repairSpeed * Time.deltaTime));
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
