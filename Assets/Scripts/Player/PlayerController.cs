using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    //Character ID
    public enum CharacterID
    {
        GeorgeWashington,
        AbrahamLincoln
    };

    //Inspector
    [Header("CharacterID")]
    public CharacterID characterID;
    [Header("Player values")]
    public float rotationSpeed;
    [Header("Animator reference")]
    public Animator animator;
    [Header("Joystick references")]
    public Joystick joystick;
    public GameObject attackButton;
    [Header("Joystick Dead Zone")]
    [Range(0, 1)]
    public float MovementDeadZoneValue;

    
    //Private
    private CharacterController _characterController;
    private Vector3 move;
    [HideInInspector]
    public Vector3 targetDir;
    private Vector3 _velocityVector;
    private Vector3 accelerationVector;
    private Vector3 decelerationVector;
    private float _decelerationModule;
    private float _accelerationModule;
    #region Animator Triggers
    private string ATTACK = "Attack";
    #endregion

    //Public
    [HideInInspector]
    public PlayerMovementData playerMovementData;
    [HideInInspector]
    public CharacterController characterController { get => _characterController; set => _characterController = value; }
    [HideInInspector]
    public Vector3 velocityVector { get => _velocityVector; set => _velocityVector = value; }
    [HideInInspector]
    //public Vector3 targetDir { get => targetDir; set => targetDir = value; }
    
    public float decelerationModule { get => _decelerationModule; set => _decelerationModule = value; }
    [HideInInspector]
    public float accelerationModule { get => _accelerationModule; set => _accelerationModule = value; }

    private void Awake()
    {
        _characterController = transform.GetComponent<CharacterController>();

        foreach (var item in animator.GetBehaviours<PlayerBaseState>()) {
            item.SetContext(this, animator);
        }
    }


    public void Movement(Vector3 _targetDir, float _maxSpeed, float _accelerationModule) {

        Vector3 accelerationVectorTemp = _targetDir;
        accelerationVectorTemp.y = 0;
        accelerationVector = accelerationVectorTemp.normalized * _accelerationModule;
        move = Vector3.ClampMagnitude((velocityVector * Time.deltaTime + 0.5f * accelerationVector * Mathf.Pow(Time.deltaTime, 2)), _maxSpeed * Time.deltaTime);
        velocityVector = Vector3.ClampMagnitude((velocityVector + accelerationVector * Time.deltaTime), _maxSpeed);
        characterController.Move(move);
    }

    public void Rotation() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg, transform.eulerAngles.z);
    }

    public void MovementReset() {
        move = Vector3.zero;
        accelerationVector = Vector3.zero;
        velocityVector = Vector3.zero;
        decelerationVector = Vector3.zero;
        decelerationModule = 0;
    }

    public void Deceleration(AnimationCurve _movementDecelCurve, float _t0, float _t1, int _iterations) {
        float vectorAngle = Vector3.SignedAngle(Vector3.forward, velocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        decelerationVector = new Vector3(Mathf.Sin(vectorAngle) * decelerationModule, 0, Mathf.Cos(vectorAngle) * decelerationModule);
        move = decelerationVector.normalized * Integration.IntegrateCurve(_movementDecelCurve, _t0, _t1, _iterations);
        velocityVector = _movementDecelCurve.Evaluate(_t1) * decelerationVector.normalized;
        characterController.Move(move);
    }

    public bool checkDeadZone() {
        if (Mathf.Pow(joystick.Horizontal, 2) + Mathf.Pow(joystick.Vertical, 2) >= Mathf.Pow(MovementDeadZoneValue, 2)) {
            return true;
        }
        else {
            return false;
        }
    }

    public void Attack() {
        animator.SetTrigger(ATTACK);
    }

    

}
