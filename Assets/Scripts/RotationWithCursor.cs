using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationWithCursor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Camera cam;

    private float rotationInDegrees;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 cursorScreenPos = cam.ScreenToWorldPoint(mousePos);
        Vector2 posDir = cursorScreenPos - rb.position;
        double rotationInRadians = Math.Atan2(posDir.y, posDir.x);
        rotationInDegrees = (float) rotationInRadians * Mathf.Rad2Deg;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.rotation = rotationInDegrees + 90f;
        }
    }
}
