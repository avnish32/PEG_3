using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RotationWithCursor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Sprite down;
    [SerializeField]
    private Sprite up;
    [SerializeField]
    private Sprite left;
    [SerializeField]
    private Sprite right;

    private float rotationInDegrees;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.isGamePaused)
        {
            return;
        }

        Vector2 mousePos = Input.mousePosition;
        Vector2 cursorScreenPos = cam.ScreenToWorldPoint(mousePos);
        Vector2 posDir = cursorScreenPos - rb.position;
        double rotationInRadians = Math.Atan2(posDir.y, posDir.x);
        rotationInDegrees = (float) rotationInRadians * Mathf.Rad2Deg;

        rotationInDegrees += 90f;
        if (rotationInDegrees < -45f || rotationInDegrees >= 225f)
        {
            spriteRenderer.sprite = left;
        }
        else if (rotationInDegrees >= -45f && rotationInDegrees < 45f)
        {
            spriteRenderer.sprite = down;
        }
        else if (rotationInDegrees >= 45f && rotationInDegrees < 135f)
        {
            spriteRenderer.sprite = right;
        }
        else
        {
            spriteRenderer.sprite = up;
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.rotation = rotationInDegrees;
            //Debug.Log("P1 rotation: " + rotationInDegrees);
        }
    }

    private void OnDeath()
    {
        this.enabled = false;
    }
}
