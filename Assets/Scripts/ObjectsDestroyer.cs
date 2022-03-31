using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDestroyer : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet")
            /*|| collision.gameObject.CompareTag("Hiding Enemy")*/)
        {
            Destroy(collision.gameObject);
        }
    }
}
