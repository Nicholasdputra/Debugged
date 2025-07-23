using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCompositionData", menuName = "ScriptableObjects/EnemyCompositionData")]
public class EnemyCompositionDataSO : ScriptableObject
{
    public EnemyCompositionSO[] enemyCompositions;

    // You can add methods to manipulate or retrieve enemy compositions if needed
    public EnemyCompositionSO GetEnemyComposition(int index)
    {
        foreach (var composition in enemyCompositions)
        {
            if (composition.level == index)
            {
                return composition;
            }
        }
        Debug.LogWarning("No enemy composition found for level: " + index);
        return null;
    }
}