using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public float life = 100;
    public float speed = 10;
    public float maxSpeed = 10;
    public float rotationSpeed;
    [Header("Animator reference")]
    public Animator animator;
    [Header("View reference [MVC]")]
    public GameObject graphics;
    [Header("Collider Reference (Attack)")]
    public Collider attackCollider;
    [Header("UI")]
    public Image graphicLife;
    [Header("Sound References")]
    public AudioSource punchSound;
    [Header("Joystick references")]
    public Joystick joystick;
    public GameObject attackButton;
    [Header("Joystick Dead Zone")]
    [Range(0, 1)]
    public float MovementDeadZoneValue;


    //Private
    private CharacterController _characterController;
    private Rigidbody rb;
    private Vector3 move;
    [HideInInspector]
    public Vector3 targetDir;
    private Vector3 _velocityVector = Vector3.zero;
    private Vector3 accelerationVector;
    private Vector3 decelerationVector;
    private float _decelerationModule;
    private float _accelerationModule;
    private float _maxLife;
    #region Animator Triggers
    private string ATTACK = "Attack";
    #endregion

    //Public
    [HideInInspector]
    public PlayerMovementData playerMovementData;
    //[HideInInspector]
    //public CharacterController characterController { get => _characterController; set => _characterController = value; }
    [HideInInspector]
    public Vector3 velocityVector { get => _velocityVector; set => _velocityVector = value; }
    [HideInInspector]
    //public Vector3 targetDir { get => targetDir; set => targetDir = value; }

    public float decelerationModule { get => _decelerationModule; set => _decelerationModule = value; }
    [HideInInspector]
    public float accelerationModule { get => _accelerationModule; set => _accelerationModule = value; }
    public float maxLife { get => _maxLife; }

    private void Awake() {
        //_characterController = transform.GetComponent<CharacterController>();
        _maxLife = life;
        rb = GetComponent<Rigidbody>();
        //attackCollider.gameObject.SetActive(false);
        foreach (var item in animator.GetBehaviours<PlayerBaseState>()) {
            item.SetContext(this, animator);
        }

        punchSound.gameObject.SetActive(false);
    }


    private void Update() {
        Debug.Log(this.gameObject.name + " " + life);
        MovementInput();
        Death();
        //UpdateUI();
    }

    public void FixedUpdate() {
        if (velocityVector != Vector3.zero) {
            Vector3 velocity = rb.velocity;
            Vector3 deltaVelocity = (velocityVector - velocity);

            deltaVelocity.x = Mathf.Clamp(deltaVelocity.x, -maxSpeed, maxSpeed);
            deltaVelocity.z = Mathf.Clamp(deltaVelocity.z, -maxSpeed, maxSpeed);
            deltaVelocity.y = 0;

            rb.AddForce(deltaVelocity, ForceMode.Acceleration);
        }
    }


    #region Methods for CharacterController 

    //public void Movement(Vector3 _targetDir, float _maxSpeed, float _accelerationModule) {

    //    Vector3 accelerationVectorTemp = _targetDir;
    //    accelerationVectorTemp.y = 0;
    //    accelerationVector = accelerationVectorTemp.normalized * _accelerationModule;
    //    move = Vector3.ClampMagnitude((velocityVector * Time.deltaTime + 0.5f * accelerationVector * Mathf.Pow(Time.deltaTime, 2)), _maxSpeed * Time.deltaTime);
    //    velocityVector = Vector3.ClampMagnitude((velocityVector + accelerationVector * Time.deltaTime), _maxSpeed);
    //    characterController.Move(move);
    //}

    //public void Rotation() {
    //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg, transform.eulerAngles.z);
    //}

    //public void MovementReset() {
    //    move = Vector3.zero;
    //    accelerationVector = Vector3.zero;
    //    velocityVector = Vector3.zero;
    //    decelerationVector = Vector3.zero;
    //    decelerationModule = 0;
    //}

    //public void Deceleration(AnimationCurve _movementDecelCurve, float _t0, float _t1, int _iterations) {
    //    float vectorAngle = Vector3.SignedAngle(Vector3.forward, velocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
    //    decelerationVector = new Vector3(Mathf.Sin(vectorAngle) * decelerationModule, 0, Mathf.Cos(vectorAngle) * decelerationModule);
    //    move = decelerationVector.normalized * Integration.IntegrateCurve(_movementDecelCurve, _t0, _t1, _iterations);
    //    velocityVector = _movementDecelCurve.Evaluate(_t1) * decelerationVector.normalized;
    //    characterController.Move(move);
    //}

    #endregion

    public void UpdateUI() {
        graphicLife.fillAmount = life / _maxLife;
    }

    public void Death() {
        if (life <= 0) {
            PhotonNetwork.LeaveRoom();
            LeftRoom();
        }
    }


    public void LeftRoom() {
        SceneLoader.Instance.LoadScene("Lobby");
    }


    public void MovementInput() {

        Vector3 _velocityVetctor = Vector3.zero;

        if (checkDeadZone()) {

            float _xAxesMovement = joystick.Horizontal;
            float _zAxesMovement = joystick.Vertical;

            Vector3 _movementHorizontal = transform.right * _xAxesMovement;
            Vector3 _movementVertical = transform.forward * _zAxesMovement;
            _velocityVetctor = (_movementHorizontal + _movementVertical) * speed;

            Move(_velocityVetctor);
            Rotation();
            animator.SetTrigger("Movement");

        }
        else {
            Move(_velocityVetctor);
            animator.SetTrigger("Idle");
        }

    }

    private void Move(Vector3 _velocityVector) {
        velocityVector = _velocityVector;
    }

    public void Rotation() {
        graphics.transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg, transform.eulerAngles.z);
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
        attackCollider.gameObject.SetActive(true);
    }

    [PunRPC]
    public void ReceiveDamage(float _damage) {
        life -= _damage;
    }

    [PunRPC]
    public void UpdateUI(PlayerController _player) {
        graphicLife.fillAmount = life / maxLife;
    }

    [PunRPC]
    public void PlayPunchFX() {
        punchSound.gameObject.SetActive(true);
        punchSound.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {


        if (other.gameObject.GetComponent<DamageController>()) {
            ReceiveDamage(other.gameObject.GetComponent<DamageController>().damage);
            UpdateUI();
            PlayPunchFX();
        }

        if (gameObject.GetComponent<PhotonView>().IsMine) {
            gameObject.GetComponent<PhotonView>().RPC("ReceiveDamage", RpcTarget.AllBuffered, other.gameObject.GetComponent<DamageController>().damage);
            gameObject.GetComponent<PhotonView>().RPC("UpdateUI", RpcTarget.AllBuffered);
            gameObject.GetComponent<PhotonView>().RPC("PlayPunchFX", RpcTarget.AllBuffered);
        }


    }



}
