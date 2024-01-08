using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationWithCursor : MonoBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPt;

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
    private SpriteRenderer _spriteRenderer;
    private ControlledShooting _controlledShooting;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controlledShooting = GetComponent<ControlledShooting>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.isGamePaused)
        {
            return;
        }

        Vector2 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = cam.ScreenToWorldPoint(cursorScreenPos);

        CalculateRotationFromCursorPos(cursorWorldPos);

        //Debug.Log("rotationInDegrees = " + _rotationInDegrees);

        SetSpriteAndBulletSpawnPtByRotation();

        if (_controlledShooting != null)
        {
            _controlledShooting.SetBulletSpawnPt(_bulletSpawnPt);
        }
    }

    private void CalculateRotationFromCursorPos(Vector3 cursorWorldPos)
    {
        
        Vector3 posDir = cursorWorldPos - transform.position;
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
            _spriteRenderer.sprite = up;
            _bulletSpawnPt = _bulletSpawnPtUp;
        }
        else if (_rotationInDegrees >= 45f && _rotationInDegrees < 135f)
        {
            _spriteRenderer.sprite = left;
            _bulletSpawnPt = _bulletSpawnPtLeft;
        }
        else if (_rotationInDegrees >= 135f && _rotationInDegrees < 225f)
        {
            _spriteRenderer.sprite = down;
            _bulletSpawnPt = _bulletSpawnPtDown;
        }
        else
        {
            _spriteRenderer.sprite = right;
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
