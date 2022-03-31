using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject basicBullet;
    [SerializeField] private GameObject redBullet;
    //[SerializeField] private GameObject laserBullet;
    [SerializeField] private GameObject flameBullet;
    [SerializeField] private GameObject machineGunBullet;
    private GameObject currentBullet;
    private float shotDelay;

    public const string BASIC_WEAPON = "Basic_Weapon";
    public const string MACHINEGUN_WEAPON = "MachineGun_Weapon";
    //public const string LASERGUN_WEAPON = "LaserGun_Weapon";
    public const string FLAME_WEAPON = "Flame_Weapon";
    public const string SUPER_WEAPON = "Super_Weapon";
       
    public float GetShotDelay() 
    {
        return shotDelay;
    }

    public void ChangeWeapon(string weapon) 
    {
        switch (weapon)
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
                shotDelay = 0.1f;
                break;

            case SUPER_WEAPON:
                currentBullet = redBullet;
                shotDelay = 0.1f;
                break;

            default:
                currentBullet = basicBullet;
                shotDelay = 0.2f;
                break;
        }
    }

    public GameObject GetCurrentBullet()
    {
        return currentBullet;
    }
}


