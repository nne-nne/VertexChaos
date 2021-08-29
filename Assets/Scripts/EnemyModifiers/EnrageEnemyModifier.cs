using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class EnrageEnemyModifier : EnemyModifier
{
    public override void trigger_effect(GameObject enemy)
    {
        EnemyController ec = enemy.GetComponent<EnemyController>();
        ec.maxMovSpeed += 0.5f * strenght;
        ec.timeBetweenShots -= 0.01f * strenght;
    }
}
