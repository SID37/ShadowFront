using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float mouseSpeed = 1;
    public float aimSpeed = 1;
    public float aimRotation = 1;
    public float aimWalk = 1;
    public float cameraRotationMin = -40;
    public float cameraRotationMax = 40;
    public Camera targetCamera;
    public Aim aim;
    public Gun gun;
    public GameObject explosion;
    public AudioSource fireSound;
    public AudioSource walkSound;
    public AudioSource hitSound;
    public RawImage hitLeft;
    public RawImage hitRight;
    public float hitDelay = 0.5f;
    public int Hp = 5;

    private Rigidbody body;
    private Vector3 cameraRotation;
    private float lastHit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<Rigidbody>();
        cameraRotation = Vector3.left * Vector3.Dot(targetCamera.transform.localRotation.eulerAngles, Vector3.left);
        lastHit = Time.time;
        hitLeft.color = new Color(1, 1, 1, 0);
        hitRight.color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        float addHitColor = -Time.deltaTime * 0.1f;
        AddHitColor(addHitColor, addHitColor);

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
            aim.radius += 0.1f;
            mouseY += 1.0f / mouseSpeed;
            gun.AddRotation(new Vector3(-30, 0, -20));
        }

        aim.radius -= aimSpeed * Time.deltaTime;

        aim.radius += aimRotation * mouseSpeed * new Vector2(mouseX, mouseY).magnitude;
        aim.radius += aimWalk * Time.deltaTime * new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).magnitude;
        if (aim.radius >= 0.3f) aim.radius = 0.3f;
        if (aim.radius <= 0) aim.radius = 0;
        gun.amplitude = aim.radius;

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles +
            Vector3.up * mouseX * mouseSpeed
        );

        cameraRotation += Vector3.left * mouseY * mouseSpeed;
        cameraRotation = Vector3.left * Mathf.Clamp(Vector3.Dot(cameraRotation, Vector3.left), cameraRotationMin, cameraRotationMax);
        targetCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
    }

    void FixedUpdate()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        walkSound.mute = Mathf.Abs(vertical) + Mathf.Abs(horizontal) < 0.01;

        body.velocity =
            Vector3.Normalize(
                transform.forward * vertical +
                transform.right   * horizontal
            ) * speed +
            Vector3.down * Vector3.Dot(body.velocity, Vector3.down);
    }

    void Fire()
    {
        float k = targetCamera.fieldOfView * aim.radius;
        var rotation = Quaternion.Euler(
            Random.Range(-k, k),
            Random.Range(-k, k),
            0
        );
        Vector3 position = targetCamera.transform.position;
        Vector3 direction = targetCamera.transform.TransformDirection(rotation * Vector3.forward);

        fireSound.Play();

        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, Mathf.Infinity))
        {
            Instantiate(explosion, hit.point, Quaternion.LookRotation(-direction));
            if (hit.collider != null)
            {
                var monster = hit.collider.GetComponent<MonsterController>();
                if (monster != null) monster.Hit();
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (lastHit + hitDelay < Time.time)
        {
            if (collision.collider.GetComponent<MonsterController>() != null)
            {
                lastHit = Time.time;
                hitSound.Play();
                Hp -= 1;

                var targetVector = targetCamera.transform.InverseTransformDirection(collision.collider.transform.position - transform.position).normalized;
                float k = Vector3.Dot(Vector3.left, targetVector) * 0.5f;
                AddHitColor((0.5f + k) * 0.1f, (0.5f - k) * 0.1f);

                if (Hp <= 0) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    void AddHitColor(float left, float right)
    {
        hitLeft.color  = new Color(1, 1, 1, Mathf.Clamp(hitLeft.color.a  + left, 0, 1));
        hitRight.color = new Color(1, 1, 1, Mathf.Clamp(hitRight.color.a + right, 0, 1));
    }
}
