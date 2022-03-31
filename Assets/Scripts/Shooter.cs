using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    [SerializeField] private Transform firePointForwardRight;   
    [SerializeField] private Transform firePointForwardUpRight;  
    [SerializeField] private Transform firePointForwardDownRight;   
    [SerializeField] private Transform firePointUpRight;   
    [SerializeField] private Transform firePointLieingRight;    
    [SerializeField] private Transform firePointInWaterForwardRight;    
    [SerializeField] private Transform firePointInWaterForwardUpRight;   
    [SerializeField] private Transform firePointInWaterUpRight;   
    [SerializeField] private Transform firePointDown;
    [SerializeField] private Transform firePointJumpingForwardUpRight;    
    [SerializeField] private Transform firePointJumpingUp;
    private Vector3 firePoint;
    private PlayerMovement playerMovement;
    private float fireDirection;

    [SerializeField] private GameObject basicBullet;
    [SerializeField] private GameObject redBullet;
    //[SerializeField] private GameObject laserBullet;
    [SerializeField] private GameObject flameBullet;
    [SerializeField] private GameObject machineGunBullet;
    private GameObject currentBullet;
    private float shotDelay;
    private string currentWeapon;

    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip standartBulletFire;

    public const string BASIC_WEAPON = "Basic_Weapon";
    public const string MACHINEGUN_WEAPON = "MachineGun_Weapon";
    //public const string LASERGUN_WEAPON = "LaserGun_Weapon";
    public const string FLAME_WEAPON = "Flame_Weapon";
    public const string SPREAD_WEAPON = "Spread_Weapon";

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentBullet = basicBullet;
        currentWeapon = BASIC_WEAPON;
        playerAudioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        GameObject bullet;

        fireDirection = FireDirection();

        string fp = playerMovement.FirePoint();
        
        if (fp != null) 
        {
            firePoint = GetFirePointPosition(fp);

            bullet = Instantiate(currentBullet, firePoint, Quaternion.identity);
            Rigidbody2D currentBulletVelocity = bullet.GetComponent<Rigidbody2D>();

            currentBulletVelocity.velocity = Vector2.ClampMagnitude(GetCurrentBulletVelocity(fp, currentBulletVelocity), fireSpeed);

            playerAudioSource.PlayOneShot(standartBulletFire);
        }       
    }
    private Vector3 GetFirePointPosition(string firePoint) 
    {
        switch (firePoint)
        {
            case "firePointForwardRight":
                return firePointForwardRight.position;
                
            case "firePointForwardUpRight":
                return firePointForwardUpRight.position;

            case "firePointForwardDownRight":
                return firePointForwardDownRight.position;

            case "firePointUpRight":
                return firePointUpRight.position;

            case "firePointLieingRight":
                return firePointLieingRight.position;

            case "firePointInWaterForwardRight":
                return firePointInWaterForwardRight.position;

            case "firePointInWaterForwardUpRight":
                return firePointInWaterForwardUpRight.position;

            case "firePointInWaterUpRight":
                return firePointInWaterUpRight.position;

            case "firePointDown":
                return firePointDown.position;

            case "firePointJumpingForwardUpRight":
                return firePointJumpingForwardUpRight.position;

            case "firePointJumpingUp":
                return firePointJumpingUp.position;

            default:
                return firePointForwardRight.position;
        }
    }

    private Vector2 GetCurrentBulletVelocity(string firePoint, Rigidbody2D currentBulletVelocity) 
    {
        switch (firePoint)
        {
            case "firePointForwardRight":               
                return new Vector2(fireSpeed * fireDirection, currentBulletVelocity.velocity.y);

            case "firePointForwardUpRight":
                return new Vector2(fireSpeed * fireDirection, fireSpeed);

            case "firePointForwardDownRight":
                return new Vector2(fireSpeed * fireDirection, -fireSpeed);

            case "firePointUpRight":
                return new Vector2(0, fireSpeed);

            case "firePointLieingRight":
                return new Vector2(fireSpeed * fireDirection, currentBulletVelocity.velocity.y);
            
            case "firePointInWaterForwardRight":
                return new Vector2(fireSpeed * fireDirection, currentBulletVelocity.velocity.y);

            case "firePointInWaterForwardUpRight":
                return new Vector2(fireSpeed * fireDirection, fireSpeed);

            case "firePointInWaterUpRight":
                return new Vector2(0, fireSpeed);

            case "firePointDown":
                return new Vector2(0, -fireSpeed);

            case "firePointJumpingForwardUpRight":
                return new Vector2(fireSpeed * fireDirection, fireSpeed);

            case "firePointJumpingUp":
                return new Vector2(0, fireSpeed);

            default:
                return new Vector2(fireSpeed * fireDirection, currentBulletVelocity.velocity.y);
        }
    }

    private void ChangeWeapon()
    {
        switch (currentWeapon)
        {
            case BASIC_WEAPON:
                currentBullet = basicBullet;
                shotDelay = 0.2f;
                break;

            case MACHINEGUN_WEAPON:
                currentBullet = machineGunBullet;
                shotDelay = 0.1f;
                break;

            //case LASERGUN_WEAPON:
            //    currentBullet = laserBullet;
            //    shotDelay = 0.2f;
            //    break;

            case FLAME_WEAPON:
                currentBullet = flameBullet;
                shotDelay = 0.5f;
                break;

            case SPREAD_WEAPON:
                currentBullet = redBullet;
                shotDelay = 0.1f;
                break;

            default:
                currentBullet = basicBullet;
                shotDelay = 0.2f;
                break;
        }
    }

    private GameObject GetCurrentBullet()
    {
        return currentBullet;
    }

    //public void SpreadGunShoot()
    //{
    //    GameObject[] redBullets = new GameObject[5];

    //    basicBullet = GetCurrentBullet();

    //    fireDirection = FireDirection();

    //    string fp = playerMovement.FirePoint();

    //    firePoint = GetFirePointPosition(fp);

    //    Vector2 angleY = new Vector2(0, -3.0f);

    //    Vector2 changeAngleY = new Vector2(0, 1.5f);

    //    for (int i = 0; i < redBullets.Length; i++)
    //    {                        
    //        redBullets[i] = Instantiate(currentBullet, firePoint, Quaternion.identity);
    //        Rigidbody2D currentBulletVelocity = redBullets[i].GetComponent<Rigidbody2D>();

    //        currentBulletVelocity.velocity = Vector2.ClampMagnitude(GetCurrentBulletVelocity(fp, currentBulletVelocity), fireSpeed);

    //        angleY += changeAngleY;

    //        Debug.Log("Bullet: " + i + "  x " + currentBulletVelocity.velocity.x + "  y " + currentBulletVelocity.velocity.y);
    //    }        
    //}

    private int FireDirection()
    {
        if (playerMovement.IsLookRight()) // определяем направление вылета пули
            return 1;
        else
            return -1;
    }
}
