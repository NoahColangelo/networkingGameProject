using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);//not visible to the new player that is connecting

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPLayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);//called before onPlayerEnteredRoom

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPLayerLeftRoom() {0}", otherPlayer.NickName);// seen when a player disconnects

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom isMasterClient {0}", PhotonNetwork.IsMasterClient);//called  before onPlayerLeftRoom

            LoadArena();
        }
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    private void LoadArena()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Photon Network: trying to load level but we are not the master client");
        }

        Debug.LogFormat("Photon Network: loading level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion
}
