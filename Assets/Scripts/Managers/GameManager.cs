using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    //Inspector
    [Header("UI")]
    public GameObject informPanelReference;
    public TextMeshProUGUI uiInformReference;
    public GameObject searchGameButtonReference;
    public GameObject adjustButton;
    public GameObject raycastCenterReference;

    //Private
    private float secondsDeactivation = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        informPanelReference.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callbacks
    public void JoinRandomRoom() {
        uiInformReference.text = "Searching for open lobbies";
        PhotonNetwork.JoinRandomRoom();
        searchGameButtonReference.SetActive(false);
    }

    #endregion


    #region Photon Callbacks

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.LogWarning(message);
        uiInformReference.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom() {

        adjustButton.SetActive(false);
        raycastCenterReference.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            uiInformReference.text = PhotonNetwork.CurrentRoom.Name + " joined! Waiting for more players.";
        }
        else {
            uiInformReference.text = PhotonNetwork.CurrentRoom.Name + " joined!";
            StartCoroutine(DeactivatedAfterSeconds(informPanelReference , secondsDeactivation));
        }

        Debug.LogFormat("{0} has joined the {1}" , PhotonNetwork.NickName , PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.LogFormat("{0} has joined the {1}. Total Players: {2}", PhotonNetwork.NickName, PhotonNetwork.CurrentRoom.Name , PhotonNetwork.CurrentRoom.PlayerCount);
        uiInformReference.text = newPlayer.NickName + " has joined the " + PhotonNetwork.CurrentRoom.Name + ". Total Players: " + PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(DeactivatedAfterSeconds(informPanelReference , secondsDeactivation));
    
    }

    #endregion

    void CreateAndJoinRoom() {

        string randomRoomName = "Room" + Random.Range(0,1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(randomRoomName , roomOptions);
    }

    IEnumerator DeactivatedAfterSeconds(GameObject _gameObject , float _seconds) {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
        
    }

}
