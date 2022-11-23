using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_TeacherManager : YJ_PlazaManager
{
    public static new NK_TeacherManager instance;

    private void Awake()
    {
        instance = this;
    }

    //[HideInInspector]
    //public new UserInfo userInfo;

    // ���� �ִ� �ο� �ľ��ϱ�
    public new int liveCount = 0;

    public new Vector3[] spawnPos;

    public override void CreateAllUser()
    {
        spawnPos = new Vector3[10];

        // ����Ʈ�� �������� ��ġ�����ϰ�
        for (int i = 0; i < 10; i++)
        {
            spawnPos[i] = Vector3.zero + new Vector3(Random.Range(-3, 3), 3, Random.Range(0, -10));
        }

        // ������ ������ �ο�
        liveCount = PhotonNetwork.CountOfPlayers;

        // �ϴ� ť���������
        if (!createBook)
        {
            me = PhotonNetwork.Instantiate("YJ/Player", spawnPos[liveCount], Quaternion.identity);
        }
    }

    public override void JoinRoom()
    {
        //PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.photonView.RPC("RPCLeaveRoom", RpcTarget.All);
            print("out!!!");
        }
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
