using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCannon : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireSpeed;
    private Animator anim;
    private Health health;

    private int cannonCurrentState;

    private const int CANNON_CREATING = 0;
    private const int CANNON_POSITION_9 = 9; // За основу взято направление часовой стрелки
    private const int CANNON_POSITION_10 = 10;
    private const int CANNON_POSITION_11 = 11;
    private const int CANNON_DESTROY = 1;

    private int playerPosition;
    private bool isFireAllowed;
    private bool isChangePositionNeed;

    private float timeToShooting = 2;
    private float currentTimeToShooting = 0;

    private float timeToChangePosition = 1;
    private float currentTimeToChangePosition;

    private int bulletsCount = 3;
    private float maxShotDelay = 0.3f;
    private float shotDelay = 0;

    private Vector3 firepoint;

    [SerializeField] private GameObject firepoints;

    [SerializeField] private Transform firePoint_9;
    [SerializeField] private Transform firePoint_10;
    [SerializeField] private Transform firePoint_11;

    [SerializeField] private BoxCollider2D RedCannonBoxCollider;

    [SerializeField] private AudioSource redCannonAudioSource;
    [SerializeField] private AudioClip hitCannon;
    [SerializeField] private AudioClip explosion;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        redCannonAudioSource = GetComponent<AudioSource>();
        cannonCurrentState = CANNON_CREATING;
        firepoint = firePoint_9.position;
        RedCannonBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        switch (cannonCurrentState)
        {
            case CANNON_CREATING:
                break;
            case CANNON_POSITION_9:
                firepoint = firePoint_9.position;
                anim.SetInteger("Position", 9);
                break;
            case CANNON_POSITION_10:
                firepoint = firePoint_10.position;
                anim.SetInteger("Position", 10);
                break;
            case CANNON_POSITION_11:
                firepoint = firePoint_11.position;
                anim.SetInteger("Position", 11);
                break;
            case CANNON_DESTROY:
                anim.SetBool("isAlive", false);
                break;
            default:
                firepoint = firePoint_9.position;
                anim.SetInteger("Position", 9);
                break;
        }

        GetPlayerPosition();

        int checkCannonAndPlayerPosition = CheckCannonAndPlayerPosition();
        if (isChangePositionNeed)
        {
            currentTimeToChangePosition += Time.deltaTime;

            if (checkCannonAndPlayerPosition > 0 && currentTimeToChangePosition > timeToChangePosition)
            {
                cannonCurrentState++;
                currentTimeToChangePosition = 0;
            }
            else if (checkCannonAndPlayerPosition < 0 && currentTimeToChangePosition > timeToChangePosition)
            {
                cannonCurrentState--;               
                currentTimeToChangePosition = 0;
            }
        }

        currentTimeToShooting += Time.deltaTime;
        GameObject currentBullet;

        if (currentTimeToShooting >= timeToShooting && health.isAlive == true && checkCannonAndPlayerPosition == 0 && isFireAllowed == true)
        {            
            shotDelay += Time.deltaTime;

            if(shotDelay > maxShotDelay && bulletsCount > 0)
            {                
                currentBullet = Instantiate(bullet, firepoint, Quaternion.identity);
                Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

                Vector3 fireDirection = firepoint - firepoints.transform.position;
                currentBulletVelocity.velocity = Vector2.ClampMagnitude(fireDirection, fireSpeed) * fireSpeed;

                bulletsCount--;
                shotDelay = 0;
            }
            
            if(bulletsCount <= 0)
            {
                currentTimeToShooting = 0;
                bulletsCount = 3;
            }              
        }

        if (health.isAlive == false)
        {
            cannonCurrentState = CANNON_DESTROY;
        }
    }

    private int CheckCannonAndPlayerPosition()
    {
        int value = playerPosition - cannonCurrentState;
        if (value != 0)
            isChangePositionNeed = true;
        else
            isChangePositionNeed = false;

        return value;
    }

    private void GetPlayerPosition()
    {
        Vector2 playerCoordinates = player.transform.position - transform.position; // координаты игрока относительно RedCannon

        if (playerCoordinates.x > 0)
        {            
            playerPosition = 11;
            isFireAllowed = false;
        }
        else if (playerCoordinates.x < 0 && playerCoordinates.y < 0 && Mathf.Abs(playerCoordinates.x) / 4 < Mathf.Abs(playerCoordinates.y))
        {
            playerPosition = 9;
            isFireAllowed = false;
        }                

        else if (playerCoordinates.y > 0 && playerCoordinates.x < 0 || Mathf.Abs(playerCoordinates.x) / 4 > playerCoordinates.y)
        {            
            if (Mathf.Abs(playerCoordinates.x) < playerCoordinates.y)
            {
                playerPosition = 11;
                isFireAllowed = true;
            }
            else if (Mathf.Abs(playerCoordinates.x) > playerCoordinates.y && Mathf.Abs(playerCoordinates.x) / 4 < playerCoordinates.y)
            {
                playerPosition = 10;
                isFireAllowed = true;
            }
            else if (Mathf.Abs(playerCoordinates.x) / 4 > Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 9;
                isFireAllowed = true;
            }
        }
    }

    public void DefaultPosition()
    {
        cannonCurrentState = CANNON_POSITION_9;
        RedCannonBoxCollider.enabled = true;
    }

    public void HitCannonSound()
    {
        if (health.isAlive == true)
            redCannonAudioSource.PlayOneShot(hitCannon);
    }

    private void DeadSound() // используется в анимации
    {
        redCannonAudioSource.PlayOneShot(explosion);
    }
}
