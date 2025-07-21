using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyComposition", menuName = "ScriptableObjects/EnemyComposition", order = 2)]
public class EnemyCompositionSO : ScriptableObject
{
    public int level;
    public WaveSO[] waves;
}