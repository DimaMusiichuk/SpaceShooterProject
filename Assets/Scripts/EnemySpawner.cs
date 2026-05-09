using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Налаштування ворогів")]
    [SerializeField] private GameObject[] enemyPrefabs;
    
    [Header("Налаштування рандому спавну")]
    [SerializeField] private Vector2 spawnRateRange = new Vector2(1.5f, 4.0f); 
    [SerializeField] private float spawnY = 7f; 
    [SerializeField] private float minX = -6f; 
    [SerializeField] private float maxX = 6f; 

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float currentSpawnRate = Random.Range(spawnRateRange.x, spawnRateRange.y);
            yield return new WaitForSeconds(currentSpawnRate);

            if (enemyPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject prefabToSpawn = enemyPrefabs[randomIndex];

                float randomX = Random.Range(minX, maxX);
                Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);

                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            }
        }
    }
}