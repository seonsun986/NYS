using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_PlayerNameSet : MonoBehaviourPun
{
    // �� �г���(�ϴ��� ���̵�� �����Ǿ����� ConnectionManager����)�� �Ӹ����� ����ʹ�
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
