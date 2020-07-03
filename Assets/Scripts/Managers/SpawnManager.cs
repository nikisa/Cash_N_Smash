using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SpawnManager : MonoBehaviourPunCallbacks
{

    //Inspector
    public GameObject[] charactersPrefabs;
    public Transform[] spawnPoint;
    public GameObject arenaReference;

    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0
    }

    private void Start() {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDestroy() {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    #region Photon Callbacks

    void OnEvent(EventData photonEvent) {

        if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode) {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int receivedPlayerSelectionData = (int)data[3];

            GameObject player = Instantiate(charactersPrefabs[receivedPlayerSelectionData],receivedPosition + arenaReference.transform.position , receivedRotation);
            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.ViewID = (int)data[2];
        }

    }

    public override void OnJoinedRoom() {
        if (PhotonNetwork.IsConnectedAndReady) {
            //object characterSelectionNumber;
            //if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerCharacterSelector.CHARACTER_SELECTED_NUMBER , out characterSelectionNumber)) {
            //    Debug.Log("Character selection number: "  + (int) characterSelectionNumber);

            //    //TO DO: intantiatePosition anchored to the dollar
            //    Vector3 instantiatePosition = Vector3.zero;

            //    PhotonNetwork.Instantiate(charactersPrefabs[(int) characterSelectionNumber].name , instantiatePosition , Quaternion.identity);

            //}
            SpawnPlayer();
        }

    }

    #endregion


    void SpawnPlayer() {
        object characterSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerCharacterSelector.CHARACTER_SELECTED_NUMBER, out characterSelectionNumber)) {
            Debug.Log("Character selection number: " + (int)characterSelectionNumber);

            //TO DO: intantiatePosition anchored to the dollar
            Vector3 instantiatePosition = Vector3.zero;

            GameObject playerReference = Instantiate(charactersPrefabs[(int)characterSelectionNumber], instantiatePosition, Quaternion.identity);
            PhotonView photonView = playerReference.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView)) {

                object[] data = new object[] { playerReference.transform.position - arenaReference.transform.position, playerReference.transform.rotation, photonView.ViewID, characterSelectionNumber };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode , data , raiseEventOptions , sendOptions);


            }
            else {
                Destroy(playerReference);
                Debug.Log("Allocation Failed!");
            }

        }
    }


}
