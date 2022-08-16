using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeed : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet_script.speed *= 1.35f + 0.15f*strenght;
    }

    public override string show_message()
    {
        return "Raise bullet speed by 0.05";
    }
}
