using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterModifier : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        bullet_script.damage_mul *= 0.5f;
        bullet_script.delay += 1;

        
        for (int i = 1; i < 5*strenght; i++)
        {
            GameObject new_bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                new_bullet.transform.position = bullet.transform.position;
                new_bullet.transform.rotation = bullet.transform.rotation;
                new_bullet.transform.RotateAround(new_bullet.transform.position, Vector3.up, 25 * Random.Range(-1f, 1f));

                BulletSc new_bulletSc = new_bullet.GetComponent<BulletSc>();

                if (new_bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    List<BulletModifier> new_bms = new List<BulletModifier>(bullet_script.GetBulletModifiers());
                    new_bms.Remove(this);
                    new_bullet.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                    new_bulletSc.damage_mul *= 0.5f;
                    new_bulletSc.speed *= Random.Range(0.95f, 1.05f);
                    new_bulletSc.affiliation = bullet_script.affiliation;
                    new_bulletSc.SetBulletMaterialToAffiliation();
                    new_bulletSc.Activate();
                    new_bulletSc.AddModifiers(new_bms);
                    new_bulletSc.Shoot();
                }
            }
        }
        bullet.transform.RotateAround(bullet.transform.position, Vector3.up, 45 * Random.Range(-1f, 1f));
    }

    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
