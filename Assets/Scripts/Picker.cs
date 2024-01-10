using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            return;
        }

        IPickable pickable = collision.gameObject.GetComponent<IPickable>();
        if (pickable == null)
        {
            //Debug.Log("No IPickable component on collided trigger.");
            return;
        }

        Health thisObjectHealth = GetComponent<Health>();
        if (thisObjectHealth == null)
        {
            return;
        }

        pickable.Pick(thisObjectHealth);
    }
}
