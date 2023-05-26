using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NimbySpawner : MonoBehaviour
{
    public Transform player;
    public Nimby nimbyPrefab;
    public int numberOfNimbys = 10;
    private float startDelayNimby = 5.0f;
    private float spawnIntervalNimby = 15.0f;
    private float spawnRange = 24.0f;
    private ObjectPool nimbyPool;

    private NavMeshTriangulation triangulation;

    // Set up the NIMBY spawn pool and player reference
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nimbyPool = ObjectPool.CreateInstance(nimbyPrefab, numberOfNimbys);
    }

    // Spawn NIMBYs on a repeating interval when the game is started
    public void StartSpawn()
    {
        triangulation = NavMesh.CalculateTriangulation();
        InvokeRepeating("SpawnNimby", startDelayNimby, spawnIntervalNimby);
    }

    // Spawn NIMBYs randomly into the play area and start chasing them using the function in the Nimby class
    void SpawnNimby()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            PoolableObject instance = nimbyPool.GetObject();

            if (instance != null)
            {
                Nimby nimby = instance.GetComponent<Nimby>();
                
                int vertextIndex = Random.Range(0, triangulation.vertices.Length);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(triangulation.vertices[vertextIndex], out hit, 2f, 1))
                {
                    nimby.agent.enabled = true;
                    nimby.agent.Warp(hit.position);
                    nimby.movement.player = player;
                    nimby.movement.StartChasing();
                }
            }
        }
    }
}