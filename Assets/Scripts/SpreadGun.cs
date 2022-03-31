using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireSpeed;
    [SerializeField] private Transform firePointForwardRight;
    private Vector3 firePoint;
    private PlayerMovement playerMovement;
    private float fireDirection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Shoot()
    {
        GameObject currentBullet;

        if (playerMovement.IsLookRight()) // определяем направление вылета пули
            fireDirection = fireSpeed * 1;
        else
            fireDirection = fireSpeed * -1;

        string fp = playerMovement.FirePoint();

        if (fp != null)
        {
            //firePoint = GetFirePointPosition(fp);

            currentBullet = Instantiate(bullet, firePoint, Quaternion.identity);
            Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

            //currentBulletVelocity.velocity = Vector2.ClampMagnitude(GetCurrentBulletVelocity(fp, currentBulletVelocity), fireSpeed);
        }
    }
}
