using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.BroadcastPropsChangeToAll = true;

        if (_roomName != null)
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        else
            Debug.Log("pleae add a name to the room before creating");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("created room successfully", this);
        _roomsCanvases.CurrentRoomCanvas.Show();

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
    }
}
