using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelsScript : MonoBehaviour
{
    public Transform player;
    public List<NotSharedPool> enemyPools;
    public List<float> enemyFrequencies;
    public float randomFrac;
    public float difficultyMultiplier;
    [SerializeField] private float arenaRadius;
    private int levelNum;
    private int enemiesLeft;

    public static UnityEvent EndLevelEvent { get; set; } = new UnityEvent();
    public static UnityEvent StartLevelEvent { get; set; } = new UnityEvent();

    void Start()
    {
        EnemiesSubscribeEvents();
        StartLevelEvent.AddListener(NextLevel);
        StartGame(); //TODO: game should be started via main menu
    }

    private void StartGame()
    {
        levelNum = 1;
        NextLevel();
    }

    private void EnemiesSubscribeEvents()
    {
        foreach (NotSharedPool pool in enemyPools)
        {
            foreach(Transform enemy in pool.gameObject.transform)
            {
                EnemyController enemyController = enemy.gameObject.GetComponent<EnemyController>();
                enemyController.DeathEvent.AddListener(CheckNextLevel);
            }
        }
    }

    private int Sum(List<int> list)
    {
        int result = 0;
        foreach(int elem in list)
        {
            result += elem;
        }
        return result;
    }

    private void CheckNextLevel()
    {
        enemiesLeft -= 1;
        if (enemiesLeft == 0)
        {
            EndLevelEvent.Invoke();
        }
    }

    List<GameObject> PrepareEnemySquad(List<int> amounts)
    {
        List<GameObject> enemySquad = new List<GameObject>();
        for(int i = 0; i < amounts.Count; i++)
        {
            if (i >= enemyPools.Count) break;
            for (int j = 0; j < amounts[i]; j++)
            {
                GameObject newEnemy = enemyPools[i].GetPooledObject();
                if (newEnemy != null)
                {
                    enemySquad.Add(newEnemy);
                    newEnemy.SetActive(true);
                }
            }
        }
        return enemySquad;
    }

    private List<GameObject> Shuffle(List<GameObject> enemies)
    {
        List<GameObject> result = new List<GameObject>();
        int iterations = enemies.Count;
        for(int i = 0; i < iterations; i++)
        {
            int index = Random.Range(0, enemies.Count);
            result.Add(enemies[index]);
            enemies.RemoveAt(index);
        }
        return result;
    }

    void PlaceEnemies(List<GameObject> enemiesSquad)
    {
        //List<GameObject> enemies = Shuffle(enemiesSquad);
        List<GameObject> enemies = enemiesSquad;
        int totalEnemies = enemies.Count;
        float angleStep = 2 * Mathf.PI / (totalEnemies + 1);

        for(int i = 0; i < enemies.Count; i++)
        {
            float currentAngle = i * angleStep;
            enemies[i].transform.position = player.position + new Vector3(
                     arenaRadius * Mathf.Sin(currentAngle),
                     0f,
                     arenaRadius * Mathf.Cos(currentAngle));
            enemies[i].transform.rotation = transform.rotation;
        }
    }

    List<int> CalculateAmounts(int levelNum)
    {
        List<int> amounts = new List<int>();
        float baseCount = 1 + levelNum * difficultyMultiplier;   
        foreach (int freq in enemyFrequencies)
        {
            int randomOffset = (int)Random.Range(-randomFrac * baseCount, randomFrac * baseCount);
            int enemyInstances = levelNum * freq + randomOffset;
            if (enemyInstances < 0) enemyInstances = 0;
            amounts.Add(enemyInstances);
        }
        return amounts;
    }

    void PrintList(List<int> list)
    {
        Debug.Log("**********");
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }

    private void TestCalculatingAmounts(int numberOfTests)
    {
        int level = 0;
        for (int i = 0; i < numberOfTests; i++)
        {
            level += 1;
        }
    }

    void Spawn()
    {
        List<int> amounts = CalculateAmounts(levelNum);
        List<GameObject> enemySquad = PrepareEnemySquad(amounts);
        PlaceEnemies(enemySquad);
        enemiesLeft = enemySquad.Count;
    }

    private void NextLevel()
    {
        Spawn();
        levelNum += 1;
    }

    void Update()
    {
        
    }
}
