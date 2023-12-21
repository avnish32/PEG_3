using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Button _buttonForThisTower;

    [SerializeField]
    private Towers _thisTower;

    private void OnDestroy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyTargetDestroyed(gameObject);
        }
            

        if ( _buttonForThisTower != null )
        {
            Destroy(_buttonForThisTower.gameObject);
        }

        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        if (player1 != null && player1.GetComponent<Teleporter>().GetCurrentTower() == _thisTower)
        {
            Destroy(player1);
        }
    }
}
