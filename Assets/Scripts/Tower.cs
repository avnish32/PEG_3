using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    /*[SerializeField]
    private Button _buttonForThisTower;*/

    [SerializeField]
    private Towers _thisTower;

    [SerializeField]
    private GameObject _explosionEffect;

    private void OnDeath()
    {
        GameObject towerExplosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(towerExplosion, 3f);

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyTargetDestroyed(gameObject);
        }

        LevelController levelController = FindObjectOfType<LevelController>();
        if (levelController != null)
        {
            levelController.OnTowerDeath();
        }


        /*if (_buttonForThisTower != null)
        {
            Destroy(_buttonForThisTower.gameObject);
        }*/

        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        if (player1 != null && player1.GetComponent<Teleporter>().GetCurrentTower() == _thisTower)
        {
            player1.BroadcastMessage("OnDeath");
            Animator p1Animator = player1.GetComponent<Animator>();
            if (p1Animator != null)
            {
                p1Animator.enabled = true;
                p1Animator.Play("Death");
            }
            Destroy(player1, 3f);
        }
    }

    public Towers GetThisTower()
    {
        return _thisTower;
    }
}
