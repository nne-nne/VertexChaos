using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "ScriptableObjects/EnemyWaveScriptableObject")]
public class EnemyWave : ScriptableObject
{
    public List<GameObject> enemies;
}
