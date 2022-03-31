using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRiflemanActivator : MonoBehaviour
{
    [SerializeField] private GameObject rifleman;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(rifleman != null && collision.CompareTag("Player"))
        {
            rifleman.SetActive(true);
        }
    }
}
