using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_PlayerNameSet : MonoBehaviourPun
{
    // �� �г���(�ϴ��� ���̵�� �����Ǿ����� ConnectionManager����)�� �Ӹ����� ����ʹ�
    string Pname;
    // �濡�ִ� �ο�Ȯ�� (RPC�� �ٽ� �� ���ֱ����ؼ�)
    int playerIndex;

    void Start()
    {
        playerIndex = PhotonNetwork.CurrentRoom.Players.Count;

        gameObject.GetComponent<Text>().text = photonView.Owner.NickName;
        //if (photonView.IsMine)
        //{
        //    photonView.RPC("RpcPlayerNameSet", RpcTarget.All, UserInfo_e.nickname);
        //}

    }

    void Update()
    {
        //if (playerIndex != PhotonNetwork.CurrentRoom.Players.Count)
        //{
        //    if (photonView.IsMine)
        //    {
        //        photonView.RPC("RpcPlayerNameSet", RpcTarget.All, UserInfo_e.nickname);
        //    }

        //    playerIndex = PhotonNetwork.CurrentRoom.Players.Count;
        //}
    }

    [PunRPC]
    void RpcPlayerNameSet(string s)
    {
        Pname = s;
        this.gameObject.GetComponent<Text>().text = Pname;
    }
}
