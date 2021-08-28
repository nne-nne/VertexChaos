using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetterModifier : BulletModifier
{
    public override void trigger_effect(GameObject bullet, Collider other)
    {
        if (other.CompareTag("Enemy"))
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
                    new_bulletSc.damage = 0.5f * strenght;
                    new_bulletSc.Activate();
                    new_bulletSc.AddModifiers(new List<BulletModifier>());
                    new_bulletSc.Shoot();

                }
            }
        }
    }


    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
