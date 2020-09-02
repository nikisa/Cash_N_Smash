using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerSynchronization : MonoBehaviour, IPunObservable
{

    //Public
    public bool synchronizedVelocity = true;
    public bool synchronizedAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportConstraintValue = 5;


    //Private
    Rigidbody rb;
    PhotonView photonView;
    PlayerController player;
    Vector3 networkedPosition;
    Quaternion networkedRotation;
    float distance;
    float angle;
    GameObject arenaReference;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        player = GetComponent<PlayerController>();
        arenaReference = GameObject.Find("Arena");
        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();

    }


    private void FixedUpdate() {

        if (!photonView.IsMine) {
            //player.transform.position = Vector3.MoveTowards(player.transform.position , networkedPosition , distance*(1/PhotonNetwork.SerializationRate));
            //player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, networkedRotation, angle*(1/PhotonNetwork.SerializationRate)); 
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance * (1.0f / PhotonNetwork.SerializationRate));
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
            player.graphics.transform.rotation = Quaternion.RotateTowards(player.graphics.transform.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //My controller --> Send data and info to other players
            stream.SendNext(rb.position - arenaReference.transform.position);
            //stream.SendNext(rb.rotation);
            stream.SendNext(player.graphics.transform.rotation);

            if (synchronizedVelocity) {
                stream.SendNext(rb.velocity);
            }

            if (synchronizedAngularVelocity) {
                stream.SendNext(rb.angularVelocity);
            }


        }
        else {
            //Gets the update of the position and rotations

            networkedPosition = (Vector3)stream.ReceiveNext() + arenaReference.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();


            if (isTeleportEnabled) {
                if (Vector3.Distance(rb.position, networkedPosition) > teleportConstraintValue) {
                    //player.transform.position = networkedPosition;
                    rb.position = networkedPosition;
                }
            }

            if (synchronizedVelocity || synchronizedAngularVelocity) {

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizedVelocity) {
                    //player.velocityVector = (Vector3)stream.ReceiveNext();
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkedPosition);
                }

                if (synchronizedAngularVelocity) {
                    //player.transform.rotation = (Quaternion)stream.ReceiveNext();
                    //player.attachedRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;
                    angle = Quaternion.Angle(player.graphics.transform.rotation, networkedRotation);
                }
            }
        }
    }
}
