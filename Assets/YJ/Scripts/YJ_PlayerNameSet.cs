using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_PlayerNameSet : MonoBehaviourPun
{
    // 내 닉네임(일단은 아이디로 설정되어있음 ConnectionManager에서)를 머리위에 띄우고싶다
    Text playerName;

    void Start()
    {
        playerName = GetComponent<Text>();
        photonView.RPC("RpcPlayerNameSet", RpcTarget.All);

    }

    void Update()
    {

    }

    [PunRPC]
    void RpcPlayerNameSet()
    {
        playerName.text = PhotonNetwork.NickName;
    }
}
