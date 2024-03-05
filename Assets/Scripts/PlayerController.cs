using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float cameraRotationMin = -40;
    public float cameraRotationMax = 40;
    public Transform targetCamera;

    private Rigidbody body;
    private Vector3 cameraRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<Rigidbody>();
        cameraRotation = Vector3.left * Vector3.Dot(targetCamera.localRotation.eulerAngles, Vector3.left);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles +
            Vector3.up * Input.GetAxis("Mouse X")
        );

        cameraRotation += Vector3.left * Input.GetAxis("Mouse Y");
        cameraRotation = Vector3.left * Mathf.Clamp(Vector3.Dot(cameraRotation, Vector3.left), cameraRotationMin, cameraRotationMax);
        targetCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void FixedUpdate()
    {
        body.velocity =
            Vector3.Normalize(
                transform.forward * Input.GetAxis("Vertical") +
                transform.right   * Input.GetAxis("Horizontal")
            ) * speed +
            Vector3.down * Vector3.Dot(body.velocity, Vector3.down);
    }
}
