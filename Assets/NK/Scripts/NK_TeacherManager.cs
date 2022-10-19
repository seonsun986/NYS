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

    [HideInInspector]
    public new UserInfo userInfo;

    // ���� �ִ� �ο� �ľ��ϱ�
    public new int liveCount = 0;

    // �÷��̾� ���� ����Ʈ
    List<PhotonView> playerList = new List<PhotonView> ();

    public new Vector3[] spawnPos;

/*    public override void CreatRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("PlazaScene");

    }*/
}
