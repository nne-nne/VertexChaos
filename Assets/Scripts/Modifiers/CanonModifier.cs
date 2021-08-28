using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonModifier : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();

        bullet_script.delay += 1;
        bullet_script.damage += 1.0f;
        bullet_script.speed *= 0.5f;
        bullet.transform.localScale += new Vector3(5, 5, 5);
        bullet_script.life += 2;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
