using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_RoomTrigger : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
    }

    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 0.3 && currentTime < 0.8)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcNameSet", RpcTarget.All, roomName);
        }
    }

    [PunRPC]
    void RpcNameSet(string name)
    {
        roomName = name;
    }

    public string roomName;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                YJ_PlazaManager.instance.goingRoom = roomName;
                YJ_PlazaManager.instance.OutPlaza();
                //PhotonNetwork.JoinLobby();
                //JoinRoom();
            }
        }
    }

    //// ������ ������ ����, �κ� ���� �� ���� ����
    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);

    //    // �г��� ���� ��Ʈ��ũ �ʿ�
    //    //PhotonNetwork.NickName = inputNickName.text; //"�͸���_" + Random.Range(1,10000);

    //    // �⺻ �κ� ����
    //    PhotonNetwork.JoinLobby();
    //}

    //// �κ� ���� ���� �� ȣ��
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //    JoinRoom();
    //}

    //// ������ ( ������ڴ� �ڵ����� ������ �� )
    //public virtual void JoinRoom()
    //{
    //    // XR_A��� ������ ����
    //    PhotonNetwork.JoinRoom(roomName);
    //}

    //// �����忡 ���������� �Ҹ��� �Լ�
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();

    //    // LobbyScene �̵�
    //    PhotonNetwork.LoadLevel("TeacherScene");
    //}
}
