using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonDeactivator : MonoBehaviour
{
    [SerializeField] private GameObject enemyCannon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyCannon != null && collision.CompareTag("Player"))
        {
            enemyCannon.SetActive(false);
        }
    }
    
}
