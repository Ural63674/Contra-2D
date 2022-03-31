using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunnerCreator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyRaycastPoint;
    [SerializeField] private LayerMask groundMask;
    private GameObject _enemy;
    private float distance = 4f;

    private float currentTime;
    private float nextInstantiateTime;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > nextInstantiateTime)
        {
            currentTime = 0;

            nextInstantiateTime = Random.Range(minDelay, maxDelay);

            RaycastHit2D[] hit = Physics2D.RaycastAll(enemyRaycastPoint.position, Vector2.down, distance, groundMask);

            if (hit[0].collider != null)
            {
                int rnd = Random.Range(0, hit.Length);

                _enemy = Instantiate(enemyPrefab) as GameObject; // Создание бегущего врага
                _enemy.transform.position = hit[rnd].point;
            }
        }
    }
}
