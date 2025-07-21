using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveSO : ScriptableObject
{
    // Add properties related to the wave, such as the count for each enemy types, spawn intervals, etc.
    public int basicEnemyCount;
    
}