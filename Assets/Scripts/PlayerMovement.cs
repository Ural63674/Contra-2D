using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerLegsCollider;
    [SerializeField] private float jumpOffset;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;
    [SerializeField] private Transform playerInstantiate;
    [SerializeField] private float checkGroundDistance;
    private bool isGrounded;
    private Animator anim;
    private SpriteRenderer sp;

    public bool isPlayerLieing;
    private bool isJumping;
    private bool inWater;
    private bool isDownInWater; // момент попадания в воду 
    [SerializeField] private float jumpOutFromWater;

    public const int PLAYER_1_IDLE_STATE = 0;
    public const int PLAYER_1_LOOK_UP_STATE = 1;
    public const int PLAYER_1_LIEING_STATE = 2;
    public const int PLAYER_1_RUNNING_STATE = 3;
    public const int PLAYER_1_JUMPING_STATE = 4;
    public const int PLAYER_1_RUN_FIRE_STATE = 5;
    public const int PLAYER_1_DEAD_STATE = 6;
    public const int PLAYER_1_FORWARD_UP_MOVE_STATE = 7;
    public const int PLAYER_1_FORWARD_DOWN_MOVE_STATE = 8;
    public const int PLAYER_1_DOWN_IN_WATER_STATE = 9;  // момент падения в воду
    public const int PLAYER_1_UNDER_WATER_STATE = 10;
    public const int PLAYER_1_IN_WATER_STATE = 11;
    public const int PLAYER_1_FIRE_FORWARD_IN_WATER_STATE = 12;
    public const int PLAYER_1_FIRE_FORWARD_UP_IN_WATER_STATE = 13;
    public const int PLAYER_1_FIRE_UP_IN_WATER_STATE = 14;
    public const int PLAYER_1_FALLING_STATE = 15;

    public int currentState;
    private bool isLookRight;
    private string firePoint;

    private Health health;

    [SerializeField] private BoxCollider2D playerBoxCollider;
    [SerializeField] private GameObject playerLegCollider;
    //[SerializeField] private GameObject playerBodyCollider;

    [SerializeField] private float deadOffset; // смещение при смерти персонажа

    private bool playerRespawn = false;
    private float transparentValue;
    private float currentTime = 0;
    private bool colorAlphaNull = false;

    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip landingAudio;
    [SerializeField] private AudioClip playerDead;

    private int boxColliderState;
    private const int standingColliderState = 1;
    private const int jumpingColliderState = 2;
    private const int inWaterColliderState = 3;
    private const int underWaterColliderState = 4;
    private const int lieingColliderState = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = PLAYER_1_IDLE_STATE;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        isPlayerLieing = false;
        isJumping = false;
        inWater = false;
        isDownInWater = false;
        isLookRight = true;
        health = GetComponent<Health>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {        
        switch (currentState) 
        {           
            case PLAYER_1_IDLE_STATE:
                anim.SetInteger("state", 0);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_LOOK_UP_STATE:
                anim.SetInteger("state", 1);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_LIEING_STATE:
                anim.SetInteger("state", 2);
                isPlayerLieing = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                boxColliderState = lieingColliderState;
                break;
            case PLAYER_1_RUNNING_STATE:
                anim.SetInteger("state", 3);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_JUMPING_STATE:
                anim.SetInteger("state", 4);
                anim.SetBool("onGround", isGrounded);
                boxColliderState = jumpingColliderState;
                break;
            case PLAYER_1_RUN_FIRE_STATE:
                anim.SetInteger("state", 5);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_DEAD_STATE:
                anim.SetInteger("state", 6);
                break;
            case PLAYER_1_FORWARD_UP_MOVE_STATE:
                anim.SetInteger("state", 7);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_FORWARD_DOWN_MOVE_STATE:
                anim.SetInteger("state", 8);
                boxColliderState = standingColliderState;
                break;
            case PLAYER_1_DOWN_IN_WATER_STATE:
                anim.SetInteger("state", 9);
                isDownInWater = true;
                boxColliderState = inWaterColliderState;
                break;
            case PLAYER_1_UNDER_WATER_STATE:
                anim.SetInteger("state", 10);
                isPlayerLieing = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                boxColliderState = underWaterColliderState;
                break;
            case PLAYER_1_IN_WATER_STATE:
                anim.SetInteger("state", 11);
                boxColliderState = inWaterColliderState;
                break;
            case PLAYER_1_FIRE_FORWARD_IN_WATER_STATE:
                anim.SetInteger("state", 12);
                boxColliderState = inWaterColliderState;
                break;
            case PLAYER_1_FIRE_FORWARD_UP_IN_WATER_STATE:
                anim.SetInteger("state", 13);
                boxColliderState = inWaterColliderState; 
                break;
            case PLAYER_1_FIRE_UP_IN_WATER_STATE:
                anim.SetInteger("state", 14);
                boxColliderState = inWaterColliderState;
                break;
            case PLAYER_1_FALLING_STATE:
                anim.SetInteger("state", 15);
                boxColliderState = standingColliderState;
                if (isGrounded == true)
                    currentState = PLAYER_1_IDLE_STATE;
                break;
        }

        switch (boxColliderState)
        {
            case standingColliderState:
                playerBoxCollider.size = new Vector2(0.109f, 0.26f);
                playerBoxCollider.offset = new Vector2(0, 0.17f);
                break;
            case jumpingColliderState:
                playerBoxCollider.size = new Vector2(0.109f, 0.13f);
                playerBoxCollider.offset = new Vector2(0, 0.11f);
                break;
            case inWaterColliderState:
                playerBoxCollider.size = new Vector2(0.109f, 0.08f);
                playerBoxCollider.offset = new Vector2(0, 0.11f);
                playerBoxCollider.enabled = true;
                break;
            case underWaterColliderState:
                playerBoxCollider.enabled = false;
                break;
            case lieingColliderState:
                playerBoxCollider.size = new Vector2(0.32f, 0.13f);
                playerBoxCollider.offset = new Vector2(0, 0.06f);
                break;
        }

        if (health.isAlive == false)
        {
            PlayerDead();           
        }

        //if (isJumping == true)
        //{
        //    playerBoxCollider.size = new Vector2(0.109f, 0.13f);
        //    playerBoxCollider.offset = new Vector2(0, 0.11f);
        //}
        //else if (isJumping == false && isGrounded == true)
        //{
        //    playerBoxCollider.size = new Vector2(0.109f, 0.26f);
        //    playerBoxCollider.offset = new Vector2(0, 0.17f);
        //}

        //if (inWater == true && isPlayerLieing == false)
        //{
        //    playerBoxCollider.size = new Vector2(0.109f, 0.08f);
        //    playerBoxCollider.offset = new Vector2(0, 0.11f);
        //    playerBoxCollider.enabled = true;
        //}
        //else if (inWater == true && isPlayerLieing == true)
        //{
        //    playerBoxCollider.enabled = false;
        //}

        //if (isPlayerLieing == true)
        //{
        //    playerBoxCollider.size = new Vector2(0.32f, 0.13f);
        //    playerBoxCollider.offset = new Vector2(0, 0.06f);
        //}
        //else if (isPlayerLieing == false)
        //{
        //    playerBoxCollider.size = new Vector2(0.109f, 0.26f);
        //    playerBoxCollider.offset = new Vector2(0, 0.17f);
        //}

        if (playerRespawn == true)
        {
            float maxTime = 0.5f;
            currentTime += Time.deltaTime;
            //playerBoxCollider.enabled = false;
            playerLegCollider.layer = 10; //layer DeadPlayer
            player.layer = 10; //layer DeadPlayer
            //playerBodyCollider.layer = 10; //layer DeadPlayer

            if (currentTime >= maxTime)
            {
                colorAlphaNull = !colorAlphaNull;
                currentTime = 0;
            }

            if(colorAlphaNull == false)
            {
                transparentValue = (maxTime - currentTime) / maxTime;
                sp.color = new Color(1, 1, 1, transparentValue);
            }
            else
            {
                transparentValue = currentTime / maxTime;
                sp.color = new Color(1, 1, 1, transparentValue);
            }
        }       
    }    

    private void FixedUpdate()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(playerLegsCollider.position, Vector2.down, checkGroundDistance, groundMask);
        if(groundInfo.collider == true)
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    public void Move(float directionX, float directionY, bool isJumpButtonPressed, bool isFireButtonPressed)
    {
        if (isJumpButtonPressed == true && isPlayerLieing == false && isJumping == false)
        {
            Jump();
        }
        anim.SetBool("onGround", isGrounded);

        if (Mathf.Abs(directionX) > 0.01f && isPlayerLieing == false)
            HorizontalMovement(directionX);

        Reverse(directionX);

        ChangeState(directionX, directionY, isFireButtonPressed);
        CurrentFirePoint(directionX, directionY);

        if (currentState == PLAYER_1_LIEING_STATE && isJumpButtonPressed == true)
            JumpDown();
    }

    public void Jump() 
    {
        if (isGrounded == true && isPlayerLieing == false) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            currentState = PLAYER_1_JUMPING_STATE;
            isJumping = true;
        }
    }

    private void HorizontalMovement(float direction) 
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        
        if (direction > 0)
            isLookRight = true;
        else if (direction < 0)
            isLookRight = false;
    }

    private void Reverse(float direction) 
    {
        if (direction > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        else if (direction < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    private void ChangeState(float directionX, float directionY, bool isFireButtonPressed) 
    {
        if (isGrounded && isJumping == false)
        {
            if (directionX == 0 && directionY == 0)
                currentState = PLAYER_1_IDLE_STATE;
            else if (directionX > 0 && directionY == 0 || directionX < 0 && directionY == 0)
                currentState = PLAYER_1_RUNNING_STATE;
            else if (directionX > 0 && directionY > 0 || directionX < 0 && directionY > 0)
                currentState = PLAYER_1_FORWARD_UP_MOVE_STATE;
            else if (directionX == 0 && directionY > 0)
                currentState = PLAYER_1_LOOK_UP_STATE;

            else if (directionX == 0 && directionY < 0 && isGrounded == true)
                currentState = PLAYER_1_LIEING_STATE;
            else if (directionX > 0 && directionY < 0 || directionX < 0 && directionY < 0)
                currentState = PLAYER_1_FORWARD_DOWN_MOVE_STATE;
        }

        if (currentState != PLAYER_1_LIEING_STATE && currentState != PLAYER_1_UNDER_WATER_STATE)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (inWater == true && isDownInWater == false) 
        {
            if (directionX == 0 && directionY == 0)
                currentState = PLAYER_1_IN_WATER_STATE;
            else if (directionY < 0)
                currentState = PLAYER_1_UNDER_WATER_STATE;
            else if (directionX > 0 && directionY == 0 || directionX < 0 && directionY == 0)
                currentState = PLAYER_1_IN_WATER_STATE;
            else if (directionX > 0 && directionY > 0 && isFireButtonPressed == true || 
                     directionX < 0 && directionY > 0 && isFireButtonPressed == true) 
            {
                currentState = PLAYER_1_FIRE_FORWARD_UP_IN_WATER_STATE;
                anim.SetBool("isFireFromWater", true);
            }
                
            else if (directionX == 0 && directionY > 0 && isFireButtonPressed == true) 
            {
                currentState = PLAYER_1_FIRE_UP_IN_WATER_STATE;
                anim.SetBool("isFireFromWater", true);
            }
            else if(currentState == PLAYER_1_IN_WATER_STATE && isFireButtonPressed == true) 
            {
                currentState = PLAYER_1_FIRE_FORWARD_IN_WATER_STATE;
                anim.SetBool("isFireFromWater", true);
            }
                
        }

        if (isFireButtonPressed && currentState == PLAYER_1_RUNNING_STATE || 
            isFireButtonPressed && currentState == PLAYER_1_RUN_FIRE_STATE)
        {
            currentState = PLAYER_1_RUN_FIRE_STATE;
            anim.SetBool("isRunningStateActive", true);
            StopCoroutine(RunningFireState());
            StartCoroutine(RunningFireState());
        }

        if (isFireButtonPressed && inWater == true)
        {
            if (directionX == 0 && directionY == 0 || directionX > 0 && directionY == 0 || directionX < 0 && directionY == 0)
            {
                currentState = PLAYER_1_FIRE_FORWARD_IN_WATER_STATE;
                anim.SetBool("isFireFromWater", true);
            }    
        }

        if(currentState == PLAYER_1_JUMPING_STATE)
        {
            if(rb.velocity.y < 0 && isGrounded == true)
            {
                currentState = PLAYER_1_IDLE_STATE;
                isJumping = false;
                playerAudioSource.PlayOneShot(landingAudio);
            }
        }
        if (isJumping == false && isGrounded == false && rb.velocity.y < 0)
        {
            currentState = PLAYER_1_FALLING_STATE;
        }
    }

    //private void EndJump() // триггер для анимации прыжка
    //{
    //    currentState = PLAYER_1_IDLE_STATE;
    //    isJumping = false;
    //}

    private void JumpDown() // спрыгивание вниз через платформу
    {       
        transform.position = transform.position - (Vector3.up * 0.15f); // смещение игрока вниз при спрыгивании с платформы       
        currentState = PLAYER_1_FALLING_STATE;
        isGrounded = false;
        isPlayerLieing = false;
    }

    private void InWaterTrigger() //триггер для анимации падения в воду
    {
        currentState = PLAYER_1_IN_WATER_STATE;
        isDownInWater = false;
    }

    private void DownInWater() 
    {
        currentState = PLAYER_1_DOWN_IN_WATER_STATE;
        inWater = true;
    }

    private void JumpOutFromWater() 
    {        
        rb.velocity = new Vector2(rb.velocity.x, jumpOutFromWater);
        inWater = false;
        currentState = PLAYER_1_RUNNING_STATE;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            DownInWater();
        }

        if(collision.gameObject.tag == "Coast") 
        {
            JumpOutFromWater();
        }
    }    
    
    private string CurrentFirePoint(float directionX, float directionY) 
    {
        if (currentState == PLAYER_1_JUMPING_STATE) 
        {
            if (directionY == 0)
                firePoint = "firePointForwardRight";

            else if (directionX != 0 && directionY > 0)
                firePoint = "firePointJumpingForwardUpRight";

            else if (directionX == 0 && directionY > 0)
                firePoint = "firePointJumpingUp";

            else if (directionX != 0 && directionY < 0)
                firePoint = "firePointForwardDownRight";

            else if (directionX == 0 && directionY < 0)
                firePoint = "firePointDown";

            else
                firePoint = null;
        }

        else if (currentState == PLAYER_1_IDLE_STATE)
            firePoint = "firePointForwardRight";

        else if (currentState == PLAYER_1_LOOK_UP_STATE)
            firePoint = "firePointUpRight";

        else if (currentState == PLAYER_1_LIEING_STATE)
            firePoint = "firePointLieingRight";

        else if (currentState == PLAYER_1_RUN_FIRE_STATE)
            firePoint = "firePointForwardRight";

        else if (currentState == PLAYER_1_FORWARD_UP_MOVE_STATE)
            firePoint = "firePointForwardUpRight";

        else if (currentState == PLAYER_1_FORWARD_DOWN_MOVE_STATE)
            firePoint = "firePointForwardDownRight";

        else if (currentState == PLAYER_1_FALLING_STATE)
            firePoint = "firePointForwardRight";

        else if (currentState == PLAYER_1_FIRE_FORWARD_IN_WATER_STATE)
            firePoint = "firePointInWaterForwardRight";

        else if (currentState == PLAYER_1_FIRE_FORWARD_UP_IN_WATER_STATE)
            firePoint = "firePointInWaterForwardUpRight";

        else if (currentState == PLAYER_1_FIRE_UP_IN_WATER_STATE)
            firePoint = "firePointInWaterUpRight";

        else
            firePoint = null;

        return firePoint;

    }

    public bool IsLookRight() 
    {
        if (isLookRight)
            return true;
        else
            return false;
    }  

    public string FirePoint() 
    {
        return firePoint;
    }
    private IEnumerator RunningFireState() // задержка возврата из анимации стрельбы в анимацию движения
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("isRunningStateActive", false);
    }

    //private IEnumerator FireFromWater() 
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    anim.SetBool("isFireFromWater", false);
    //}
    private void FireFromWater() // триггер для анимации выстрела из воды
    {
        anim.SetBool("isFireFromWater", false);
    }

    private void PlayerDead()
    {
        currentState = PLAYER_1_DEAD_STATE;
        anim.SetBool("isAlive", false);
        rb.simulated = false;

        transform.Translate(Vector3.left * deadOffset * Time.deltaTime);
    }

    private void DeadPlayerDontMove() // используется в анимации
    {
        deadOffset = 0;
        rb.velocity = Vector2.zero;
    }

    private void PlayerRespawn() // используется в анимации
    {
        playerRespawn = true;
        gameObject.transform.position = playerInstantiate.position;
        anim.SetBool("isAlive", true);
        inWater = false;
        health.isAlive = true;
        health.currentHealth = 100;
        rb.simulated = true;
        StartCoroutine(BlinkTime());
    }

    private IEnumerator BlinkTime()
    {
        yield return new WaitForSeconds(3f);
        playerRespawn = false;
        sp.color = new Color(1, 1, 1, 1);
        //playerBoxCollider.enabled = true;
        playerLegCollider.layer = 3; //layer Player
        player.layer = 3; //layer Player
        //playerBodyCollider.layer = 0; //layer Default
    }

    private void PlayerDeadSound() // используется в анимации
    {
        playerAudioSource.PlayOneShot(playerDead);
    }
}
