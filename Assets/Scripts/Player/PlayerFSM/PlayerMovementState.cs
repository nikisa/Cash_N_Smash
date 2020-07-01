using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{

    //Inspector
    public PlayerMovementData playerMovementData;

    public override void Enter() {
        player.accelerationModule = playerMovementData.maxSpeed / playerMovementData.AccelerationTime;
    }

    public override void Tick() {


        if (player.checkDeadZone()) {
            player.targetDir = new Vector3(player.joystick.Horizontal, 0, player.joystick.Vertical);
            Debug.DrawRay(player.transform.position, player.targetDir, Color.red, 0.2f);
            //if (player.CharacterController == null) {
            player.Movement(player.targetDir, playerMovementData.maxSpeed, player.accelerationModule);
            player.Rotation();

        }
        else {
            animator.SetTrigger(MOVEMENT_DECELERATION);
        }
    }
}
