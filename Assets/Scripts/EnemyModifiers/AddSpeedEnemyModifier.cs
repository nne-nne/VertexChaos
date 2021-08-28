using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class AddSpeedEnemyModifier : EnemyModifier
{
    public override void create_effect(GameObject enemy)
    {
        EnemyController enemy_sc = enemy.GetComponent<EnemyController>();
        enemy_sc.maxMovSpeed += 5 * strenght;
    }
}
