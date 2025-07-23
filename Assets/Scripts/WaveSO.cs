using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveSO : ScriptableObject
{
    // Add properties related to the wave, such as the count for each enemy types, spawn intervals, etc.
    public EnemyInformation[] enemyInformation;
    public float spawnInterval; // Time between spawns in seconds
}

//make a class to hold enemy types and their counts
[System.Serializable]
public class EnemyInformation
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public int count; // Number of enemies to spawn of this type
}