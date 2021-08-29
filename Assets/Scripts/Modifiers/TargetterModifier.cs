using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class TargetterModifier : BulletModifier
{
    public override void trigger_effect(GameObject bullet, Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            for (int i = 0; i < strenght; i++)
            {
                GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
                if (bullet != null)
                {
                    GameObject Player = GameObject.FindGameObjectsWithTag("Player")[0];
                    new_bullet.transform.position = Player.transform.position;
                    new_bullet.transform.LookAt(other.transform);

                    BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

                    if (new_bulletSc != null)
                    {
                        new_bulletSc.damage = 1f;
                        new_bulletSc.speed *= 1 + 0.15f * i;
                        new_bulletSc.affiliation = Affiliation.Player;
                        new_bulletSc.SetBulletMaterialToAffiliation();
                        new_bulletSc.Activate();
                        new_bulletSc.AddModifiers(new List<BulletModifier>());
                        new_bulletSc.Shoot();

                    }
                }
            }
        }
    }


    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
