using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A script that spawns enemies of given prefabs,
/// then waits till all the enemies are killed,
/// then spawns another wave.
/// 
/// Enemies are spawned around a circle, equally spaced
/// </summary>

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyWave> waves;
    [SerializeField] private float arenaRadius;

    private List<GameObject> enemiesToKill;
    private int currentWave;

    void Start()
    {
        currentWave = 0;
        enemiesToKill = new List<GameObject>();
    }

    private void NextWave()
    {
        SpawnWave(waves[currentWave]);
        currentWave = (currentWave + 1) % waves.Count;
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        enemiesToKill.Remove(enemy);
        if(enemiesToKill.Count == 0)
        {
            NextWave();
        }
    }

    private void SpawnWave(EnemyWave wave)
    {
        float angleStep = 360/(wave.enemies.Count+1);
        foreach(GameObject enemy in wave.enemies)
        {
            GameObject newEnemy = Instantiate(enemy, new Vector3(0f, 0f, arenaRadius),
                                    Quaternion.Euler(Vector3.back), gameObject.transform);
            enemiesToKill.Add(enemy);
            transform.Rotate(0f, angleStep, 0f);
        }
    }

    void Update()
    {
        /// prowizorka tymczasowa ofkors
        if(Input.GetKeyDown(KeyCode.E))
        {
            NextWave();
        }
    }
}
