using System.Collections;
using UnityEngine;

public class NimbySpawn : MonoBehaviour
{
    public Nimby NimbyPrefab;
    private float startDelayNimby = 5.0f;
    private float spawnIntervalNimby = 15.0f;
    private float spawnRange = 24.0f;
    private ObjectPool NimbyPool;

    private void Awake()
    {
        NimbyPool = ObjectPool.CreateInstance(NimbyPrefab, 10);
    }

    public void StartSpawn()
    {
        InvokeRepeating("SpawnNimby", startDelayNimby, spawnIntervalNimby);
    }

    // Spawn NIMBYs randomly into the play area
    void SpawnNimby()
    {
        if (GameManager.Instance.IsGameActive() && !GameManager.Instance.IsGameOver())
        {
            PoolableObject instance = NimbyPool.GetObject();

            if (instance != null)
            {
                instance.transform.SetParent(transform, false);
                Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
                instance.transform.localPosition = spawnPos;
                instance.gameObject.SetActive(true);
            }
        }
    }
}