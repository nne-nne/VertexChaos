using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class BigBadEnemyModifier : EnemyModifier
{
    public override void create_effect(GameObject enemy)
    {
        EnemyController enemy_sc = enemy.GetComponent<EnemyController>();
        enemy_sc.maxHealth += 10 + 5*strenght;
        enemy_sc.health = enemy_sc.maxHealth;
        enemy.transform.localScale += new Vector3(1.5f+strenght/4, 1.5f + strenght / 4, 1.5f + strenght / 4);
        enemy_sc.timeBetweenShots *= 1 / (1+strenght/100);
        enemy_sc.maxMovSpeed -= 20f;

    }
}
