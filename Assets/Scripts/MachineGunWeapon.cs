using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunWeapon : MonoBehaviour
{
    private Weapon weapon;
    
    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player") 
    //    {
    //        weapon.ChangeWeapon(Weapon.MACHINEGUN_WEAPON);
    //        Destroy(gameObject);
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            weapon.ChangeWeapon(Weapon.MACHINEGUN_WEAPON);
            Destroy(gameObject);
        }
    }
}
