using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{

    // Fields

    public TMP_InputField _createInput;
    public TMP_InputField _joinInput;
    public TextMeshProUGUI feedbackText; // To display messages to the user

    // Methods

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_createInput.text))
        {
            feedbackText.text = "Room name cannot be empty.";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(_createInput.text, roomOptions, TypedLobby.Default);
        feedbackText.text = "Creating room...";
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_joinInput.text))
        {
            feedbackText.text = "Room name cannot be empty.";
            return;
        }

        PhotonNetwork.JoinRoom(_joinInput.text);
        feedbackText.text = "Joining room...";
    }

    // Callbacks

    public override void OnJoinedRoom()
    {
        feedbackText.text = "Successfully joined the room!";
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        feedbackText.text = $"Room creation failed: {message}";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        feedbackText.text = $"Failed to join room: {message}";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedbackText.text = $"Random room join failed: {message}";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedbackText.text = $"Disconnected from server: {cause}";
    }
}
