using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterController : MonoBehaviour
{
    public float walkSpeed = 3;
    public float runSpeed = 10;
    public Animation anim;
    public NavMeshAgent agent;
    public PlayerController player;
    public float updateRate = 0.1f;
    public AudioSource audioSource;
    public AudioClip runClip;
    public GameObject killObject;
    public float playerTriggerDistance = 20;

    Collider monsterCollider;
    float updateTime;
    public State state = State.Idle;

    public enum State
    {
        Idle,
        Walk,
        Run,
        Death,
    }

    void Start()
    {
        monsterCollider = GetComponent<Collider>();
        updateTime = Time.time + updateRate;

        anim["Run"].weight = 1;
        anim["Run"].speed = 0;
        anim["Run"].normalizedTime = Random.Range(0.0f, 1.0f);
        anim["Idle"].weight = 1;
        anim["Idle"].speed = 0;
        anim["Idle"].normalizedTime = Random.Range(0.0f, 1.0f);
    }

    void Update()
    {
        if (player.Hp <= 0)
        {
            audioSource.mute = true;
            return;
        }
        switch (state)
        {
            case State.Idle:
                agent.speed = 0;
            break;
            case State.Walk:
                agent.speed = walkSpeed;
                agent.destination = player.transform.position;
            break;
            case State.Run:
                agent.speed = runSpeed;
                agent.destination = player.transform.position;
            break;
            case State.Death:
                agent.speed = 0;
            break;
        }

        // if (state == State.Run && Vector3.Distance(agent.destination, agent.transform.position) < 1.0f)
        //     Hit();

        if (updateTime < Time.time) {
            UpdateState();
        }
    }

    void UpdateState()
    {

        if (state == State.Idle && Vector3.Distance(transform.position, player.transform.position) < playerTriggerDistance)
        {
            state = State.Walk;
        }

        updateTime = Time.time + updateRate;

        switch (state)
        {
            case State.Idle:
                anim.Play("Idle");
                IncAnimation("Idle", 0.2f);
            break;
            case State.Walk:
                anim.Play("Run");
                IncAnimation("Run", 0.2f);
            break;
            case State.Run:
                anim.Play("Run");
                IncAnimation("Run", 0.4f);
            break;
            case State.Death:
                anim.Play("Lie");
                anim["Lie"].normalizedTime = 1.0f;
            break;
        }

        transform.position = agent.transform.position;
        transform.rotation = agent.transform.rotation;
    }

    private void IncAnimation(string animation, float value)
    {
        anim[animation].normalizedTime += value;
        if (anim[animation].normalizedTime > 1)
            anim[animation].normalizedTime -= 1;
    }

    public void Hit()
    {
        switch (state)
        {
            case State.Idle:  state = State.Run;   break;
            case State.Walk:  state = State.Run;   break;
            case State.Run:   state = State.Death; break;
            case State.Death: state = State.Death; break;
        }

        if (state == State.Run) {
            audioSource.clip = runClip;
            audioSource.Play();
        }

        if (state == State.Death) {
            Destroy(killObject.gameObject, 30);
            monsterCollider.enabled = false;
            audioSource.Stop();
        }
        UpdateState();
    }
}
