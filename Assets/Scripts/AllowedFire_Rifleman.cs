using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowedFire_Rifleman : MonoBehaviour
{
    private bool isFireAllowed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isFireAllowed = true;
        }
    }

    public bool IsFireAllowed()
    {
        return isFireAllowed;
    }
}
