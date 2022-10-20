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
        roomName = YJ_DataManager.CreateRoomInfo.roomName;

        print("¿Ã∏ß¿ª πŸ≤„!!");

        if(photonView.IsMine)
            photonView.RPC("RpcTextChange", RpcTarget.All);
    }

    void Update()
    {
        
    }

    [PunRPC]
    void RpcTextChange()
    {
        text.text = roomName;
    }
}
