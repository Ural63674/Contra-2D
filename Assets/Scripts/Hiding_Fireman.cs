using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding_Fireman : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    private Animator anim;
    private Health health;
    private Rigidbody2D rb;

    private int enemyCurrentState;

    private const int ENEMY_IDLE_POSITION = 0;
    private const int ENEMY_RISING_POSITION = 1;
    private const int ENEMY_SHOOTING_POSITION = 2;
    private const int ENEMY_HIDING_POSITION = 3;
    private const int ENEMY_DEAD = 4;

    private float timeToRaising = 3;
    private float currentTime = 0;
    private int fireDirection;

    private bool isEnemyHiding;
    [SerializeField] private float fireSpeed;

    private Vector3 firepoint;

    [SerializeField] private Transform firepointLeftPosition;
    [SerializeField] private BoxCollider2D enemyCollider;
    [SerializeField] private GameObject enemyTriggerCollider;
    [SerializeField] private float jumpOffset;

    [SerializeField] private AudioSource hidingEnemy;
    [SerializeField] private AudioClip enemyDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyCurrentState = 0;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (enemyCurrentState)
        {
            case ENEMY_IDLE_POSITION:
                anim.SetInteger("Position", 0);
                isEnemyHiding = true;
                break;
            case ENEMY_RISING_POSITION:
                anim.SetInteger("Position", 1);
                break;
            case ENEMY_SHOOTING_POSITION:
                anim.SetInteger("Position", 2);
                break;
            case ENEMY_HIDING_POSITION:
                anim.SetInteger("Position", 3);
                break;
            case ENEMY_DEAD:
                anim.SetBool("isAlive", false);
                break;
            default:
                anim.SetInteger("Position", 0);
                break;
        }

        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            fireDirection = 1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            fireDirection = -1;
        }

        Vector2 playerCoordinates = player.transform.position - transform.position; // координаты игрока относительно Enemy_Rifleman        

        if (isEnemyHiding)
        {
            currentTime += Time.deltaTime;
            gameObject.tag = "Hiding Enemy";
            gameObject.layer = 10; // Dead Player layer
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 9; // Enemy layer
        }            

        if (currentTime >= timeToRaising && health.isAlive == true)
        {
            enemyCurrentState = ENEMY_RISING_POSITION;
            isEnemyHiding = false;
            currentTime = 0;
        }

        if (health.isAlive == false)
        {
            JumpBeforeDead();
        }

        if(enemyCurrentState == ENEMY_SHOOTING_POSITION)
        {
            enemyTriggerCollider.SetActive(true);
        }
        else
            enemyTriggerCollider.SetActive(false);
    }
    public void JumpBeforeDead()
    {
        enemyCollider.size = new Vector2(0.01f, 0.01f);
        enemyTriggerCollider.SetActive(false);
        rb.AddForce(Vector2.up * jumpOffset, ForceMode2D.Impulse);
    }

    public void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        hidingEnemy.PlayOneShot(enemyDead);
    }

    public void Shoot()
    {
        GameObject currentBullet;

        firepoint = firepointLeftPosition.position;

        currentBullet = Instantiate(bullet, firepoint, Quaternion.identity);
        Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

        currentBulletVelocity.velocity = new Vector2(fireSpeed * fireDirection, currentBulletVelocity.velocity.y);
    }

    public void ShootingPosition()
    {
        enemyCurrentState = ENEMY_SHOOTING_POSITION;
    }

    public void HidingPosition()
    {
        enemyCurrentState = ENEMY_HIDING_POSITION;
    }

    public void IdlePosition()
    {
        enemyCurrentState = ENEMY_IDLE_POSITION;
    }
}
