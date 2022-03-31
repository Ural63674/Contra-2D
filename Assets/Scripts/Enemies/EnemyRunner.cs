using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunner : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask waterMask;

    private bool movingLeft = true;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    [SerializeField] private float jumpPower;

    private Animator anim;
    private bool isJumping = false;

    private Health health;

    [SerializeField] private float jumpOffset;
    [SerializeField] private GameObject enemyCollider;

    [SerializeField] private AudioSource enemyRunner;
    [SerializeField] private AudioClip enemyDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if(health.isAlive)
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, groundMask);
        RaycastHit2D waterInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, waterMask);

        if (health.isAlive == false)
        {
            JumpBeforeDead();
        }

        if (groundInfo.collider == false && isJumping == false && health.isAlive == true)
        {
            int chance = Random.Range(0, 2);

            if (chance < 1 && movingLeft == true)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = false;
                StartCoroutine(StopFreezePositionY());
            }
            else if (chance < 1 && movingLeft == false)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                transform.eulerAngles = new Vector3(0, 180, 0);
                movingLeft = true;
                StartCoroutine(StopFreezePositionY());
            }
            else if (chance >= 1)
            {
                Jump();
            }
        }
        
        if(groundInfo.collider == true && isJumping == true)
        {
            anim.SetBool("IsJumping", false);
            isJumping = false;
        }

        if(waterInfo.collider == true)
        {
            anim.SetBool("IsInWater", true);
            speed = 0;
        }        
    }

    public void Jump()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        anim.SetBool("IsJumping", true);
        isJumping = true;
    }

    private IEnumerator StopFreezePositionY()
    {
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }   

    public void EnemyDead() // используется в анимации
    {
        Destroy(gameObject);
    }
   
    public void JumpBeforeDead()
    {
        capsuleCollider.size = new Vector2(0.01f, 0.01f);
        enemyCollider.SetActive(false);
        rb.AddForce(Vector2.up * jumpOffset, ForceMode2D.Impulse);
        WaitHalfSecond();
    }

    private IEnumerator WaitHalfSecond()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }

    public void FreezyPosition() // используется в анимации
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        enemyRunner.PlayOneShot(enemyDead);
    }
}
