using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private bool _isInteractableWithinRange = false;
    private IInteractable _interactableWithinRange;

    [SerializeField]
    private InputActionReference _interactAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.action.IsPressed() && _isInteractableWithinRange && _interactableWithinRange != null)
        {
            //Debug.Log("Interacting with interactable.");
            _interactableWithinRange.Interact();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Collided with an interactable.");
            _isInteractableWithinRange = true;
            _interactableWithinRange = collision.gameObject.GetComponent<IInteractable>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Exited collider of interactable.");
            _isInteractableWithinRange = false;
            _interactableWithinRange = null;
        }
    }
}
