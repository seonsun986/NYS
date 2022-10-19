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
        PhotonNetwork.Instantiate("YJ/Cube", spawnPos[liveCount], Quaternion.identity);
    }


    #region 방생성 후 이동

    // 방 오브젝트 생성
    public GameObject[] room;

    public void CreatRoom()
    {
        PhotonNetwork.Instantiate("YJ/Type" + YJ_UIManager_Plaza.roomInfo.roomType, new Vector3(Random.Range(-5, 5), 2, Random.Range(-5, 5)), Quaternion.identity);

        //PhotonNetwork.LeaveRoom();

    }

    //// 마스터 서버에 접속, 로비 생성 및 진입 가능
    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);

    //    // 닉네임 설정 네트워크 필요
    //    //PhotonNetwork.NickName = inputNickName.text; //"익명의_" + Random.Range(1,10000);

    //    // 기본 로비 진입
    //    PhotonNetwork.JoinLobby();
    //}

    //// 로비 접속 성공 시 호출
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //    CreateRoom();
    //}

    //public void CreateRoom()
    //{
    //    // 방정보 셋팅
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = (byte)YJ_UIManager_Plaza.roomInfo.roomNumber;

    //    // 방을 만든다
    //    PhotonNetwork.CreateRoom(YJ_UIManager_Plaza.roomInfo.roomName, roomOptions);
    //    print(YJ_UIManager_Plaza.roomInfo.roomName);

    //}

    //// 방 생성 완료 확인
    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //}

    //// 방 생성 실패했을때
    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    print("OnCreateRoomFailed, " + returnCode + ", " + message);
    //}

    //// 방입장 ( 방생성자는 자동으로 입장이 됨 )
    //public void JoinRoom()
    //{
    //    // XR_A라는 방으로 입장
    //    PhotonNetwork.JoinRoom(YJ_UIManager_Plaza.roomInfo.roomName);
    //}


    //// 방입장에 성공했을때 불리는 함수
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();

    //    // LobbyScene 이동
    //    PhotonNetwork.LoadLevel("TeacherScene");
    //}

    #endregion



}
