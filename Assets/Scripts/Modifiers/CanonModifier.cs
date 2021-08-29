using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonModifier : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();

        bullet_script.delay += 1*(strenght/4);
        bullet_script.damage += 0.5f * strenght;
        bullet_script.speed *= 0.25f / strenght;
        bullet.transform.localScale += new Vector3(5, 5, 5) * strenght/2;
        bullet_script.life += 4 * strenght;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
