using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManuverability : PlayerModifier
{
    public override void normal_effect(PlayerController playerController)
    {
        base.normal_effect(playerController);
        playerController.acceleration += 2;
        playerController.angularSpeed += 1.5f;

    }
}
