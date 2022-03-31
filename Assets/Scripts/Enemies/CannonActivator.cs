using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonActivator : MonoBehaviour
{
    [SerializeField] private GameObject enemyCannon;
    [SerializeField] private GameObject cannonIdlePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyCannon != null && collision.CompareTag("Player"))
        {
            enemyCannon.SetActive(true);
            cannonIdlePosition.SetActive(false);
        }
    }
}
