using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SpawnManagerScript : MonoBehaviour
{
    public Canvas canvas;
    public int enemiesLeft;
    public GameObject[] spawnPoints;
    public bool[] spawnPointIsActive;
    private Vector3[] worldSpawnPositions;
    public EnemyCompositionSO enemiesToSpawn;
    public WaveSO currentWave;
    public int[] enemyInRow;
    public bool canSpawnNextWave = true;
    public bool spawnCompleted;

    void Start()
    {
        spawnCompleted = false;
        GameObject inGameView = GameManagerScript.Instance.inGameView;
        //look for a child named base canvas inside inGameView (has to be by name because there can be multiple canvases)
        if (inGameView != null)
        {
            canvas = inGameView.transform.Find("BaseCanvas").GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Base Canvas not found in InGameView.");
            }
            else
            {
                // Debug.Log("Base Canvas found: " + canvas.name);
            }
        }
        else
        {
            Debug.LogError("InGameView not found.");
        }
        spawnPointIsActive = new bool[spawnPoints.Length];
        enemyInRow = new int[spawnPoints.Length];
        StartCoroutine(SpawnEnemies());
    }
    
    private void CalculateSpawnPositions()
    {
        worldSpawnPositions = new Vector3[spawnPoints.Length];
        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            RectTransform rectTransform = spawnPoints[i].GetComponent<RectTransform>();
            Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(rectTransform.position);
            worldSpawnPositions[i] = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            
            // Debug.Log($"Spawn Point {i} ({spawnPoints[i].name}): UI Pos = {rectTransform.position}, World Pos = {worldSpawnPositions[i]}");
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
        CalculateSpawnPositions();

        enemiesLeft = enemiesToSpawn.GetTotalEnemyCount();
        Debug.Log("Total enemies to spawn: " + enemiesLeft);
        for (int i = 0; i < enemiesToSpawn.waves.Length; i++)
        {
            currentWave = CloneWave(enemiesToSpawn.waves[i]);
            Debug.Log("Starting wave " + i + " with " + currentWave.enemyInformation.Length + " enemy types.");
            StartCoroutine(SpawnEnemiesInWave(currentWave));
            canSpawnNextWave = false;

            //Wait until canSpawnNextWave is true
            while (!canSpawnNextWave)
            {
                yield return null; // Wait for the next frame
            }

            if (enemiesToSpawn.timeToWaitBetweenWaves.Length > i)
            {
                Debug.Log("Waiting for " + enemiesToSpawn.timeToWaitBetweenWaves[i] + " seconds before next wave.");
                yield return new WaitForSeconds(enemiesToSpawn.timeToWaitBetweenWaves[i]);
            }
            else
            {
                while (enemiesLeft != 0)
                {
                    Debug.Log("Waiting for enemies to finish spawning. Enemies left: " + enemiesLeft);
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

    void DetermineValidSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (enemyInRow[i] < 2)
            {
                // Debug.Log("Spawn point " + spawnPoints[i].name + " is active. Current enemy count in row: " + enemyInRow[i]);
                spawnPointIsActive[i] = true;
                continue;
            }
            // Debug.Log("Spawn point " + spawnPoints[i].name + " is inactive. Current enemy count in row: " + enemyInRow[i]);
            spawnPointIsActive[i] = false;
        }
    }

    private IEnumerator SpawnEnemiesInWave(WaveSO wave)
    {
        while (wave.enemyInformation.Length > 0)
        {
            DetermineValidSpawnPoints();
            Debug.Log("Spawning enemies for wave with " + wave.enemyInformation.Length + " type(s) left.");

            int randomEnemyIndex = Random.Range(0, wave.enemyInformation.Length);
            // Debug.Log("Randomly selected enemy type index: " + randomEnemyIndex + " - " + wave.enemyInformation[randomEnemyIndex].enemyPrefab.name);

            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            // Debug.Log("Randomly selected spawn point index: " + randomSpawnPointIndex + " - " + spawnPoints[randomSpawnPointIndex].name);

            if (!spawnPointIsActive[randomSpawnPointIndex])
            {
                // Debug.Log("Spawn point " + spawnPoints[randomSpawnPointIndex].name + " is inactive. Finding a new spawn point...");
                while (!spawnPointIsActive[randomSpawnPointIndex])
                {
                    DetermineValidSpawnPoints();
                    randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
                    // Debug.Log("Randomly selected spawn point index: " + randomSpawnPointIndex + " - " + spawnPoints[randomSpawnPointIndex].name);
                }
            }
            enemyInRow[randomSpawnPointIndex]++;

            Vector3 worldPos = worldSpawnPositions[randomSpawnPointIndex];

            // Debug.Log("Spawning enemy at world position: " + worldPos);

            GameObject spawnedEnemy = Instantiate(
                wave.enemyInformation[randomEnemyIndex].enemyPrefab,
                worldPos,
                Quaternion.identity
            );

            spawnedEnemy.GetComponent<Enemy>().row = randomSpawnPointIndex;
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

                Debug.Log("Removed enemy type from wave. Remaining types: " + wave.enemyInformation.Length);
            }
            else
            {
                // Debug.Log("Enemy type still has remaining count: " + wave.enemyInformation[randomEnemyIndex].count);
            } 

            Debug.Log("Waiting for " + wave.spawnInterval + " seconds before next spawn.");
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        Debug.Log("Wave completed. All enemies spawned.");
        canSpawnNextWave = true;
    }
}