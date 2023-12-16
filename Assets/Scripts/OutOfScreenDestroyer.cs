using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfScreenDestroyer : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject.transform.root.gameObject);
    }
}
