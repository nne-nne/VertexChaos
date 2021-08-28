using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class BigBadEnemyModifier : EnemyModifier
{
    public override void create_effect(GameObject enemy)
    {
        EnemyController enemy_sc = enemy.GetComponent<EnemyController>();
        enemy_sc.maxHealth += 40;
        enemy_sc.health = enemy_sc.maxHealth;
        enemy.transform.localScale += new Vector3(5, 5, 5);
        enemy_sc.timeBetweenShots -= 0.5f;
        enemy_sc.maxMovSpeed -= 10f;
    }
}
