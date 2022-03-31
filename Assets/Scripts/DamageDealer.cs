using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);

            if (gameObject.CompareTag("Bullet"))
                Destroy(gameObject);
        }
            
    }
}
