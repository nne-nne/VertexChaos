using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllModifiers : MonoBehaviour
{
    public static AllModifiers instance;

    public List<GeneralModifier> modifiers;
    public List<String> modifierNames;
    public List<String> modifierDesc;
    public GameObject[] mods = new GameObject[2];
    public void Start()
    {
        instance = this;
        init_modifiers();
    }

    private void init_modifiers()
    {
        modifiers = new List<GeneralModifier>();
        modifierNames = new List<string>();
        modifierDesc = new List<string>();

        modifiers.Add(new AddDamage());             // 0
        modifierNames.Add("Add Damage");
        modifierDesc.Add("Raises damage dealt by your bullets");

        modifiers.Add(new AddLifeTime());           // 1
        modifierNames.Add("Add Life Time");
        modifierDesc.Add("Bullets can fly further");

        modifiers.Add(new AddSpeed());              // 2
        modifierNames.Add("Add Speed");
        modifierDesc.Add("Couses bullets to fly faster");

        modifiers.Add(new CanonModifier());         // 3
        modifierNames.Add("Cannon Modifier");
        modifierDesc.Add("Your projectiles will be bigger and badder");

        modifiers.Add(new AddDamage());             // 4
        modifierNames.Add("Add Damage");
        modifierDesc.Add("Raises damage dealt by your bullets");

        modifiers.Add(new ExplosionModifier(mods)); // 5
        modifierNames.Add("Exploding Bullets");
        modifierDesc.Add("Bullets explode when they are destryed");

        modifiers.Add(new FourWayShootModifier());  // 6
        modifierNames.Add("All Way Bullets");
        modifierDesc.Add("Bullets split to all directions");

        modifiers.Add(new AddSpeed());              // 7
        modifierNames.Add("Add Speed");
        modifierDesc.Add("Couses bullets to fly faster");

        modifiers.Add(new PierceModifier());        // 8
        modifierNames.Add("Piercing Bullets");
        modifierDesc.Add("Bullets pierce through the angels");

        modifiers.Add(new ScatterModifier());       // 9
        modifierNames.Add("Scatter Bullets");
        modifierDesc.Add("Shots burst of small bullets");

        modifiers.Add(new TargetterModifier());     // 10
        modifierNames.Add("Targetter Bullets");
        modifierDesc.Add("Hitting enemy will couse you to shout additional bullet at the enemy");

        modifiers.Add(new ShootOnDamage());
        modifierNames.Add("Shoot On Damage");
        modifierDesc.Add("Shoots bullets when damaged");

        modifiers.Add(new PlayerHealth());
        modifierNames.Add("Raise Max Health");
        modifierDesc.Add("Raises players maximum health");

        modifiers.Add(new PlayerHeal());
        modifierNames.Add("Heal Player");
        modifierDesc.Add("Fully heals player, also raises maximum health slightly");

    }

    public List<GeneralModifier> GetModifiers()
    {
        return modifiers;
    }

    public void clearStrenght()
    {
        foreach (GeneralModifier m in modifiers)
        {
            if (m.type == ModType.modType.Bullet)
            {
                BulletModifier b = m as BulletModifier;
                b.strenght = 1;

            }
            else if (m.type == ModType.modType.Player)
            {
                PlayerModifier b = m as PlayerModifier;
                b.strenght = 1;
            }
        }
    }
}
