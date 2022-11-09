using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.TextCore.Text;
using Photon.Realtime;

public class YJ_PlazaManager : MonoBehaviourPunCallbacks
{
    public static YJ_PlazaManager instance;
    private void Awake()
    {
        instance = this;

    }


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
        //PhotonNetwork.AutomaticallySyncScene = true;

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
        if (!createBook)
        {
            me = PhotonNetwork.Instantiate("YJ/Player", spawnPos[liveCount], Quaternion.identity);
            //me = PhotonNetwork.Instantiate("YJ/Player", Vector3.zero, Quaternion.identity);
        }

    }


    #region ���� ����
    public void DeleteRoomOBJ(int objId, int listId)
    {
        //PhotonNetwork.Destroy(room.gameObject);
        photonView.RPC("RpcDeleteRoom", RpcTarget.MasterClient, objId, listId);
    }

    [PunRPC]
    void RpcDeleteRoom(int objId, int listId)
    {
        PhotonView objView = PhotonView.Find(objId);
        PhotonView listView = PhotonView.Find(listId);
        //Destroy(view.gameObject);
        PhotonNetwork.Destroy(objView.gameObject);
        PhotonNetwork.Destroy(listView.gameObject);
    }

    #endregion

    #region ����� �� �̵�

    //public GameObject[] room;
    public GameObject[] roomType = new GameObject[3];
    //GameObject roomType1, roomType2, roomType3;
    GameObject myRoom;
    public int roomViewId;
    public int roomListViewId;

    public GameObject roomList;
    GameObject roomSet;

    float setTime = 0;
    private void Update()
    {
        // �̰������ ���� ���ŵ�.. �ѹ��� �ʰ� ����
        if (roomSet != null && myRoom != null)
        {
            setTime += Time.deltaTime;

            if (setTime > 1)
            {
                OutPlaza();

                setTime = 0;

            }
        }

    }

    public void OutPlaza()
    { 
        // �� ���� ������Ʈ ���ֱ�
        PhotonNetwork.Destroy(me.gameObject);
        // ����� �� ������
        PhotonNetwork.LeaveRoom();
    }

    public virtual void CreatRoom()
    {
        // �������Ʈ ����
        myRoom = PhotonNetwork.Instantiate("YJ/Type" + YJ_DataManager.CreateRoomInfo.roomType, new Vector3(Random.Range(1,5),3f,Random.Range(1,5)), Quaternion.identity);

        
        //photonView.RPC("RpcCreatRoom", RpcTarget.All);

        // ���Ͽ� ����Ʈ ����
        roomSet = PhotonNetwork.Instantiate("YJ/RoomItem", new Vector3(0, 0, 0), Quaternion.identity);

        // �� ����Ʈ�� �ѹ� ����� �ؽ�Ʈ�� ���ŵ�
        roomList.SetActive(true);
        //roomList.SetActive(false);


        // �������Ʈ �� ����Ʈ ViewId ����
        roomViewId = myRoom.GetComponent<PhotonView>().ViewID;
        roomListViewId = roomSet.GetComponent<PhotonView>().ViewID;
    }

    // ��ȭ����� ��ư
    bool createBook = false;
    public void OnClickCreateBook()
    {
        if (!createBook)// && photonView.IsMine)
        {
            createBook = true;
            YJ_DataManager.CreateRoomInfo.roomName = PhotonNetwork.NickName + "createBook";
            OutPlaza();
        }
    }

    // ���̷� ��ư
    bool goMyRoom = false;
    public void OnClickMyRoom()
    {
        if (!goMyRoom)
        {
            goMyRoom = true;
            YJ_DataManager.CreateRoomInfo.roomName = PhotonNetwork.NickName + "MyRoom";
            OutPlaza();
        }
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
        if (YJ_DataManager.CreateRoomInfo.roomName != null)
            CreateRoom();
        else
            JoinRoom();
    }

    public void CreateRoom()
    {
        // ������ ����
        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = (byte)YJ_DataManager.CreateRoomInfo.roomNumber;

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

    public string goingRoom;
    public int goingRoomType;

    // ������ ( ������ڴ� �ڵ����� ������ �� )
    public virtual void JoinRoom()
    {
        // XR_A��� ������ ����
        if(YJ_DataManager.CreateRoomInfo.roomName != null)
            PhotonNetwork.JoinRoom(YJ_DataManager.CreateRoomInfo.roomName);
        else
        {
            PhotonNetwork.JoinRoom(goingRoom);
            YJ_DataManager.CreateRoomInfo.roomType = goingRoomType;
        }

    }


    // �����忡 ���������� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if(YJ_DataManager.CreateRoomInfo.roomName != null)
            YJ_DataManager.instance.changeScene++;

        // LobbyScene �̵�
        PhotonNetwork.LoadLevel(ChangeSceneName());
        //Destroy(this);
    }

    // �̵��� �� ���� ����
    public virtual string ChangeSceneName()
    {
        // �׸� �����Ǹ� �׸��� ���� CreatRoom �Լ����� sceneName �������ֱ�
        if (createBook)
        {
            sceneName = "EditorScene";
            YJ_DataManager.CreateRoomInfo.roomName = null;
            createBook = false;
        }
        else if(goMyRoom)
        {
            sceneName = "MyRoomScene";
            YJ_DataManager.CreateRoomInfo.roomName = null;
            goMyRoom = false;
        }
        else if(YJ_DataManager.CreateRoomInfo.roomType == 1)
        {
            sceneName = "TeacherScene(Candy)";
            YJ_DataManager.CreateRoomInfo.roomType = 0;
        }
        else if (YJ_DataManager.CreateRoomInfo.roomType == 2)
        {
            sceneName = "TeacherScene(ClassRoom)";
            YJ_DataManager.CreateRoomInfo.roomType = 0;
        }
        else if (YJ_DataManager.CreateRoomInfo.roomType == 3)
        {
            sceneName = "TeacherScene(Christmas)";
            YJ_DataManager.CreateRoomInfo.roomType = 0;
        }
        return sceneName;
    }
    #endregion
}
