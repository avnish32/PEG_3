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
    //private InputActionAsset playerInputs; //Can be used when whole action asset along with all its maps needs to be referenced.
    //This requires a string name of the action. e.g. playerInputs.FindAction("Fire")
    private InputActionReference playerFire; //This is useful for when only one particular action needs to be referenced.
                                             //It also doesn't need a string to be referenced, so avoids hardcoding.

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckAndShoot", 0f, 0.1f);
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
        }
        
    }

    private void OnDeath()
    {
        CancelInvoke();
    }

    public void SetBulletSpawnPt(Transform bulletSpawnPt)
    {
        _bulletSpawnPt = bulletSpawnPt;
    }
}
