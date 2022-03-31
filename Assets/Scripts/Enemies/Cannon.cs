using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireSpeed;

    [SerializeField] private AudioSource cannonAudioSource;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip hitCannon;
    private Animator anim;
    private Health health;

    private int cannonCurrentState;

    private const int CANNON_POSITION_0 = 0;
    private const int CANNON_POSITION_1 = 1;
    private const int CANNON_POSITION_2 = 2;
    private const int CANNON_POSITION_3 = 3;
    private const int CANNON_POSITION_4 = 4;
    private const int CANNON_POSITION_5 = 5;
    private const int CANNON_POSITION_6 = 6;
    private const int CANNON_POSITION_7 = 7;
    private const int CANNON_POSITION_8 = 8;
    private const int CANNON_POSITION_9 = 9;
    private const int CANNON_POSITION_10 = 10;
    private const int CANNON_POSITION_11 = 11;
    private const int CANNON_DESTROY = 12;

    private int playerPosition;
    private bool isChangePositionNeed;

    private float timeToShooting = 4;
    private float currentTimeToShooting = 0;

    private float timeToChangePosition = 1;
    private float currentTimeToChangePosition;

    private Vector3 firepoint;

    [SerializeField] private GameObject firepoints;

    [SerializeField] private Transform firePoint_0;
    [SerializeField] private Transform firePoint_1;
    [SerializeField] private Transform firePoint_2;
    [SerializeField] private Transform firePoint_3;
    [SerializeField] private Transform firePoint_4;
    [SerializeField] private Transform firePoint_5;
    [SerializeField] private Transform firePoint_6;
    [SerializeField] private Transform firePoint_7;
    [SerializeField] private Transform firePoint_8;
    [SerializeField] private Transform firePoint_9;
    [SerializeField] private Transform firePoint_10;
    [SerializeField] private Transform firePoint_11;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        cannonCurrentState = CANNON_POSITION_3;
        firepoint = firePoint_3.position;
    }

    void Update()
    {
        switch (cannonCurrentState)
        {
            case CANNON_POSITION_0:
                firepoint = firePoint_0.position;
                anim.SetInteger("Position", 0);
                break;
            case CANNON_POSITION_1:
                firepoint = firePoint_1.position;
                anim.SetInteger("Position", 1);
                break;
            case CANNON_POSITION_2:
                firepoint = firePoint_2.position;
                anim.SetInteger("Position", 2);
                break;
            case CANNON_POSITION_3:
                firepoint = firePoint_3.position;
                anim.SetInteger("Position", 3);
                break;
            case CANNON_POSITION_4:
                firepoint = firePoint_4.position;
                anim.SetInteger("Position", 4);
                break;
            case CANNON_POSITION_5:
                firepoint = firePoint_5.position;
                anim.SetInteger("Position", 5);
                break;
            case CANNON_POSITION_6:
                firepoint = firePoint_6.position;
                anim.SetInteger("Position", 6);
                break;
            case CANNON_POSITION_7:
                firepoint = firePoint_7.position;
                anim.SetInteger("Position", 7);
                break;
            case CANNON_POSITION_8:
                firepoint = firePoint_8.position;
                anim.SetInteger("Position", 8);
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
        }        

        GetPlayerPosition();

        int checkCannonAndPlayerPosition = CheckCannonAndPlayerPosition();
        if (isChangePositionNeed)
        {            
            currentTimeToChangePosition += Time.deltaTime;

            if (checkCannonAndPlayerPosition < 6 && checkCannonAndPlayerPosition > 0 && currentTimeToChangePosition > timeToChangePosition ||
                checkCannonAndPlayerPosition < -6 && currentTimeToChangePosition > timeToChangePosition)
            {
                cannonCurrentState++;
                if (cannonCurrentState > 11)
                    cannonCurrentState = 0;
                currentTimeToChangePosition = 0;
            }
            else if(checkCannonAndPlayerPosition > 6 && currentTimeToChangePosition > timeToChangePosition ||
                    checkCannonAndPlayerPosition < 0 && checkCannonAndPlayerPosition > -6 && currentTimeToChangePosition > timeToChangePosition)
            {
                cannonCurrentState--;
                if (cannonCurrentState < 0)
                    cannonCurrentState = 11;
                currentTimeToChangePosition = 0;
            }
        }

        currentTimeToShooting += Time.deltaTime;
        GameObject currentBullet;   

        if (currentTimeToShooting >= timeToShooting && health.isAlive == true && checkCannonAndPlayerPosition == 0)
        {
            currentBullet = Instantiate(bullet, firepoint, Quaternion.identity);
            Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

            Vector3 fireDirection = firepoint - firepoints.transform.position;
            currentBulletVelocity.velocity = Vector2.ClampMagnitude(fireDirection.normalized, fireSpeed) * fireSpeed;

            currentTimeToShooting = 0;
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
        Vector2 playerCoordinates = player.transform.position - transform.position; // координаты игрока относительно Enemy_Cannon

        if (playerCoordinates.y > 0 && playerCoordinates.x > 0)
        {
            if (Mathf.Abs(playerCoordinates.x) < playerCoordinates.y / 4)
            {
                playerPosition = 0;
            }
            else if (Mathf.Abs(playerCoordinates.x) > playerCoordinates.y / 4 && Mathf.Abs(playerCoordinates.x) < playerCoordinates.y)
            {
                playerPosition = 1;
            }
            else if (Mathf.Abs(playerCoordinates.x) > playerCoordinates.y && Mathf.Abs(playerCoordinates.x) / 4 < playerCoordinates.y)
            {
                playerPosition = 2;
            }
            else if (Mathf.Abs(playerCoordinates.x) / 4 > playerCoordinates.y)
            {
                playerPosition = 3;
            }
        }
        else if (playerCoordinates.y < 0 && playerCoordinates.x > 0)
        {
            if (Mathf.Abs(playerCoordinates.x) / 4 > Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 3;
            }
            else if (Mathf.Abs(playerCoordinates.x) > Mathf.Abs(playerCoordinates.y) &&
                     Mathf.Abs(playerCoordinates.x) / 4 < Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 4;
            }
            else if (Mathf.Abs(playerCoordinates.x) > Mathf.Abs(playerCoordinates.y) / 4 &&
                     Mathf.Abs(playerCoordinates.x) < Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 5;
            }
            else if (Mathf.Abs(playerCoordinates.x) < Mathf.Abs(playerCoordinates.y) / 4)
            {
                playerPosition = 6;
            }
        }
        else if (playerCoordinates.y < 0 && playerCoordinates.x < 0)
        {
            if (Mathf.Abs(playerCoordinates.x) < Mathf.Abs(playerCoordinates.y) / 4)
            {
                playerPosition = 6;
            }
            else if (Mathf.Abs(playerCoordinates.x) > Mathf.Abs(playerCoordinates.y) / 4 &&
                     Mathf.Abs(playerCoordinates.x) < Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 7;
            }
            else if (Mathf.Abs(playerCoordinates.x) > Mathf.Abs(playerCoordinates.y) &&
                     Mathf.Abs(playerCoordinates.x) / 4 < Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 8;
            }
            if (Mathf.Abs(playerCoordinates.x) / 4 > Mathf.Abs(playerCoordinates.y))
            {
                playerPosition = 9;
            }
        }
        else if(playerCoordinates.y > 0 && playerCoordinates.x < 0)
        {
            if (Mathf.Abs(playerCoordinates.x) < playerCoordinates.y / 4)
            {
                playerPosition = 0;
            }
            else if (Mathf.Abs(playerCoordinates.x) > playerCoordinates.y / 4 && Mathf.Abs(playerCoordinates.x) < playerCoordinates.y)
            {
                playerPosition = 11;
            }
            else if (Mathf.Abs(playerCoordinates.x) > playerCoordinates.y && Mathf.Abs(playerCoordinates.x) / 4 < playerCoordinates.y)
            {
                playerPosition = 10;
            }
            else if (Mathf.Abs(playerCoordinates.x) / 4 > playerCoordinates.y)
            {
                playerPosition = 9;
            }
        }
    }

    private void DefaultCannonPosition() // Используется в анимации
    {
        cannonCurrentState = CANNON_POSITION_9;
    }

    private void DeadSound() // используется в анимации
    {
        cannonAudioSource.PlayOneShot(explosion);
        StartCoroutine(StopDeadSound());
    }

    private IEnumerator StopDeadSound()
    {
        yield return new WaitForSeconds(2.6f);
        cannonAudioSource.Stop();
    }

    public void HitCannonSound()
    {
        if(health.isAlive == true)
            cannonAudioSource.PlayOneShot(hitCannon);
    }
}
