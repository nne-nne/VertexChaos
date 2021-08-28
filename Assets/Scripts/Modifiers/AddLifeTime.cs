using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLifeTime : BulletModifier
{
    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet_script.lifetime += 1.25f*strenght;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 25%";
    }
}
