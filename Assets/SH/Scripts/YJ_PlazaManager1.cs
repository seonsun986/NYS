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


    // �̵��� �� �̸�
    public string sceneName;

    void Start()
    {


        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
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
        // ����� �� ������
        //PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MyRoomScene");
    }


    // ������ ������ ����, �κ� ���� �� ���� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� ���� �� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //if (YJ_DataManager.CreateRoomInfo.roomName != null)
        //    CreateRoom();
        //else
            JoinRoom();
    }

    // ������ ( ������ڴ� �ڵ����� ������ �� )
    public virtual void JoinRoom()
    {
        // XR_A��� ������ ����
        PhotonNetwork.JoinRoom("Lobby");

    }


    // �����忡 ���������� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // LobbyScene �̵�
        PhotonNetwork.LoadLevel("PlazaScene");
        //Destroy(this);

    }

}