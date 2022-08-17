using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModType;

public class PlayerModifier : GeneralModifier
{
    public int strenght = 1;

    public PlayerModifier()
    {
        type = modType.Player;
    }

    public PlayerModifier(GameObject[] a)
    {
        type = modType.Player;
    }

    virtual public void damaged_effect()
    {

    }

    virtual public void update_effect()
    {

    }

    virtual public void normal_effect(PlayerController playerController)
    {

    }

}
