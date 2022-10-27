using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_RoomTrigger : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
            
            photonView.RPC("RpcNameSet", RpcTarget.All, roomName, roomType);
        }
    }

    void Update()
    {
        //currentTime += Time.deltaTime;
        //if (currentTime > 0.3 && currentTime < 0.8)
        //{
        //    if (photonView.IsMine)
        //        photonView.RPC("RpcNameSet", RpcTarget.All, roomName);
        //}

        if(canJoinRoom)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                YJ_PlazaManager.instance.goingRoom = roomName;
                YJ_PlazaManager.instance.goingRoomType = roomType;
                YJ_PlazaManager.instance.OutPlaza();
                //PhotonNetwork.JoinLobby();
                //JoinRoom();
            }
        }
    }

    [PunRPC]
    void RpcNameSet(string name, int type)
    {
        roomName = name;
        roomType = type;
    }

    public string roomName;
    public int roomType;

    bool canJoinRoom;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && photonView.IsMine)
        {
            canJoinRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            canJoinRoom = false;
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
    //    JoinRoom();
    //}

    //// 방입장 ( 방생성자는 자동으로 입장이 됨 )
    //public virtual void JoinRoom()
    //{
    //    // XR_A라는 방으로 입장
    //    PhotonNetwork.JoinRoom(roomName);
    //}

    //// 방입장에 성공했을때 불리는 함수
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();

    //    // LobbyScene 이동
    //    PhotonNetwork.LoadLevel("TeacherScene");
    //}
}
