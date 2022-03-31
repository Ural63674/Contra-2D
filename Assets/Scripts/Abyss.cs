using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {            
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);                            
        }
    }
}
