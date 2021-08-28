using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class ShootOnDeath : EnemyModifier
{
    public override void destroy_effect(GameObject enemy)
    {
        for (int i = 1; i < 8; i++)
        {
            GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (enemy != null)
            {
                new_bullet.transform.position = enemy.transform.position;
                new_bullet.transform.rotation = enemy.transform.rotation;
                new_bullet.transform.RotateAround(new_bullet.transform.position, Vector3.up, (360 / (8)) * i);

                BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

                if (new_bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    List<BulletModifier> new_bms = new List<BulletModifier>();
                    new_bulletSc.affiliation = Affiliation.Enemy;
                    new_bulletSc.Activate();
                    new_bulletSc.AddModifiers(new_bms);
                    new_bulletSc.Shoot();

                }
            }
        }

    }
}
