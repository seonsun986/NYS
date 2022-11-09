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


    // 들어와있는 인원 파악하기
    public int liveCount = 0;

    // 플레이어 접속 리스트
    List<PhotonView> playerList = new List<PhotonView>();

    public Vector3[] spawnPos;

    // 이동할 씬 이름
    public string sceneName;


    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        //PhotonNetwork.AutomaticallySyncScene = true;

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

    GameObject me;
    void CreateAllUser()
    {
        spawnPos = new Vector3[10];

        // 리스트에 랜덤으로 위치생성하고
        for(int i = 0; i < 10; i++)
        {
            spawnPos[i] = Vector3.zero + new Vector3(Random.Range(-5, 0), 3, Random.Range(-5, 0));
        }

        // 서버에 접속한 인원
        liveCount = PhotonNetwork.CountOfPlayers;

        // 일단 큐브생성하자
        if (!createBook)
        {
            me = PhotonNetwork.Instantiate("YJ/Player", spawnPos[liveCount], Quaternion.identity);
            //me = PhotonNetwork.Instantiate("YJ/Player", Vector3.zero, Quaternion.identity);
        }

    }


    #region 내방 삭제
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

    #region 방생성 후 이동

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
        // 이거해줘야 방목록 갱신됨.. 한박자 늦게 들어가기
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
        // 내 게임 오브젝트 없애기
        PhotonNetwork.Destroy(me.gameObject);
        // 광장씬 방 나가기
        PhotonNetwork.LeaveRoom();
    }

    public virtual void CreatRoom()
    {
        // 방오브젝트 생성
        myRoom = PhotonNetwork.Instantiate("YJ/Type" + YJ_DataManager.CreateRoomInfo.roomType, new Vector3(Random.Range(1,5),3f,Random.Range(1,5)), Quaternion.identity);

        
        //photonView.RPC("RpcCreatRoom", RpcTarget.All);

        // 방목록에 리스트 생성
        roomSet = PhotonNetwork.Instantiate("YJ/RoomItem", new Vector3(0, 0, 0), Quaternion.identity);

        // 룸 리스트를 한번 띄워야 텍스트가 갱신됨
        roomList.SetActive(true);
        //roomList.SetActive(false);


        // 방오브젝트 및 리스트 ViewId 저장
        roomViewId = myRoom.GetComponent<PhotonView>().ViewID;
        roomListViewId = roomSet.GetComponent<PhotonView>().ViewID;
    }

    // 동화만들기 버튼
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

    // 마이룸 버튼
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




    // 마스터 서버에 접속, 로비 생성 및 진입 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 닉네임 설정 네트워크 필요
        //PhotonNetwork.NickName = inputNickName.text; //"익명의_" + Random.Range(1,10000);

        // 기본 로비 진입
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 성공 시 호출
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
        // 방정보 셋팅
        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = (byte)YJ_DataManager.CreateRoomInfo.roomNumber;

        // 방을 만든다
        PhotonNetwork.CreateRoom(YJ_DataManager.CreateRoomInfo.roomName, roomOptions);
        print(YJ_DataManager.CreateRoomInfo.roomName);

    }

    // 방 생성 완료 확인
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // 방 생성 실패했을때
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    public string goingRoom;
    public int goingRoomType;

    // 방입장 ( 방생성자는 자동으로 입장이 됨 )
    public virtual void JoinRoom()
    {
        // XR_A라는 방으로 입장
        if(YJ_DataManager.CreateRoomInfo.roomName != null)
            PhotonNetwork.JoinRoom(YJ_DataManager.CreateRoomInfo.roomName);
        else
        {
            PhotonNetwork.JoinRoom(goingRoom);
            YJ_DataManager.CreateRoomInfo.roomType = goingRoomType;
        }

    }


    // 방입장에 성공했을때 불리는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if(YJ_DataManager.CreateRoomInfo.roomName != null)
            YJ_DataManager.instance.changeScene++;

        // LobbyScene 이동
        PhotonNetwork.LoadLevel(ChangeSceneName());
        //Destroy(this);
    }

    // 이동할 씬 네임 변경
    public virtual string ChangeSceneName()
    {
        // 테마 설정되면 테마에 따라 CreatRoom 함수에서 sceneName 변경해주기
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
