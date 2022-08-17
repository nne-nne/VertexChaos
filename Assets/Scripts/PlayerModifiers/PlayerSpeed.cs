using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : PlayerModifier
{
    public override void normal_effect(PlayerController playerController)
    {
        base.normal_effect(playerController);
        playerController.maxMovSpeed += 20;
    }
}
