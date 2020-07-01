using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : BaseState
{

    //Protected
    protected PlayerController player;
    protected Animator graphicAnimation;


    //SateMachine Parameters
    protected const string END_STATE_TRIGGER = "EndState";
    protected const string IDLE = "Idle";
    protected const string MOVEMENT = "Movement";
    protected const string MOVEMENT_DECELERATION = "MovementDeceleration";
    protected const string ATTACK = "Attack";
    protected const string DEFENSE = "Defense";

    public void SetContext(object context, Animator animator/*, Animator graphicAnimation*/) {
        player = context as PlayerController;
        this.animator = animator;
        //this.graphicAnimation = graphicAnimation;
    }

    protected void TriggerExitState() {
        animator.SetTrigger(END_STATE_TRIGGER);
    }

}
