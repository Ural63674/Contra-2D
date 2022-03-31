using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Shooter shooter;
    private Health health;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.isAlive)
        {
            float horizontalMovement = Input.GetAxisRaw(GlobalStringVars.HORIZONTAL_AXIS);
            bool isJumpButtonPressed = Input.GetButtonDown(GlobalStringVars.JUMP);
            float verticalMovement = Input.GetAxisRaw(GlobalStringVars.VERTICAL_AXIS);
            bool isFireButtonPressed = Input.GetButtonDown(GlobalStringVars.FIRE_1);

            playerMovement.Move(horizontalMovement, verticalMovement, isJumpButtonPressed, isFireButtonPressed);

            if (Input.GetAxisRaw(GlobalStringVars.VERTICAL_AXIS) < 0 && Input.GetAxisRaw(GlobalStringVars.HORIZONTAL_AXIS) == 0)
                playerMovement.isPlayerLieing = true;
            else
                playerMovement.isPlayerLieing = false;

            if (isJumpButtonPressed)
                playerMovement.Jump();

            if (isFireButtonPressed)
            {
                shooter.Shoot();
            }
        }
    }
}
