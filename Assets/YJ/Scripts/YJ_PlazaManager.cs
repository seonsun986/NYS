using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.TextCore.Text;
using Photon.Realtime;

public class YJ_PlazaManager : MonoBehaviourPunCallbacks
{
    public static YJ_PlazaManager instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public UserInfo userInfo;

    // 들어와있는 인원 파악하기
    public int liveCount = 0;

    // 플레이어 접속 리스트
    List<PhotonView> playerList = new List<PhotonView>();

    public Vector3[] spawnPos;



    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;

        print(liveCount);

        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        //GameObject user = GameObject.Find("UserInfo");
        //userInfo = user.GetComponent<MyUser>().userInfo;

        //GameObject users = GameObject.Find("UsersData");
        //usersData = users.GetComponent<UsersData>();

        // 플레이어 생성
        CreateAllUser();
    }

    void CreateAllUser()
    {
        spawnPos = new Vector3[10];

        // 리스트에 랜덤으로 위치생성하고
        for(int i = 0; i < 10; i++)
        {
            spawnPos[i] = Vector3.zero + new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0);
        }

        // 서버에 접속한 인원
        liveCount = PhotonNetwork.CountOfPlayers;

        // 일단 큐브생성하자
        PhotonNetwork.Instantiate("Cube", spawnPos[liveCount], Quaternion.identity);

    }

}
