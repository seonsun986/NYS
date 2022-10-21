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

    // �̵��� �� �̸�
    public string sceneName;

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

    GameObject me;
    void CreateAllUser()
    {
        spawnPos = new Vector3[10];

        // ����Ʈ�� �������� ��ġ�����ϰ�
        for(int i = 0; i < 10; i++)
        {
            spawnPos[i] = Vector3.zero + new Vector3(Random.Range(-5, 0), 3, Random.Range(-5, 0));
        }

        // ������ ������ �ο�
        liveCount = PhotonNetwork.CountOfPlayers;

        // �ϴ� ť���������
        me = PhotonNetwork.Instantiate("YJ/Cube", spawnPos[liveCount], Quaternion.identity);
        print(ParameterCode.CacheSliceIndex);
    }


    #region ���� ����
    public void DeleteRoomOBJ(int id)
    {
        //PhotonNetwork.Destroy(room.gameObject);
        photonView.RPC("RpcDeleteRoom", RpcTarget.MasterClient, id);
    }

    [PunRPC]
    void RpcDeleteRoom(int id)
    {
        print("���� ���ֶ�� " + id);
        PhotonView view = PhotonView.Find(id);
        //Destroy(view.gameObject);
        PhotonNetwork.Destroy(view.gameObject);
    }

    #endregion

    #region ����� �� �̵�

    //public GameObject[] room;
    public GameObject[] roomType = new GameObject[3];
    //GameObject roomType1, roomType2, roomType3;
    GameObject myRoom;
    public int roomViewId;

    public virtual void CreatRoom()
    {
        myRoom = PhotonNetwork.Instantiate("YJ/Type" + YJ_DataManager.CreateRoomInfo.roomType, new Vector3(Random.Range(1,5),1.5f,Random.Range(1,5)), Quaternion.identity);
        print(roomViewId);
        //photonView.RPC("RpcCreatRoom", RpcTarget.All);
        roomViewId = myRoom.GetComponent<PhotonView>().ViewID;

        PhotonNetwork.Destroy(me.gameObject);
        PhotonNetwork.LeaveRoom();

    }

    [PunRPC]
    void RpcCreatRoom()
    {
        Instantiate(roomType[YJ_DataManager.CreateRoomInfo.roomType-1], new Vector3(Random.Range(1, 5), 1.5f, Random.Range(1, 5)), Quaternion.identity);
    }

    // ������ ������ ����, �κ� ���� �� ���� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // �г��� ���� ��Ʈ��ũ �ʿ�
        //PhotonNetwork.NickName = inputNickName.text; //"�͸���_" + Random.Range(1,10000);

        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� ���� �� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        CreateRoom();
    }

    public void CreateRoom()
    {
        // ������ ����
        RoomOptions roomOptions = new RoomOptions();

        // ���� �����
        PhotonNetwork.CreateRoom(YJ_DataManager.CreateRoomInfo.roomName, roomOptions);
        print(YJ_DataManager.CreateRoomInfo.roomName);

    }

    // �� ���� �Ϸ� Ȯ��
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // �� ���� ����������
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    // ������ ( ������ڴ� �ڵ����� ������ �� )
    public virtual void JoinRoom()
    {
        // XR_A��� ������ ����
        PhotonNetwork.JoinRoom(YJ_DataManager.CreateRoomInfo.roomName);
    }


    // �����忡 ���������� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // LobbyScene �̵�
        PhotonNetwork.LoadLevel(ChangeSceneName());
        YJ_DataManager.instance.changeScene++;
        //Destroy(this);
    }

    // �̵��� �� ���� ����
    public virtual string ChangeSceneName()
    {
        // �׸� �����Ǹ� �׸��� ���� CreatRoom �Լ����� sceneName �������ֱ�
        sceneName = "TeacherScene";
        return sceneName;
    }
    #endregion
}
