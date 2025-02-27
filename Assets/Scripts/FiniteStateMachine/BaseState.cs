﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{

    protected object context;
    protected Animator animator;



    private void Awake() {

    }

    public virtual void SetContext(object context, Animator animator) {
        this.context = context;
        this.animator = animator;

    }

    public virtual void Enter() {

    }

    public virtual void Tick() {

    }

    public virtual void Exit() {

    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Enter();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Exit();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Tick();
    }




}
