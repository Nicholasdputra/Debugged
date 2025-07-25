using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SpawnManagerScript : MonoBehaviour
{
    public int waveNumber = 0;
    // public Canvas canvas;
    public EnemyCompositionDataSO enemyCompositionDataSO;
    public int enemiesLeft;
    public GameObject[] spawnPoints;
    public bool[] spawnPointIsActive;
    // private Vector3[] worldSpawnPositions;
    public EnemyCompositionSO enemiesToSpawn;
    public WaveSO currentWave;
    public int[] enemyInRow;
    public bool canSpawnNextWave = true;
    public bool spawnCompleted;

    void Awake()
    {
        waveNumber = 0;
        spawnCompleted = false;
        GameObject inGameView = GameManagerScript.Instance.inGameView;
        spawnPointIsActive = new bool[spawnPoints.Length];
        enemyInRow = new int[spawnPoints.Length];
        enemiesToSpawn = enemyCompositionDataSO.GetEnemyComposition(GameManagerScript.Instance.currentLevel);
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if(enemiesLeft <= 0 && spawnCompleted)
        {
           GameManagerScript.Instance.state = 2; // Change to win state
        }
    }

    private IEnumerator SpawnEnemies()
    {
        spawnCompleted = false;
        if (enemiesToSpawn == null)
        {
            Debug.LogWarning("No enemies to spawn assigned.");
            yield break;
        }

        yield return new WaitForEndOfFrame(); // Ensure UI is ready
        // CalculateSpawnPositions();

        enemiesLeft = enemiesToSpawn.GetTotalEnemyCount();
        // Debug.Log("Total enemies to spawn: " + enemiesLeft);
        for (int i = 0; i < enemiesToSpawn.waves.Length; i++)
        {
            currentWave = CloneWave(enemiesToSpawn.waves[i]);
            // Debug.Log("Starting wave " + i + " with " + currentWave.enemyInformation.Length + " enemy types.");
            StartCoroutine(SpawnEnemiesInWave(currentWave));
            canSpawnNextWave = false;

            //Wait until canSpawnNextWave is true
            while (!canSpawnNextWave)
            {
                yield return null; // Wait for the next frame
            }

            if (enemiesToSpawn.timeToWaitBetweenWaves.Length > i)
            {
                // Debug.Log("Waiting for " + enemiesToSpawn.timeToWaitBetweenWaves[i] + " seconds before next wave.");
                yield return new WaitForSeconds(enemiesToSpawn.timeToWaitBetweenWaves[i]);
            }
            else
            {
                while (enemiesLeft != 0)
                {
                    // Debug.Log("Waiting for enemies to finish spawning. Enemies left: " + enemiesLeft);
                    yield return null; // Wait for the next frame
                }
            }
        }
        spawnCompleted = true;
    }
    
    private WaveSO CloneWave(WaveSO original)
    {
        WaveSO clone = ScriptableObject.CreateInstance<WaveSO>();
        clone.spawnInterval = original.spawnInterval;
        
        // Deep copy the enemy information array
        clone.enemyInformation = new EnemyInformation[original.enemyInformation.Length];
        for (int i = 0; i < original.enemyInformation.Length; i++)
        {
            clone.enemyInformation[i] = new EnemyInformation
            {
                enemyPrefab = original.enemyInformation[i].enemyPrefab,
                count = original.enemyInformation[i].count // This is the copy that will be modified
            };
        }
        
        return clone;
    }

    int FindARandomSpawnPoint()
    {
        // Calculate weights for each spawn point
        float[] weights = new float[spawnPoints.Length];
        float totalWeight = 0f;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Inverse square relationship
            float weight = 1f / (1f + enemyInRow[i] * enemyInRow[i]);
            weights[i] = weight;
            totalWeight += weight;
        }
        
        // Generate random value between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);
        
        // Find which spawn point this random value corresponds to
        float currentWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
            {
                return i;
            }
        }
        
        // Fallback (shouldn't reach here, but just in case)
        return Random.Range(0, spawnPoints.Length);
    }

    private IEnumerator SpawnEnemiesInWave(WaveSO wave)
    {
        while (wave.enemyInformation.Length > 0)
        {
            // Debug.Log("Spawning enemies for wave with " + wave.enemyInformation.Length + " type(s) left.");

            int randomEnemyIndex = Random.Range(0, wave.enemyInformation.Length);
            // Debug.Log("Randomly selected enemy type index: " + randomEnemyIndex + " - " + wave.enemyInformation[randomEnemyIndex].enemyPrefab.name);

            int randomSpawnPointIndex = FindARandomSpawnPoint();
            // Debug.Log("Randomly selected spawn point index: " + randomSpawnPointIndex + " - " + spawnPoints[randomSpawnPointIndex].name);

            enemyInRow[randomSpawnPointIndex]++;

            Vector3 worldPos = spawnPoints[randomSpawnPointIndex].transform.position;

            // Debug.Log("Spawning enemy at world position: " + worldPos);

            GameObject spawnedEnemy = Instantiate(
                wave.enemyInformation[randomEnemyIndex].enemyPrefab,
                worldPos,
                Quaternion.identity
            );

            spawnedEnemy.GetComponent<Enemy>().row = randomSpawnPointIndex;
            spawnedEnemy.GetComponent<Enemy>().spawnManager = this;
            // Debug.Log("Spawned enemy: " + spawnedEnemy.name);

            // Debug.Log("Incremented enemy in row for index " + randomEnemyIndex + ": " + enemyInRow[randomEnemyIndex]);

            wave.enemyInformation[randomEnemyIndex].count--;
            // Debug.Log("Remaining count for " + spawnedEnemy.name + ": " + wave.enemyInformation[randomEnemyIndex].count);

            // Debug.Log("Checking if enemy type can be removed from wave...");
            if (wave.enemyInformation[randomEnemyIndex].count <= 0)
            {
                // Convert array to list for easier removal
                var enemyList = new System.Collections.Generic.List<EnemyInformation>(wave.enemyInformation);
                enemyList.RemoveAt(randomEnemyIndex);
                wave.enemyInformation = enemyList.ToArray();

                // Debug.Log("Removed enemy type from wave. Remaining types: " + wave.enemyInformation.Length);
            }
            else
            {
                // Debug.Log("Enemy type still has remaining count: " + wave.enemyInformation[randomEnemyIndex].count);
            } 

            // Debug.Log("Waiting for " + wave.spawnInterval + " seconds before next spawn.");
            yield return new WaitForSeconds(wave.spawnInterval);
        }
        waveNumber++;
        Debug.Log("Wave completed. All enemies spawned.");
        canSpawnNextWave = true;
    }
}