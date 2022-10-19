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

    // 들어와 있는 인원 파악하기
    public new int liveCount = 0;

    public new Vector3[] spawnPos;

    public override void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        // XR_A라는 방으로 입장
        PhotonNetwork.JoinRoom("Lobby");
    }

    public override string ChangeSceneName()
    {
        sceneName = "PlazaScene";
        return sceneName;
    }
}
