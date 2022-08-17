using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class MoreHealthEnemyModifier : EnemyModifier
{
    public override void create_effect(GameObject enemy)
    {
        EnemyController enemy_sc = enemy.GetComponent<EnemyController>();
        enemy_sc.maxHealth += 2*strenght;
    }
}
