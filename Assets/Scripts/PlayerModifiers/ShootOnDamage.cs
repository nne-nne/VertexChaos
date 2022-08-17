using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnDamage : PlayerModifier
{
    CannonController cannon;
    public override void damaged_effect()
    {
        base.damaged_effect();
        for (int i = 0; i < strenght*2; i++)
        {
            Debug.Log("shooting");
            GameObject bullet = cannon.instantShoot();
            bullet.transform.RotateAround(bullet.transform.position, Vector3.up, 45 * Random.Range(-1f, 1f));
            BulletSc bulletSc = bullet.GetComponent<BulletSc>();
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            bulletSc.Shoot();
        }


    }

    public override void normal_effect(PlayerController playerController)
    {
        base.normal_effect(playerController);
        cannon = GameObject.FindGameObjectsWithTag("PlayerCannon")[0].GetComponent<CannonController>();
    }
}
