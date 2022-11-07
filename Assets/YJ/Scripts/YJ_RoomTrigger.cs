using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_RoomTrigger : MonoBehaviourPun
{
    public string roomName;
    public int roomType;
    // Start is called before the first frame update
    void Start()
    {
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        if (photonView.IsMine)
        {
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
            
            photonView.RPC("RpcNameSet", RpcTarget.All, roomName, roomType);
        }
    }
    int playerIndex;

    void Update()
    {
        // ���ο� ����� �������� RPC �ٽ� ���ֱ�
        if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        {
            // ��ĳ���� ���� ���
            if (photonView.IsMine)
            {
                photonView.RPC("RpcNameSet", RpcTarget.All, roomName, roomType);
            }

            playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        }
    }

    [PunRPC]
    void RpcNameSet(string name, int type)
    {
        roomName = name;
        roomType = type;
    }

}
