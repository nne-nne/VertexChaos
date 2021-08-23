using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeed : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        Bullet bullet_script = bullet.GetComponent<Bullet>();
        bullet_script.speed *= 1.05f;
    }

    public override string show_message()
    {
        return "Raise bullet speed by 0.05";
    }
}
