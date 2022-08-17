using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModType;

public class BulletModifier : GeneralModifier
{
    public int strenght = 1;

    public BulletModifier()
    {
        type = modType.Bullet;
    }

    public BulletModifier(GameObject[] a)
    {
        type = modType.Bullet;
    }

    virtual public void create_effect(GameObject bullet)
    {

    }

    virtual public void destroy_effect(GameObject bullet)
    {

    }

    virtual public void trigger_effect(GameObject bullet, Collider other)
    {

    }

    virtual public void update_effect(GameObject bullet)
    {

    }

    virtual public string show_message()
    {
        return "";
    }
}
