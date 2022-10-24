using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_Back : YJ_PlazaManager
{
    public static new SH_Back instance;

    private void Awake()
    {
        instance = this;
    }

    //[HideInInspector]
    //public new UserInfo userInfo;

    // ���� �ִ� �ο� �ľ��ϱ�
    public new int liveCount = 0;

    public new Vector3[] spawnPos;

    public override void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        // XR_A��� ������ ����
        //PhotonNetwork.JoinRoom("Lobby");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom("Lobby");
    }

    public override string ChangeSceneName()
    {
        sceneName = "PlazaScene";
        return sceneName;
    }
}
