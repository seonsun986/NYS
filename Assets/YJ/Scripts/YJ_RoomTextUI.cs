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
        //text = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        //text.text = YJ_DataManager.CreateRoomInfo.roomName;

        if(photonView.IsMine)
            roomName = YJ_DataManager.CreateRoomInfo.roomName;

        //if(photonView.IsMine)
        //    photonView.RPC("RpcTextChange", RpcTarget.MasterClient, roomName);
    }

    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 1 && currentTime < 2)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcTextChange", RpcTarget.All, roomName);
        }
    }

    [PunRPC]
    void RpcTextChange(string room)
    {
        roomName = room;
        text.text = roomName;
    }
}
