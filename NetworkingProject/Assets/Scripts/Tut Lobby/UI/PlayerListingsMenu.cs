using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;

    [SerializeField]
    private GameObject startGameButton;
    [SerializeField]
    private GameObject readyUpButton;

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
        CheckPlayerStatus();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        foreach(PlayerListing listing in _listings)
        {
            Destroy(listing.gameObject);
        }
        _listings.Clear();
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach(KeyValuePair<int,Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        CheckPlayerStatus();

        int index = _listings.FindIndex(x => x.Player == newMasterClient);
        _listings[index].SetPlayerInfo(newMasterClient);
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient && CheckPlayersReady())
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
            return false;

        if (_listings.Count== 1)//if the host wants to play alone, or for testing
            return true;

        int currentPlayerNum = _listings.Count - 1;
        int playersReadyNum = 0;

        foreach(PlayerListing listing in _listings)
        {
            if (listing.Player == PhotonNetwork.LocalPlayer)//does not count the master client
                continue;
            else if(listing.GetComponent<UnityEngine.UI.RawImage>().color == Color.green)
            {
                playersReadyNum++;
            }
        }

        if(playersReadyNum == currentPlayerNum)
            return true;
        else
            return false;
    }

    private void CheckPlayerStatus()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startGameButton.GetComponent<Button>().interactable = true;
            readyUpButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            startGameButton.GetComponent<Button>().interactable = false;
            readyUpButton.GetComponent<Button>().interactable = true;
        }

    }
}
