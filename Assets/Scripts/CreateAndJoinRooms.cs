using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{

    // Fields

    public TMP_InputField _createInput;
    public TMP_InputField _joinInput;

    // Methods

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_joinInput.text);
    }

    public override void OnJoinedRoom() 
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
