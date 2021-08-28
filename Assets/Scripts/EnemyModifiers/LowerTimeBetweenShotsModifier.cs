using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class LowerTimeBetweenShotsModifier : EnemyModifier
{
    public override void create_effect(GameObject enemy)
    {
        EnemyController enemy_sc = enemy.GetComponent<EnemyController>();
        enemy_sc.timeBetweenShots *= 1 / (1 + (strenght / 25));
    }
}
