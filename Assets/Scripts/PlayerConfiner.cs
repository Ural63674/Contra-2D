using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfiner : MonoBehaviour
{
    public GameObject mainCamera;
    [SerializeField] private Vector3 offset;
    
    void Start()
    {

        offset = new Vector3(offset.x, offset.y, 0);
    }

    void Update()
    {
        transform.position = mainCamera.gameObject.transform.position + offset;
    }
}
