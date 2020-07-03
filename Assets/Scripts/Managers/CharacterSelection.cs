using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CharacterSelection : MonoBehaviour
{

    //Inspector
    [Header("UI References")]
    public GameObject uiReset;
    public GameObject uiLobby;
    public GameObject uiFight;

    //Public
    [HideInInspector]
    public GameObject actualCharacter;

    //Private
    int _selectedCharacter;

    //Public
    public int selectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }

    private void Start() {
        selectedCharacter = -1;
        uiReset.SetActive(false);
        uiFight.SetActive(false);
    }

    public void OnReSelectButtonClicked() {
        uiReset.SetActive(false);
        uiFight.SetActive(false);
        Destroy(actualCharacter);
    }

    public void OnBattleButtonClicked() {
        ExitGames.Client.Photon.Hashtable characterSelection = new ExitGames.Client.Photon.Hashtable { { MultiplayerCharacterSelector.CHARACTER_SELECTED_NUMBER, selectedCharacter } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterSelection);

        SceneLoader.Instance.LoadScene("Gameplay");
    }

    public void OnBackButtonClicked() {
        SceneLoader.Instance.LoadScene("Lobby");
    }

    //TO DO: GetSelectedCharacter() --> Based on the dollar recognition
    //TO DO: GetAttack or Difense Type base on the front or back of the dollar
}
