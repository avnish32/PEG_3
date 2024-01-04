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

    // Update is called once per frame
    void Update()
    {
        if (!_isInteractableWithinRange || _interactableWithinRange == null)
        {
            return;
        }
        
        
        if (_interactAction.action.IsPressed())
        {
            //_interactableWithinRange.HideInteractMsg();
            _interactableWithinRange.Interact();
        } else
        {
            _interactableWithinRange.DisplayInteractMsg();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<IInteractable>() != null)
        {
            _isInteractableWithinRange = true;
            _interactableWithinRange = collision.gameObject.GetComponent<IInteractable>();
            Debug.Log("Collided with an interactable: " + _interactableWithinRange.ToString());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<IInteractable>() != null)
        {
            //Debug.Log("Exited collider of interactable.");
            if (_interactableWithinRange != null)
            {
                _interactableWithinRange.HideInteractMsg();
            }
            _isInteractableWithinRange = false;
            _interactableWithinRange = null;
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
    }
}
