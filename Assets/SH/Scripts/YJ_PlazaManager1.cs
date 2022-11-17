using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.TextCore.Text;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class YJ_PlazaManager1 : MonoBehaviourPunCallbacks
{
    public static YJ_PlazaManager1 instance;
    private void Awake()
    {
        instance = this;
    }


    // 이동할 씬 이름
    public string sceneName;

    void Start()
    {


        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;


    }



    //public GameObject[] room;
    public GameObject[] roomType = new GameObject[3];
    //GameObject roomType1, roomType2, roomType3;
    GameObject myRoom;
    public int roomViewId;
    public int roomListViewId;

    public GameObject roomList;
    GameObject roomSet;

    float setTime = 0;


    public void OutPlaza()
    {
        // 광장씬 방 나가기
        //PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MyRoomScene");
    }


    // 마스터 서버에 접속, 로비 생성 및 진입 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 기본 로비 진입
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 성공 시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //if (YJ_DataManager.CreateRoomInfo.roomName != null)
        //    CreateRoom();
        //else
            JoinRoom();
    }

    // 방입장 ( 방생성자는 자동으로 입장이 됨 )
    public virtual void JoinRoom()
    {
        // XR_A라는 방으로 입장
        PhotonNetwork.JoinRoom("Lobby");

    }


    // 방입장에 성공했을때 불리는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // LobbyScene 이동
        PhotonNetwork.LoadLevel("PlazaScene");
        //Destroy(this);

    }

}