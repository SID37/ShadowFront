using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Vector3 startRotation;
    public float amplitudeK = 50;
    public float amplitude;
    Vector3 addRotation = new Vector3();

    float time = 0;

    void Start()
    {
        startRotation = transform.localRotation.eulerAngles;
    }

    public void AddRotation(Vector3 angles)
    {
        addRotation += angles;
        float border = 60;
        addRotation.x = Mathf.Clamp(addRotation.x, -border, border);
        addRotation.y = Mathf.Clamp(addRotation.y, -border, border);
        addRotation.z = Mathf.Clamp(addRotation.z, -border, border);
        time = 0.1f;
        Update();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 0.1f)
        {
            transform.localRotation = Quaternion.Euler(
                startRotation +
                addRotation +
                new Vector3(
                    amplitude * Mathf.Sin(Time.time * 10) * amplitudeK,
                    0,
                    amplitude * Mathf.Sin(Time.time * 10) * amplitudeK
                )
            );
            addRotation = addRotation * Mathf.Pow(0.1f, time);
            time = 0;
        }
    }
}
