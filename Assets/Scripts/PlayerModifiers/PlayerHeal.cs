using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : PlayerModifier
{
    public override void normal_effect(PlayerController playerController)
    {
        base.normal_effect(playerController);
        playerController.maxHealth += 1;
        playerController.health += 1;
        playerController.health = playerController.maxHealth;
    }
}
