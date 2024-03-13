using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Explosion : MonoBehaviour
{
    public float ttl = 0.2f;
    Material material;
    float startTime;

    void Start()
    {
        startTime = Time.time;
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        int uvCount = 4;
        float dt = Time.time - startTime;
        material.SetInteger(Shader.PropertyToID("_UVLayer"), (int)((dt / ttl) * (uvCount + 1)));

        if (dt > ttl)
            Destroy(gameObject);
    }
}
