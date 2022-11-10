using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_PlayerNameSet : MonoBehaviourPun
{
    // 내 닉네임(일단은 아이디로 설정되어있음 ConnectionManager에서)를 머리위에 띄우고싶다
    string Pname;
    // 방에있는 인원확인 (RPC를 다시 또 쏴주기위해서)
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
