using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMultiplayerSetup : MonoBehaviourPun
{

    //Inspector
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        if (photonView.IsMine) {
            transform.GetComponent<PlayerController>().enabled = true;
            transform.GetComponent<PlayerController>().joystick.gameObject.SetActive(true);
            transform.GetComponent<PlayerController>().attackButton.gameObject.SetActive(true);
        }
        else {
            transform.GetComponent<PlayerController>().enabled = false;
            transform.GetComponent<PlayerController>().joystick.gameObject.SetActive(false);
            transform.GetComponent<PlayerController>().attackButton.gameObject.SetActive(false);

        }
        SetPlayerName();
    }

    void SetPlayerName() {
        if (photonView.Owner != null) {
            playerNameText.text = photonView.Owner.NickName;
        }

        if (photonView.IsMine) {
            playerNameText.color = Color.blue;
        }
        else {
            playerNameText.color = Color.red;
        }

    }
    
}
