using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagingAuraModifier : BulletModifier
{
    public GameObject aura_prefab;
    GameObject aura;
    Vector3 base_scale;

    public DamagingAuraModifier(GameObject aura_new)
    {
        aura_prefab = aura_new;
    }

    public override void create_effect(GameObject bullet)
    {
        

        aura = Object.Instantiate(aura_prefab, bullet.transform.position, Quaternion.identity, bullet.transform);
        base_scale = aura.transform.localScale;
        aura.GetComponent<DamagingEffect>().damage = 0.5f + 0.25f * strenght;
    }

    public override void update_effect(GameObject bullet)
    {
        aura.transform.localScale =
    new Vector3(base_scale.x * bullet.transform.localScale.x,
    base_scale.y * bullet.transform.localScale.y,
    base_scale.z * bullet.transform.localScale.z) * (0.8f + 0.2f*strenght);
    }

    public override void destroy_effect(GameObject bullet)
    {
        Object.Destroy(aura);
    }


}
