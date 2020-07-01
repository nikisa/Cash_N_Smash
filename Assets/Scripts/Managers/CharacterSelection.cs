using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CharacterSelection : MonoBehaviour
{

    //Inspector
    [Header("UI References")]
    public TextMeshProUGUI characterType;
    [Header("UI References")]
    public GameObject uiSelecting;
    public GameObject uiSelected;

    //Private
    int selectedCharacter;

    private void Start() {
        selectedCharacter = 0;
        uiSelecting.SetActive(true);
        uiSelected.SetActive(false);
    }

    public void OnReSelectButtonClicked() {
        uiSelecting.SetActive(true);
        uiSelected.SetActive(false);
    }

    public void OnBattleButtonClicked() {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked() {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    public void OnSelectButtonClicked() {

        uiSelecting.SetActive(false);
        uiSelected.SetActive(true);

        ExitGames.Client.Photon.Hashtable characterSelection = new ExitGames.Client.Photon.Hashtable { {MultiplayerCharacterSelector.CHARACTER_SELECTED_NUMBER , selectedCharacter} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterSelection);
    }

    //TO DO: GetSelectedCharacter() --> Based on the dollar recognition
    //TO DO: GetAttack or Difense Type base on the front or back of the dollar
}
