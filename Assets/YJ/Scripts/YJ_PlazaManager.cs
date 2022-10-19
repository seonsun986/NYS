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

    // �����ִ� �ο� �ľ��ϱ�
    public int liveCount = 0;

    // �÷��̾� ���� ����Ʈ
    List<PhotonView> playerList = new List<PhotonView>();

    public Vector3[] spawnPos;



    void Start()
    {
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;

        print(liveCount);

        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;

        //GameObject user = GameObject.Find("UserInfo");
        //userInfo = user.GetComponent<MyUser>().userInfo;

        //GameObject users = GameObject.Find("UsersData");
        //usersData = users.GetComponent<UsersData>();

        // �÷��̾� ����
        CreateAllUser();
    }

    void CreateAllUser()
    {
        spawnPos = new Vector3[10];

        // ����Ʈ�� �������� ��ġ�����ϰ�
        for(int i = 0; i < 10; i++)
        {
            spawnPos[i] = Vector3.zero + new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0);
        }

        // ������ ������ �ο�
        liveCount = PhotonNetwork.CountOfPlayers;

        // �ϴ� ť���������
        PhotonNetwork.Instantiate("YJ/Cube", spawnPos[liveCount], Quaternion.identity);
    }


    #region ����� �� �̵�

    // �� ������Ʈ ����
    public GameObject[] room;

    public void CreatRoom()
    {
        PhotonNetwork.Instantiate("YJ/Type" + YJ_UIManager_Plaza.roomInfo.roomType, new Vector3(Random.Range(-5, 5), 2, Random.Range(-5, 5)), Quaternion.identity);

        //PhotonNetwork.LeaveRoom();

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
    //    CreateRoom();
    //}

    //public void CreateRoom()
    //{
    //    // ������ ����
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = (byte)YJ_UIManager_Plaza.roomInfo.roomNumber;

    //    // ���� �����
    //    PhotonNetwork.CreateRoom(YJ_UIManager_Plaza.roomInfo.roomName, roomOptions);
    //    print(YJ_UIManager_Plaza.roomInfo.roomName);

    //}

    //// �� ���� �Ϸ� Ȯ��
    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //}

    //// �� ���� ����������
    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    print("OnCreateRoomFailed, " + returnCode + ", " + message);
    //}

    //// ������ ( ������ڴ� �ڵ����� ������ �� )
    //public void JoinRoom()
    //{
    //    // XR_A��� ������ ����
    //    PhotonNetwork.JoinRoom(YJ_UIManager_Plaza.roomInfo.roomName);
    //}


    //// �����忡 ���������� �Ҹ��� �Լ�
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();

    //    // LobbyScene �̵�
    //    PhotonNetwork.LoadLevel("TeacherScene");
    //}

    #endregion



}
