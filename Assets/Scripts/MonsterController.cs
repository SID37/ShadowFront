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
    public Transform player;
    public float updateRate = 0.1f;

    Collider monsterCollider;
    float updateTime;
    State state = State.Walk;

    enum State
    {
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
        var asd = anim.Play("Run");
    }

    void Update()
    {
        switch (state)
        {
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
        updateTime = Time.time + updateRate;

        if (state != State.Death)
            anim["Run"].normalizedTime = Random.Range(0.0f, 1.0f);
        else
        {
            var asd = anim.Play("Lie");
            anim["Lie"].normalizedTime = 1.0f;
        }

        transform.position = agent.transform.position;
        transform.rotation = agent.transform.rotation;
    }

    public void Hit()
    {
        switch (state)
        {
            case State.Walk:  state = State.Run;   break;
            case State.Run:   state = State.Death; break;
            case State.Death: state = State.Death; break;
        }
        if (state == State.Death) {
            monsterCollider.enabled = false;
        }
        UpdateState();
    }
}
