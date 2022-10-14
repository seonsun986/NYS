using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
        inputId.onEndEdit.AddListener(Login_ID);
        inputPw.onEndEdit.AddListener(Login_PW);
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

    #region 서버에 보낼 ID, PW
    public string login_ID;
    public string login_PW;
    public void Login_ID(string s)
    {
        // 서버에 보낼 아이디
        login_ID = s;

        print("ID : " + s);
    }

    public void Login_PW(string s)
    {
        // 서버에 보낼 비밀번호
        login_PW = s;

        print("ID : " + s);
    }
    #endregion

    public void OnSubmit(string s)
    {
        // 둘 다 입력이 되었으면
        if (id_input && pw_input)
        {
            // 접속하자
            OnClickConnect();
        }
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
        //PhotonNetwork.NickName = inputNickName.text; //"익명의_" + Random.Range(1,10000);

        // 기본 로비 진입
        PhotonNetwork.JoinLobby();

    }

    // 로비 접속 성공 시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // LobbyScene 이동
        //PhotonNetwork.LoadLevel("LobbyScene");
    }

    void Update()
    {
        
    }
}
