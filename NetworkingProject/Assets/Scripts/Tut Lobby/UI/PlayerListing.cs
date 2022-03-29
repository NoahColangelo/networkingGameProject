using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private RawImage _rawImage;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;

        SetReadyUp(player);
    }

    private void SetReadyUp(Player player)
    {
        bool isReady = false;
        if (player.CustomProperties.ContainsKey("isReady"))
            isReady = (bool)player.CustomProperties["isReady"];

        if (isReady)
            _rawImage.color = Color.green;
        else
            _rawImage.color = Color.red;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer != null && targetPlayer == Player)
        {
            if(changedProps.ContainsKey("isReady"))
            SetReadyUp(targetPlayer);
        }
    }
}
