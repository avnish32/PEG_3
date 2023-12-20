using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script destroys the gameObject it is attached to
 * if it goes farther than 'destroyDistance' from its
 * spawn point.
 * */
public class DistanceBasedDestroyer : MonoBehaviour
{
    [SerializeField]
    private float _destroyDistance;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private Vector3 _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromSpawnPt = Vector3.Distance(_spawnPoint, transform.position);
        Color transparentColor = new(255f, 255f, 255f, 0f);
        //_spriteRenderer.color = new Color(255f, 255f, 255f, Mathf.Lerp(255f, 0f, distanceFromSpawnPt / _destroyDistance));
        _spriteRenderer.color = Color.Lerp(Color.white, transparentColor, distanceFromSpawnPt / _destroyDistance);

        if (distanceFromSpawnPt > _destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
