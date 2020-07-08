using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerSynchronization : MonoBehaviour , IPunObservable
{

    //Public
    public bool synchronizedVelocity = true;
    public bool synchronizedAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportConstraintValue = 1;


    //Private
    PhotonView photonView;
    PlayerController player;
    Vector3 networkedPosition;
    Quaternion networkedRotation;
    float distance;
    float angle;
    GameObject arenaReference;

    private void Awake() {
        player = GetComponent<PlayerController>();
        photonView = GetComponent<PhotonView>();
        arenaReference = GameObject.Find("Arena");
        networkedPosition = Vector3.zero;
        networkedRotation = Quaternion.identity;

    }


    private void FixedUpdate() {

        if (!photonView.IsMine) {
            player.transform.position = Vector3.MoveTowards(player.transform.position , networkedPosition , distance*(1/PhotonNetwork.SerializationRate));
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, networkedRotation, angle*(1/PhotonNetwork.SerializationRate)); 
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //My controller --> Send data and info to other players
            stream.SendNext(player.transform.position - arenaReference.transform.position);
            stream.SendNext(player.transform.rotation);

            if (synchronizedVelocity) {
                stream.SendNext(player.velocityVector);
            }

            if (synchronizedAngularVelocity) {
                stream.SendNext(player.transform.rotation);
            }


        }
        else{
            //Gets the update of the position and rotations
            
            networkedPosition = (Vector3)stream.ReceiveNext() + arenaReference.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();


            if (isTeleportEnabled) {
                if (Vector3.Distance(player.transform.position , networkedPosition) > teleportConstraintValue) {
                    player.transform.position = networkedPosition;
                }
            }

            if (synchronizedVelocity || synchronizedAngularVelocity) {

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizedVelocity) {
                    player.velocityVector = (Vector3)stream.ReceiveNext();
                    networkedPosition += player.velocityVector * lag;
                    distance = Vector3.Distance(player.transform.position, networkedPosition);
                }

                if (synchronizedAngularVelocity) {
                    player.transform.rotation = (Quaternion)stream.ReceiveNext();
                    //player.attachedRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
                    //networkedRotation = Quaternion.Euler(player.attachedRigidbody.angularVelocity * lag) * networkedRotation;
                    angle = Quaternion.Angle(player.transform.rotation , networkedRotation);
                }

            }

        }    
    }

}
