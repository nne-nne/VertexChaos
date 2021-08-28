using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class ShieldModifier : BulletModifier
{
    public override void trigger_effect(GameObject bullet, Collider other)
    {
        BulletSc bullet_script = bullet.GetComponent<BulletSc>();

        if (other.CompareTag("bullet"))
        {
            Affiliation affiliation = other.gameObject.GetComponent<BulletSc>().affiliation;
            switch (affiliation)
            {                
                case Affiliation.Enemy:
                    other.gameObject.SetActive(false);
                    bullet_script.life -= 1;
                    break;

            }
        }
    }


    public override string show_message()
    {
        return "Raise bullet damage by 10%";
    }
}
