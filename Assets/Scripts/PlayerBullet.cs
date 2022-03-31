using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.name == "Enemy_Cannon")
        {
            collision.gameObject.GetComponent<Cannon>().HitCannonSound();
        }

        if(collision.name == "Enemy Red Cannon")
        {
            collision.gameObject.GetComponent<RedCannon>().HitCannonSound();
        }
    }
}
