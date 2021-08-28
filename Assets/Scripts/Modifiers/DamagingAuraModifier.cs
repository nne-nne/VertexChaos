using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagingAuraModifier : BulletModifier
{
    public GameObject aura_prefab;
    GameObject aura;

    public DamagingAuraModifier(GameObject aura_new)
    {
        aura_prefab = aura_new;
    }

    public override void create_effect(GameObject bullet)
    {
        aura = Object.Instantiate(aura_prefab, bullet.transform.position, Quaternion.identity, bullet.transform);
    }

    public override void destroy_effect(GameObject bullet)
    {
        Object.Destroy(aura);
    }


}
