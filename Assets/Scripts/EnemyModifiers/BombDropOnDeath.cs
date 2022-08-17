using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class BombDropOnDeath : EnemyModifier
{
    GameObject[] explotion;
    public BombDropOnDeath(GameObject[] mod)
    {
        explotion = new GameObject[2] { mod[2], mod[2] };
    }
    public override void destroy_effect(GameObject enemy)
    {

        GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (new_bullet != null)
        {
            GameObject Player = GameObject.FindGameObjectsWithTag("Player")[0];
            new_bullet.transform.position = enemy.transform.position;
            new_bullet.transform.LookAt(Player.transform);

            BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

            if (new_bulletSc != null)
            {
                List<BulletModifier> new_bms = new List<BulletModifier>();
                new_bms.Add(new ExplosionModifier(explotion));
                new_bulletSc.damage = 1f;
                new_bulletSc.speed *= 1.5f;
                new_bulletSc.lifetime = 2f;

                new_bulletSc.affiliation = Affiliation.Enemy;
                new_bulletSc.SetBulletMaterialToAffiliation();
                new_bulletSc.Activate();
                new_bulletSc.AddModifiers(new_bms);
                new_bulletSc.Shoot();
                new_bullet.transform.localScale *= 1 + strenght;

            }
        }
        
    }
}
