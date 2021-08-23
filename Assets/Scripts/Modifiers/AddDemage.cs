using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDemage : BulletModifier
{

    public override void create_effect(GameObject bullet)
    {
        Bullet bullet_script = bullet.GetComponent<Bullet>();
        bullet_script.damage += 0.5f;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 0.5";
    }

}
