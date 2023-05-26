using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script is for all non-NIMBY spawns, including food, tokens, and power ups

public class SpawnManager : MonoBehaviour
{
    // Prefabs
    public Food foodPrefab;
    public Token tokenPrefab;
    public PowerUp powerUpPrefab;

    // Object Pools
    private ObjectPool foodPool;
    private ObjectPool tokenPool;
    private ObjectPool powerUpPool;

    // Spawn range is the NavMesh
    private NavMeshTriangulation triangulation;
    private Vector3 heightAdjustment = new Vector3(0, 1, 0);
        
    // Food
    public int numberOfFoods = 20;
    private float startDelayFood = 5.0f;
    private float spawnIntervalFood = 5.0f;

    // Token
    public int numberOfTokens = 35;
    private float startDelayToken = 1.0f;
    private float spawnIntervalToken = 2.0f;

    // Power up
    public int numberOfPowerUps = 6;
    private float startDelayPowerUp = 5.0f;
    private float spawnIntervalPowerUp = 10.0f;

    // Set up the spawn pools
    private void Awake()
    {
        foodPool = ObjectPool.CreateInstance(foodPrefab, numberOfFoods);
        tokenPool = ObjectPool.CreateInstance(tokenPrefab, numberOfTokens);
        powerUpPool = ObjectPool.CreateInstance(powerUpPrefab, numberOfPowerUps);
    }

    public void StartSpawn()
    {
        // Calculate the NavMesh area for spawning
        triangulation = NavMesh.CalculateTriangulation();

        // Invoke custom functions to spawn prefabs on a timed interval
        InvokeRepeating("SpawnFood", startDelayFood, spawnIntervalFood);
        InvokeRepeating("SpawnToken", startDelayToken, spawnIntervalToken);
        InvokeRepeating("SpawnPowerUp", startDelayPowerUp, spawnIntervalPowerUp);
    }
    
    // Spawn food randomly into the play area
    void SpawnFood()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            PoolableObject instance = foodPool.GetObject();

            if (instance != null)
            {
                Food food = instance.GetComponent<Food>();
                
                int vertextIndex = Random.Range(0, triangulation.vertices.Length);

                UnityEngine.AI.NavMeshHit hit;
                if (NavMesh.SamplePosition(triangulation.vertices[vertextIndex], out hit, 2f, 1))
                {
                    instance.transform.localPosition = hit.position + heightAdjustment;
                    instance.transform.rotation = foodPrefab.transform.rotation;
                    instance.gameObject.SetActive(true);
                }
            }
        }
    }

    // Spawn token randomly into the play area
    void SpawnToken()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            PoolableObject instance = tokenPool.GetObject();

            if (instance != null)
            {
                Token token = instance.GetComponent<Token>();
                
                int vertextIndex = Random.Range(0, triangulation.vertices.Length);

                UnityEngine.AI.NavMeshHit hit;
                if (NavMesh.SamplePosition(triangulation.vertices[vertextIndex], out hit, 2f, 1))
                {
                    instance.transform.localPosition = hit.position + heightAdjustment;
                    instance.transform.rotation = foodPrefab.transform.rotation;
                    instance.gameObject.SetActive(true);
                }
            }
        }
    }

    // Spawn power up randomly into the play area
    void SpawnPowerUp()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            PoolableObject instance = powerUpPool.GetObject();

            if (instance != null)
            {
                PowerUp powerUp = instance.GetComponent<PowerUp>();
                
                int vertextIndex = Random.Range(0, triangulation.vertices.Length);

                UnityEngine.AI.NavMeshHit hit;
                if (NavMesh.SamplePosition(triangulation.vertices[vertextIndex], out hit, 2f, 1))
                {
                    instance.transform.localPosition = hit.position + heightAdjustment;
                    instance.transform.rotation = foodPrefab.transform.rotation;
                    instance.gameObject.SetActive(true);
                }
            }
        }
    }
}
