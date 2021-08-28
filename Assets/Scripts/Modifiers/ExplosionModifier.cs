using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionModifier : BulletModifier
{
    public GameObject explotion_prefab;
    GameObject explotion;

    public ExplosionModifier(GameObject explotion_new)
    {
        explotion_prefab = explotion_new;
    }
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet_script.delay += 0.5f;
    }

    public override void destroy_effect(GameObject bullet)
    {
        explotion = Object.Instantiate(explotion_prefab, bullet.transform.position, Quaternion.identity);
        explotion.transform.localScale =
            new Vector3 (explotion.transform.localScale.x*bullet.transform.localScale.x,
            explotion.transform.localScale.y * bullet.transform.localScale.y,
            explotion.transform.localScale.z * bullet.transform.localScale.z);
        explotion.GetComponent<DamagingEffect>().damage = 1f * strenght;
    }

}
