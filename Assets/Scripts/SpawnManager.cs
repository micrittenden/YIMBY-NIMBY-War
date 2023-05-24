using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject tokenPrefab;
    public GameObject powerUpPrefab;

    // Basic spawn
    private float spawnRange = 24.0f;
    private float spawnY = 0.8f;
        
    // Food
    private float spawnRangeFood = 20.0f; // Affects NIMBYs as well by inverse
    private float startDelayFood = 10.0f;
    private float spawnIntervalFood = 20.0f;

    // Token
    private float startDelayToken = 1.0f;
    private float spawnIntervalToken = 5.0f;
    private float despawnToken = 60.0f;

    // Powerup
    private float spawnRangePowerUp = 10.0f;
    private float startDelayPowerUp = 30.0f;
    private float spawnIntervalPowerUp = 120.0f;

    public void StartSpawn()
    {
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
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), spawnY, Random.Range(spawnRangeFood, spawnRange));

            Instantiate(foodPrefab, spawnPos, foodPrefab.transform.rotation);
        }
    }

    // Spawn token randomly into the play area
    void SpawnToken()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), spawnY, Random.Range(-spawnRange, spawnRange));

            // Instantiate as a named GameObject so that I can destroy it after a minute if it has not already been picked up
            GameObject token = Instantiate(tokenPrefab, spawnPos, tokenPrefab.transform.rotation) as GameObject;

            Destroy(token, despawnToken);
        }
    }

    // Spawn power up randomly into the play area
    void SpawnPowerUp()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangePowerUp, spawnRangePowerUp), spawnY, Random.Range(-spawnRangePowerUp, spawnRangePowerUp));

            Instantiate(powerUpPrefab, spawnPos, powerUpPrefab.transform.rotation);
        }
    }
}
