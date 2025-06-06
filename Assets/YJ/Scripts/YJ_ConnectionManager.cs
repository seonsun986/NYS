using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class YJ_ConnectionManager : MonoBehaviourPunCallbacks
{
    // 아이디 InputField
    public InputField inputId;

    // 비밀번호 InputField
    public InputField inputPw;

    // 접속버튼 (ID와 PW를 입력하지 않으면 작동되지 않게할 것)
    public Button btnLogin;

    void Start()
    {
        // 아이디, 비밀번호 입력값이 변경될 때 호출되는 함수 등록
        inputId.onValueChanged.AddListener(OnValueChanged_ID);
        inputPw.onValueChanged.AddListener(OnValueChanged_PW);


        // 다 입력한 아이디, 비밀번호를 저장해두기 위한 함수
        //inputId.onEndEdit.AddListener(Login_ID);
        //inputPw.onEndEdit.AddListener(Login_PW);
    }

    #region 버튼 활성화시킬 함수
    bool id_input = false;
    bool pw_input = false;
    public void OnValueChanged_ID(string s)
    {
        // 아이디 입력 확인
        id_input = s.Length > 0;

        //print("ID : " + s);

        // 버튼활성화
        btnLogin.interactable = id_input && pw_input;
    }

    public void OnValueChanged_PW(string s)
    {
        // 비밀번호 입력확인
        pw_input = s.Length > 0;

        //print("PW : " + s);

        // 버튼활성화
        btnLogin.interactable = id_input && pw_input;
    }
    #endregion


    public void OnSubmit()
    {
        // 접속하자
        OnClickConnect();
    }


    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속 성공, 로비 생성 및 진입 불가능
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }


    // 마스터 서버에 접속, 로비 생성 및 진입 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 닉네임 설정 네트워크 필요
        PhotonNetwork.NickName = YJ_DataManager.instance.myInfo.nickname;

        // 기본 로비 진입
        PhotonNetwork.JoinLobby();

    }

    // 로비 접속 성공 시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        bool roomset = false;
        
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].Name == "Lobby")
            {
                roomset = true;
                break;
            }
        }

        if (roomset)
        {
            JoinRoom();
        }
        else
            CreateRoom();
    }



    public void CreateRoom()
    {
        // 방정보 셋팅
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;

        // 방을 만든다
        PhotonNetwork.CreateRoom("Lobby", roomOptions);

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

    // 방입장 ( 방생성자는 자동으로 입장이 됨 )
    public void JoinRoom()
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
    }

    //public void AddPlayer(PhotonView pv)
    //{
    //    playerList.Add(pv);
    //}

    //public PhotonView GetPlayerPv(int viewID)
    //{
    //    for (int i = 0; i < playerList.Count; i++)
    //    {
    //        if (playerList[i].ViewID == viewID)
    //            return playerList[i];
    //    }
    //    return null;
    //}
   

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputId.text = "kimyj111";
            inputPw.text = "pass01";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputId.text = "kimng11";
            inputPw.text = "pass01";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputId.text = "simsh111";
            inputPw.text = "pass01";
        }
    }
}
