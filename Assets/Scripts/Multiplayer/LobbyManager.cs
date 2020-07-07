using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Login")]
    public InputField playerNameInputField;
    public GameObject loginReference;
    [Header("Lobby")]
    public GameObject lobbyReference;
    [Header("Connection Status")]
    public bool isConnectionReady = false;
    public GameObject connectionStatusReference;
    public Text connectionStatus;

    private void Start() {

        if (PhotonNetwork.IsConnected) {
            LoggedUIManagement();
        }
        else {
            LoginUIManagement();
        }
    }

    private void Update() {
        if (isConnectionReady) {
            connectionStatus.text = "Connection status: " + PhotonNetwork.NetworkClientState; 
        }
    }

    #region UI_Management
    private void LoginUIManagement() {
        lobbyReference.SetActive(false);
        connectionStatusReference.SetActive(false);
        loginReference.SetActive(true);
    }

    private void LoggedUIManagement() {
        lobbyReference.SetActive(true);
        connectionStatusReference.SetActive(false);
        loginReference.SetActive(true);
    }

    private void ConnectionUIManagement() {
        lobbyReference.SetActive(false);
        loginReference.SetActive(false);
        connectionStatusReference.SetActive(true);
    }

    private void ConnectedToMasterUIManagement() {
        lobbyReference.SetActive(true);
        loginReference.SetActive(false);
        connectionStatusReference.SetActive(false);
    }
    #endregion

    #region UI Callbacks

    public void OnEnterGameButtonClicked() {

        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName)) {
            isConnectionReady = true;
            ConnectionUIManagement();

            if (!PhotonNetwork.IsConnected) {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }

        }
        else {
            Debug.Log("METTI ER NOME AO PORCACCIO IDDIO");
        }
    }

    public void OnQuickMatchButtonClicked() {
        SceneLoader.Instance.LoadScene("CharacterSelection");
    }

    #endregion

    #region Photon Callbacks
    public override void OnConnected() {
        Debug.Log("CONNECTED");
    }

    public override void OnConnectedToMaster() {
        Debug.LogFormat("{0} is CONNECTED to the server" , PhotonNetwork.LocalPlayer.NickName);
        ConnectedToMasterUIManagement();
    }


    #endregion

}
