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
    CharacterController ch;
    Vector3 networkedPosition;
    Quaternion networkedRotation;
    float distance;
    float angle;
    GameObject arenaReference;

    private void Awake() {
        ch = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        arenaReference = GameObject.Find("Arena");
        networkedPosition = Vector3.zero;
        networkedRotation = Quaternion.identity;

    }


    private void FixedUpdate() {

        if (!photonView.IsMine) {
            ch.transform.position = Vector3.MoveTowards(ch.transform.position , networkedPosition , distance*(1/PhotonNetwork.SerializationRate));
            ch.transform.rotation = Quaternion.RotateTowards(ch.transform.rotation, networkedRotation, angle*(1/PhotonNetwork.SerializationRate)); 
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //My controller --> Send data and info to other players
            stream.SendNext(ch.transform.position - arenaReference.transform.position);
            stream.SendNext(ch.transform.rotation);

            //TODO: (??PROBABLY??) stream also the animations connecvted to the player animator controller?

            if (synchronizedVelocity) {
                stream.SendNext(ch.attachedRigidbody.velocity);
            }

            if (synchronizedAngularVelocity) {
                stream.SendNext(ch.attachedRigidbody.angularVelocity);
            }


        }
        else{
            //Gets the update of the position and rotations
            
            networkedPosition = (Vector3)stream.ReceiveNext() + arenaReference.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();


            if (isTeleportEnabled) {
                if (Vector3.Distance(ch.transform.position , networkedPosition) > teleportConstraintValue) {
                    ch.transform.position = networkedPosition;
                }
            }

            if (synchronizedVelocity || synchronizedAngularVelocity) {

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizedVelocity) {
                    ch.attachedRigidbody.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += ch.attachedRigidbody.velocity * lag;
                    distance = Vector3.Distance(ch.transform.position, networkedPosition);
                }

                if (synchronizedAngularVelocity) {
                    ch.attachedRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotation = Quaternion.Euler(ch.attachedRigidbody.angularVelocity * lag) * networkedRotation ;
                    angle = Quaternion.Angle(ch.transform.rotation , networkedRotation);
                }

            }

        }    
    }

}
