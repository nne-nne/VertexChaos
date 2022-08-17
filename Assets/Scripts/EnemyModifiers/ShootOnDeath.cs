using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class ShootOnDeath : EnemyModifier
{
    public override void destroy_effect(GameObject enemy)
    {
        float actual_value = 1.25f + (1.25f * (strenght - 1));
        for (int i = 1; i < actual_value; i++)
        {
            GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (enemy != null)
            {
                new_bullet.transform.position = enemy.transform.position;
                new_bullet.transform.rotation = enemy.transform.rotation;
                new_bullet.transform.RotateAround(new_bullet.transform.position, Vector3.up, (360 / (4 * strenght)) * i);

                BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

                if (new_bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    List<BulletModifier> new_bms = new List<BulletModifier>();
                    new_bulletSc.affiliation = Affiliation.Enemy;
                    new_bulletSc.SetBulletMaterialToAffiliation();
                    new_bulletSc.Activate();
                    new_bulletSc.AddModifiers(new_bms);
                    new_bulletSc.Shoot();

                }
            }
        }

    }
}
