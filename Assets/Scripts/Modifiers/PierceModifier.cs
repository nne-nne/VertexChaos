using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceModifier : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();

        bullet_script.damage -= 0.2f;
        bullet_script.speed *= 1.01f;
        bullet_script.delay += 0.25f;
        bullet.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
        bullet_script.life += 5*strenght;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}

