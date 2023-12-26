using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RotationWithCursor : MonoBehaviour
{
    [SerializeField]
    private ControlledShooting _controlledShooting;

    [SerializeField]
    private Transform _bulletSpawnPt;

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

    [SerializeField]
    private Transform _bulletSpawnPtUp;
    [SerializeField]
    private Transform _bulletSpawnPtDown;
    [SerializeField]
    private Transform _bulletSpawnPtRight;
    [SerializeField]
    private Transform _bulletSpawnPtLeft;

    private float _rotationInDegrees;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        _controlledShooting = GetComponent<ControlledShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.isGamePaused)
        {
            return;
        }

        CalculateRotationFromCursorPos();

        //Debug.Log("rotationInDegrees = " + _rotationInDegrees);

        SetSpriteAndBulletSpawnPtByRotation();

        if (_controlledShooting != null)
        {
            _controlledShooting.SetBulletSpawnPt(_bulletSpawnPt);
        }
    }

    private void CalculateRotationFromCursorPos()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 cursorScreenPos = cam.ScreenToWorldPoint(mousePos);
        Vector3 posDir = cursorScreenPos - transform.position;
        double rotationInRadians = Math.Atan2(posDir.y, posDir.x);
        _rotationInDegrees = (float)rotationInRadians * Mathf.Rad2Deg;

        _rotationInDegrees -= 90f;
        if (_rotationInDegrees < 0)
        {
            _rotationInDegrees = 360f + _rotationInDegrees;
        }
    }

    private void SetSpriteAndBulletSpawnPtByRotation()
    {
        if (_rotationInDegrees > 315f || _rotationInDegrees < 45f)
        {
            spriteRenderer.sprite = up;
            _bulletSpawnPt = _bulletSpawnPtUp;
        }
        else if (_rotationInDegrees >= 45f && _rotationInDegrees < 135f)
        {
            spriteRenderer.sprite = left;
            _bulletSpawnPt = _bulletSpawnPtLeft;
        }
        else if (_rotationInDegrees >= 135f && _rotationInDegrees < 225f)
        {
            spriteRenderer.sprite = down;
            _bulletSpawnPt = _bulletSpawnPtDown;
        }
        else
        {
            spriteRenderer.sprite = right;
            _bulletSpawnPt = _bulletSpawnPtRight;
        }
    }

    private void FixedUpdate()
    {
        /*if (rb != null)
        {
            rb.rotation = rotationInDegrees;
            //Debug.Log("P1 rotation: " + rotationInDegrees);
        }*/

        if (_bulletSpawnPt != null)
        {
            _bulletSpawnPt.rotation = Quaternion.Euler(0f, 0f, _rotationInDegrees);
        }
        
    }

    private void OnDeath()
    {
        this.enabled = false;
    }

    public Transform GetCurrentBulletSpawnPt()
    {
        return _bulletSpawnPt;
    }
}
