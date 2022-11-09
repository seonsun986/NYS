using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_RoomTextUI : MonoBehaviourPun
{
    public Text text;
    string roomName;

    void Start()
    {
        if (photonView.IsMine)
        {
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            photonView.RPC("RpcTextChange", RpcTarget.All, roomName);
        }
    }

    int playerIndex;
    void Update()
    {
        if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RpcTextChange", RpcTarget.All, roomName);
            }

            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
    }

    [PunRPC]
    void RpcTextChange(string room)
    {
        roomName = room;
        text.text = roomName;
    }
}
