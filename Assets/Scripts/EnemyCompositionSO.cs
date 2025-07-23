using UnityEngine;

[CreateAssetMenu(fileName = "EnemyComposition", menuName = "ScriptableObjects/EnemyComposition", order = 2)]
public class EnemyCompositionSO : ScriptableObject
{
    public int level;
    public WaveSO[] waves;
    public float[] timeToWaitBetweenWaves;

    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var wave in waves)
        {
            foreach (var enemyInfo in wave.enemyInformation)
            {
                total += enemyInfo.count;
            }
        }
        return total;
    }
}