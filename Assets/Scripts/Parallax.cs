using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform[] layers;
    [SerializeField] private float[] coeff;

    private int layersCount;

    // Start is called before the first frame update
    void Start()
    {
        layersCount = layers.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layersCount; i++)
        {
            layers[i].position = transform.position * coeff[i];
        }
    }
}
