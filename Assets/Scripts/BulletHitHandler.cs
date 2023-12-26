using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private float damage = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health otherObjectHealth = collision.gameObject.GetComponent<Health>();
        if (otherObjectHealth != null && otherObjectHealth.enabled)
        {
            otherObjectHealth.ReduceHealth(damage);
        }
        
        if (hitEffect != null)
        {
            GameObject newHitEffect = Instantiate(hitEffect, collision.GetContact(0).point, Quaternion.identity);
            Destroy(newHitEffect, 2f);
        }
        Destroy(gameObject);
    }
}
