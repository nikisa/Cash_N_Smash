using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetupState : PlayerBaseState
{
    public PlayerMovementData playerMovementData;

    public override void Enter() {
        player.playerMovementData = playerMovementData;
    }
}
