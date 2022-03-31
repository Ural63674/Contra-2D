using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] private Transform titleScreen;
    [SerializeField] private Rigidbody2D titleRB;
    [SerializeField] private GameObject startButton;

    private int count;
    private int maxCount = 3;

    private void Awake()
    {
        titleRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        titleRB.AddForce(Vector2.left, ForceMode2D.Impulse);

        if(count >= maxCount)
        {
            titleScreen.position = new Vector2(400, 300);
            startButton.SetActive(true);           
        }

        if (Input.anyKeyDown)
        {            
            count = maxCount;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        count++;
    }
}
