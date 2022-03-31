using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCannonActivator : MonoBehaviour
{
    [SerializeField] private GameObject redCannon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (redCannon != null && collision.CompareTag("Player"))
        {
            redCannon.SetActive(true);
        }
    }
}
