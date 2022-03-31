using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float currentHealth;
    public bool isAlive;

    private Animator deathAnimation;

    private void Awake()
    {
        currentHealth = maxHealth;
        isAlive = true;
        deathAnimation = GetComponent<Animator>();
        deathAnimation.SetBool("isAlive", true);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CheckIsAlive();
    }

    public void CheckIsAlive()
    {
        if (currentHealth > 0)
            isAlive = true;
        else
        {
            isAlive = false;
            deathAnimation.SetBool("isAlive", false);
        }

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
