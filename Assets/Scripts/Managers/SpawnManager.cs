using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{

    //Inspector
    public GameObject[] charactersPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Photon Callbacks
    public override void OnJoinedRoom() {
        if (PhotonNetwork.IsConnectedAndReady) {
            object characterSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerCharacterSelector.CHARACTER_SELECTED_NUMBER , out characterSelectionNumber)) {
                Debug.Log("Character selection number: "  + (int) characterSelectionNumber);

                //TO DO: intantiatePosition anchored to the dollar
                Vector3 instantiatePosition = Vector3.zero;
                
                PhotonNetwork.Instantiate(charactersPrefabs[(int) characterSelectionNumber].name , instantiatePosition , Quaternion.identity);
            
            }
        }

    }

    #endregion
}
