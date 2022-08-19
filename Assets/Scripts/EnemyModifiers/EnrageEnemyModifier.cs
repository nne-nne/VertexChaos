using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class EnrageEnemyModifier : EnemyModifier
{
    bool used = false;
    public override void trigger_effect(GameObject enemy)
    {
        if (!used)
        {
            used = true;
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.maxMovSpeed += 10f * strenght;
        }
    }
}
