using System.Collections.Generic;
using UnityEngine;

public class ProceduralCoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int numberOfCoins = 20;
    public float spawnRadius = 10f;
    public float noiseScale = 5f;
    public float placementThreshold = 0.4f;
    public string groundTag = "Ground";
    public float coinFloatHeight = 0.5f;
    public float coinLifetime = 30f;
    public float delayBetweenWaves = 60f;

    private Transform playerTransform;
    private float respawnTimer = 0f;
    private bool isWaitingToRespawn = false;

    private List<GameObject> activeCoins = new List<GameObject>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnCoinsAroundPlayer();
    }

    void Update()
    {
        activeCoins.RemoveAll(coin => coin == null);

        if (activeCoins.Count == 0 && !isWaitingToRespawn)
        {
            isWaitingToRespawn = true;
            respawnTimer = 0f;
        }

        if (isWaitingToRespawn)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= delayBetweenWaves)
            {
                SpawnCoinsAroundPlayer();
                isWaitingToRespawn = false;
            }
        }
    }

    void SpawnCoinsAroundPlayer()
    {
        int spawnedCoins = 0;
        int maxAttempts = numberOfCoins * 5;

        for (int i = 0; i < maxAttempts && spawnedCoins < numberOfCoins; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPos = playerTransform.position + new Vector3(randomCircle.x, 10f, randomCircle.y);

            float noiseValue = Mathf.PerlinNoise((randomPos.x + 500f) / noiseScale, (randomPos.z + 500f) / noiseScale);
            if (noiseValue < placementThreshold)
                continue;

            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 20f))
            {
                if (hit.collider.CompareTag(groundTag))
                {
                    Vector3 coinPosition = hit.point + Vector3.up * coinFloatHeight;
                    Quaternion coinRotation = Quaternion.Euler(90f, 0f, 0f);
                    GameObject coin = Instantiate(coinPrefab, coinPosition, coinRotation);
                    Destroy(coin, coinLifetime);
                    activeCoins.Add(coin);
                    spawnedCoins++;
                }
            }
        }
    }
}
