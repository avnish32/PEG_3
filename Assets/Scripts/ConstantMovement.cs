using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Constantly moving object instantiated.");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(rb.transform.up * movementSpeed, ForceMode2D.Impulse);
        rb.velocity = transform.up * movementSpeed;
    }
}
