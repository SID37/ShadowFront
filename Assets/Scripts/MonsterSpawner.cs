using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float progressiveRate = 1.3f;
    public PlayerController player;
    public float activeMonsterCount = 3;
    public GameObject monster;

    bool started = false;
    private Transform[] spawnPoints;
    private List<MonsterController> activeMonsters = new List<MonsterController>();

    void Start()
    {
        spawnPoints = transform.GetComponentsInChildren<Transform>(false);
    }

    void Update()
    {
        if (!started) return;
        activeMonsters = activeMonsters.Where(monster => monster.state != MonsterController.State.Death).ToList();
        if (activeMonsters.Count() >= activeMonsterCount) return;
        activeMonsterCount *= progressiveRate;

        var validPoints = spawnPoints.Where(point => {
            var direction = (player.transform.position - point.transform.position).normalized;
            var playerForward = player.transform.TransformDirection(Vector3.forward);
            if (Vector3.Dot(playerForward, -direction) < 0.6)
                return true;
            return false;
        }).ToArray();

        if (validPoints.Count() == 0) return;

        while (activeMonsters.Count() < activeMonsterCount)
        {
            var spawnPoint = validPoints[Random.Range(0, validPoints.Count())];
            var instance = Instantiate(monster, spawnPoint.position, Quaternion.identity);
            foreach(var m in instance.GetComponentsInChildren<MonsterController>())
            {
                m.player = player;
                m.state = MonsterController.State.Walk;
                if (Random.Range(0.0f, 1.0f) > 0.95f)
                    m.Hit();
                activeMonsters.Add(m);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player.transform) {
            started = true;
        }
    }
}
