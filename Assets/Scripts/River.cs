using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : MonoBehaviour
{
    public float period = 0.3f;
    Material material;
    float timer = 0;
    int layerCounter = 0;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= period)
        {
            layerCounter = (layerCounter + 1) % 4;
            material.SetInteger(Shader.PropertyToID("_UVLayer"), layerCounter);
            timer -= period;
        }
    }
}
