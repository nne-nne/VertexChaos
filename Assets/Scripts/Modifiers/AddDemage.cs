using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDemage : BulletModifier
{

    public override void create_effect(GameObject bullet)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();
        bullet_script.damage += 0.5f*strenght;
    }

    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }

}
