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

        enemy.transform.localScale += new Vector3(1.2f, 1.2f, 1.2f);
        enemy_sc.timeBetweenShots *= 1;
        enemy_sc.maxMovSpeed -= 10f;
    }
}
