using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class FourWayShootModifier : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet_script.delay += 0.1f;
        for (int i = 1; i < 2*strenght; i++)
        {
            GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                new_bullet.transform.position = bullet.transform.position;
                new_bullet.transform.rotation = bullet.transform.rotation;
                new_bullet.transform.RotateAround(new_bullet.transform.position, Vector3.up, (360/(2 * strenght))*i);

                BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

                if (new_bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    List<BulletModifier> new_bms = new List<BulletModifier>(bullet_script.GetBulletModifiers());
                    new_bulletSc.affiliation = bullet_script.affiliation;
                    new_bulletSc.SetBulletMaterialToAffiliation();
                    new_bms.Remove(this);
                    new_bulletSc.Activate();
                    new_bulletSc.AddModifiers(new_bms);
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
