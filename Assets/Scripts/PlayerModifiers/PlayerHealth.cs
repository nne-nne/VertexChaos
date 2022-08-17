using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : PlayerModifier
{
    public override void normal_effect(PlayerController playerController)
    {
        base.normal_effect(playerController);
        playerController.maxHealth += 5;
        playerController.health += 5;
    }
}
