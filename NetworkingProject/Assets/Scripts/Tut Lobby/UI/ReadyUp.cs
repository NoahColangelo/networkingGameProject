using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReadyUp : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _readyUp = new ExitGames.Client.Photon.Hashtable();

    private bool _isReady = false;

    private void SetReadyUp()
    {
        if (!_isReady)
            _isReady = true;
        else
            _isReady = false;

        _readyUp["isReady"] = _isReady;
        PhotonNetwork.SetPlayerCustomProperties(_readyUp);
    }

    public void OnClick_ReadyUp()
    {
        SetReadyUp();
    }
}
