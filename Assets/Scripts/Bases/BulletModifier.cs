using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModifier
{
    public int strenght = 1;

    public BulletModifier()
    {

    }

    public BulletModifier(GameObject[] a)
    {

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
