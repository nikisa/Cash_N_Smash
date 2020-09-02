using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageController : MonoBehaviour
{

    public PlayerController myCharacter;
    public float damage = 10;

    [PunRPC]
    public void ReceiveDamage(float _damage , PlayerController _enemy) {
        _enemy.life -= _damage;
    }

    [PunRPC]
    public void UpdateUI(PlayerController _enemy , PlayerController _player) {
        _enemy.graphicLife.fillAmount = _enemy.life / _enemy.maxLife;
        _player.graphicLife.fillAmount = _player.life / _player.maxLife;
    }



    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject != myCharacter.gameObject) {

    //        if (!other.gameObject.GetComponent<PlayerController>().enabled) {
    //            other.gameObject.GetComponent<PlayerController>().enabled = true;
    //        }

    //        ReceiveDamage(damage , other.gameObject.GetComponent<PlayerController>());
    //        UpdateUI(other.gameObject.GetComponent<PlayerController>() , myCharacter);
    //        if (myCharacter.gameObject.GetComponent<PhotonView>().IsMine) {
    //            myCharacter.gameObject.GetComponent<PhotonView>().RPC("ReceiveDamage" , RpcTarget.AllBuffered , damage , other.gameObject.GetComponent<PlayerController>());
    //            myCharacter.gameObject.GetComponent<PhotonView>().RPC("UpdateUI", RpcTarget.AllBuffered , other.gameObject.GetComponent<PlayerController>() , myCharacter);
    //            other.gameObject.GetComponent<PlayerController>().enabled = false;
    //        }
            
    //    }

    //}
}
