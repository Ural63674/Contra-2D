using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject[] bridgeSection;
    [SerializeField] private GameObject[] gridColliderBridge;
    [SerializeField] private GameObject[] bridgeSectionsExplosion;

    [SerializeField] private AudioSource bridge;
    [SerializeField] private AudioClip bridgeExplosion;

    private float timeToNextExplosion = 0.1f;
    private float currentTime = 0;
    private float timeToNextSectionExplosion = 1.0f;
    private float currentTimeToNextSectionExplosion = 0.9f;

    private bool startExplosion = false;
    private bool startNextExplosion = false;

    private int j = 0;
    private int i = 0;
    private int maxExplosionNumber = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startExplosion = true;
        }
    }
   
    private void Update()
    {
        if (startExplosion)
        {                        
            currentTimeToNextSectionExplosion += Time.deltaTime;
            

            if (currentTimeToNextSectionExplosion >= timeToNextSectionExplosion && i < bridgeSection.Length)
            {
                bridge.PlayOneShot(bridgeExplosion);
                bridgeSection[i].SetActive(false);
                gridColliderBridge[i].SetActive(false);
               
                i++;
                maxExplosionNumber += bridgeSection.Length;
                startNextExplosion = true;
                currentTimeToNextSectionExplosion = 0;
            }           
        }

        if (startNextExplosion)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeToNextExplosion && j < maxExplosionNumber)
            {
                bridgeSectionsExplosion[j].SetActive(true);
                j++;
                if (j >= maxExplosionNumber)
                    startNextExplosion = false;

                currentTime = 0;
            }
        }       
    }   
}
