using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public RectTransform[] angles;
    Vector2[] vectors;
    public float radius = 0;

    void Start()
    {
        vectors = angles.Select(a =>
            new Vector2(Mathf.Sign(a.anchoredPosition.x), Mathf.Sign(a.anchoredPosition.y))
        ).ToArray();
    }

    void Update()
    {
        for (int i = 0; i < angles.Count(); ++i) {
            angles[i].anchoredPosition = vectors[i] * radius;
        }
    }
}
