using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject tokenPrefab;
    public GameObject powerUpPrefab;
    public GameObject nimbyPrefab;

    // Basic spawn
    private float spawnRange = 24;
        
    // Food
    private float spawnRangeFood = 20; // Affects NIMBYs as well by inverse
    private float startDelayFood = 10;
    private float spawnIntervalFood = 15;

    // Token
    private float startDelayToken = 1;
    private float spawnIntervalToken = 5;
    private float despawnToken = 60f;

    // Powerup
    private float spawnRangePowerUp = 10;
    private float startDelayPowerUp = 30;
    private float spawnIntervalPowerUp = 120;

    // Nimby
    private float startDelayNimby = 5;
    private float spawnIntervalNimby = 15;

    // Prepare for accessing PlayerController
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        // Invoke custom functions to spawn prefabs on a timed interval
        InvokeRepeating("SpawnFood", startDelayFood, spawnIntervalFood);
        InvokeRepeating("SpawnToken", startDelayToken, spawnIntervalToken);
        InvokeRepeating("SpawnPowerUp", startDelayPowerUp, spawnIntervalPowerUp);
        InvokeRepeating("SpawnNimby", startDelayNimby, spawnIntervalNimby);
    }
    
    // Spawn food randomly into the play area
    void SpawnFood()
    {
        if (playerControllerScript.gameOver == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(spawnRangeFood, spawnRange));

            Instantiate(foodPrefab, spawnPos, foodPrefab.transform.rotation);
        }
    }

    // Spawn token randomly into the play area
    void SpawnToken()
    {
        if (playerControllerScript.gameOver == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(-spawnRange, spawnRange));

            // Instantiate as a named GameObject so that I can destroy it after a minute if it has not already been picked up
            GameObject token = Instantiate(tokenPrefab, spawnPos, tokenPrefab.transform.rotation) as GameObject;

            Destroy(token, despawnToken);
        }
    }

    // Spawn power up randomly into the play area
    void SpawnPowerUp()
    {
        if (playerControllerScript.gameOver == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangePowerUp, spawnRangePowerUp), 1, Random.Range(-spawnRangePowerUp, spawnRangePowerUp));

            Instantiate(powerUpPrefab, spawnPos, powerUpPrefab.transform.rotation);
        }
    }

    // Spawn NIMBYs randomly into the play area
    void SpawnNimby()
    {
        if (playerControllerScript.gameOver == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRangeFood));

            Instantiate(nimbyPrefab, spawnPos, nimbyPrefab.transform.rotation);
        }
    }
}
