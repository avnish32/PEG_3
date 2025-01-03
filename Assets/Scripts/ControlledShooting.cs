using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlledShooting : MonoBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPt;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject _crosshair;

    [SerializeField]
    private float _shootRange = 4f;

    [SerializeField]
    private AudioClip _bulletSound;

    [SerializeField]
    private AudioClip _deathSound;

    [SerializeField]
    AudioPlayer _audioPlayer;

    [SerializeField]
    //private InputActionAsset playerInputs; //Can be used when whole action asset along with all its maps needs to be referenced.
    //This requires a string name of the action. e.g. playerInputs.FindAction("Fire")
    private InputActionReference playerFire; //This is useful for when only one particular action needs to be referenced.
                                             //It also doesn't need a string to be referenced, so avoids hardcoding.

    private GameObject _instantiatedCrosshair;

    private void Awake()
    {
        if (_audioPlayer == null)
        {
            _audioPlayer = GameObject.FindFirstObjectByType<AudioPlayer>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckAndShoot", 0f, 0.1f);
        _instantiatedCrosshair = Instantiate(_crosshair);
    }

    private void Update()
    {
        if(LevelController.isGamePaused)
        {
            return;
        }

        UpdateCrosshairPos();
        CheckForEnemyInCrosshair();
    }

    private void CheckForEnemyInCrosshair()
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(_instantiatedCrosshair.transform.position, Vector2.zero, 0f, LayerMask.GetMask("PlayerTarget"));

        if (!hit)
        {
            //Debug.Log("PlayerTargetNotHit");
            _instantiatedCrosshair.GetComponentInChildren<SpriteRenderer>().color = Color.black;
            return;
        }
        Health playerTargetHealthUnderCursor = hit.collider.GetComponent<Health>();
        if (playerTargetHealthUnderCursor == null)
        {
            _instantiatedCrosshair.GetComponentInChildren<SpriteRenderer>().color = Color.black;
            return;
        }

        //Debug.Log("PlayerTarget with health hit");
        _instantiatedCrosshair.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

    private void UpdateCrosshairPos()
    {
        Vector2 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);

        Vector3 clampedPosWrtOrigin = Vector3.ClampMagnitude(new Vector3(cursorWorldPos.x - transform.position.x, cursorWorldPos.y - transform.position.y, 0f), _shootRange);

        float horiExtent = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        float vertExtent = Camera.main.orthographicSize;

        //Debug.Log("Hori/vert extent: "+horiExtent+" "+vertExtent);

        float xCoordinateClamped = Mathf.Clamp(clampedPosWrtOrigin.x + transform.position.x, -horiExtent, horiExtent);
        float yCoordinateClamped = Mathf.Clamp(clampedPosWrtOrigin.y + transform.position.y, -vertExtent, vertExtent);
        _instantiatedCrosshair.transform.position = new Vector3(xCoordinateClamped, yCoordinateClamped, 0f);
    }

    void OnFire()
    {
        //Debug.Log("P1 fired.");
        CheckAndShoot();
    }

    void CheckAndShoot()
    {
        if (LevelController.isGamePaused)
        {
            return;
        }

        if (playerFire.action.IsPressed())
        {
            Instantiate(bullet, _bulletSpawnPt.position, _bulletSpawnPt.rotation);
            _audioPlayer.PlaySFX(_bulletSound);
        }
        
    }

    private void OnDeath()
    {
        CancelInvoke();
        Destroy(_instantiatedCrosshair);
        _audioPlayer.PlaySFX(_deathSound);
        this.enabled = false;
        //AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position);
    }

    public void SetBulletSpawnPt(Transform bulletSpawnPt)
    {
        _bulletSpawnPt = bulletSpawnPt;
    }

    public float GetShootRange()
    {
        return _shootRange;
    }
}
