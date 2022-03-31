using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleman : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireSpeed;
    private Animator anim;
    private Health health;

    private int enemyCurrentState;

    private const int ENEMY_LEFT_POSITION = 0;
    private const int ENEMY_LEFT_UP_POSITION = 1;
    private const int ENEMY_LEFT_DOWN_POSITION = 2;
    private const int ENEMY_DEAD = 3;

    private float timeToShooting = 4;
    private float currentTime = 0;

    private Vector3 firepoint;

    [SerializeField] private Transform firepointLeftPosition;
    [SerializeField] private Transform firepointLeftUpPosition;
    [SerializeField] private Transform firepointLeftDownPosition;

    [SerializeField] private BoxCollider2D enemyCollider;
    [SerializeField] private GameObject enemyTriggerCollider;
    private Rigidbody2D rb;
    [SerializeField] private float jumpOffset;

    [SerializeField] private AllowedFire_Rifleman allowedFire_Rifleman;

    [SerializeField] private AudioSource enemyRifleman;
    [SerializeField] private AudioClip enemyDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyCurrentState = 0;
        firepoint = firepointLeftPosition.position;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        switch (enemyCurrentState)
        {
            case ENEMY_LEFT_POSITION:
                anim.SetInteger("state", 0);
                firepoint = firepointLeftPosition.position;
                break;
            case ENEMY_LEFT_UP_POSITION:
                anim.SetInteger("state", 1);
                firepoint = firepointLeftUpPosition.position;
                break;
            case ENEMY_LEFT_DOWN_POSITION:
                anim.SetInteger("state", 2);
                firepoint = firepointLeftDownPosition.position;
                break;
            case ENEMY_DEAD:
                anim.SetBool("isAlive", false);
                break;
            default:
                anim.SetInteger("state", 0);
                break;
        }
                
        if(player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        Vector2 playerCoordinates = player.transform.position - transform.position; // координаты игрока относительно Enemy_Rifleman
       
        if (playerCoordinates.x < 0 && playerCoordinates.y > 0 && Mathf.Abs(playerCoordinates.x) < playerCoordinates.y ||
                 playerCoordinates.x > 0 && playerCoordinates.y > 0 && playerCoordinates.x < playerCoordinates.y)
        {
            enemyCurrentState = 1;
        }
        else if(playerCoordinates.x < 0 && playerCoordinates.y < 0 && playerCoordinates.x > playerCoordinates.y ||
                playerCoordinates.x > 0 && playerCoordinates.y < 0 && playerCoordinates.x > playerCoordinates.y)               
        {
            enemyCurrentState = 2;
        }
        else 
        {
            enemyCurrentState = 0;
        }

        currentTime += Time.deltaTime;
        GameObject currentBullet;

        bool isFireAllowed = allowedFire_Rifleman.IsFireAllowed();

        if (currentTime >= timeToShooting && health.isAlive == true && isFireAllowed == true)
        {     
            currentBullet = Instantiate(bullet, firepoint, Quaternion.identity);
            Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

            currentBulletVelocity.velocity = Vector2.ClampMagnitude(new Vector2(playerCoordinates.x, playerCoordinates.y).normalized, fireSpeed);

            
            currentTime = 0;
        }

        if (health.isAlive == false)
        {
            JumpBeforeDead();
        }
    }
    public void JumpBeforeDead()
    {
        enemyCollider.size = new Vector2(0.01f, 0.01f);
        enemyTriggerCollider.SetActive(false);
        //rb.AddForce(Vector2.up * jumpOffset, ForceMode2D.Impulse);
    }

    public void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        enemyRifleman.PlayOneShot(enemyDead);
    }
}
