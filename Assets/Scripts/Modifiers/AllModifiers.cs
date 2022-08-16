using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllModifiers : MonoBehaviour
{
    public static AllModifiers instance;

    public List<BulletModifier> modifiers;
    public GameObject[] mods = new GameObject[2];
    public void Start()
    {
        instance = this;
        init_modifiers();
    }

    private void init_modifiers()
    {
        modifiers = new List<BulletModifier>();
        modifiers.Add(new AddDamage());
        modifiers.Add(new AddLifeTime());
        modifiers.Add(new AddSpeed());
        modifiers.Add(new CanonModifier());
        modifiers.Add(new AddDamage());
        modifiers.Add(new ExplosionModifier(mods));
        modifiers.Add(new FourWayShootModifier());
        modifiers.Add(new AddSpeed());
        modifiers.Add(new PierceModifier());
        modifiers.Add(new ScatterModifier());
        modifiers.Add(new TargetterModifier());
    }

    public List<BulletModifier> GetModifiers()
    {
        return modifiers;
    }
}
