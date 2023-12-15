using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitEffect != null)
        {
            GameObject newHitEffect = Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            Destroy(newHitEffect, 2f);
        }
        Destroy(gameObject);
    }
}
