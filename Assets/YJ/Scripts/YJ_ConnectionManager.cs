using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class YJ_ConnectionManager : MonoBehaviourPunCallbacks
{
    // ���̵� InputField
    public InputField inputId;

    // ��й�ȣ InputField
    public InputField inputPw;

    // ���ӹ�ư (ID�� PW�� �Է����� ������ �۵����� �ʰ��� ��)
    public Button btnLogin;

    void Start()
    {
        // ���̵�, ��й�ȣ �Է°��� ����� �� ȣ��Ǵ� �Լ� ���
        inputId.onValueChanged.AddListener(OnValueChanged_ID);
        inputPw.onValueChanged.AddListener(OnValueChanged_PW);

        // �� �Է��� ���̵�, ��й�ȣ�� �����صα� ���� �Լ�
        //inputId.onEndEdit.AddListener(Login_ID);
        //inputPw.onEndEdit.AddListener(Login_PW);
    }

    #region ��ư Ȱ��ȭ��ų �Լ�
    bool id_input = false;
    bool pw_input = false;
    public void OnValueChanged_ID(string s)
    {
        // ���̵� �Է� Ȯ��
        id_input = s.Length > 0;

        //print("ID : " + s);

        // ��ưȰ��ȭ
        btnLogin.interactable = id_input && pw_input;
    }

    public void OnValueChanged_PW(string s)
    {
        // ��й�ȣ �Է�Ȯ��
        pw_input = s.Length > 0;

        //print("PW : " + s);

        // ��ưȰ��ȭ
        btnLogin.interactable = id_input && pw_input;
    }
    #endregion

    #region ������ ���� ID, PW
    //LoginInfo loginInfo = new LoginInfo();

    //public void Login_ID(string s)
    //{
    //    // ������ ���� ���̵�
    //    loginInfo.ID = s;
    //    print("ID : " + loginInfo.ID);
    //}

    //public void Login_PW(string s)
    //{
    //    // ������ ���� ��й�ȣ
    //    loginInfo.PW = s;
    //    print("ID : " + loginInfo.PW);
    //}
    #endregion

    public void OnSubmit(string s)
    {
        // �� �� �Է��� �Ǿ�����
        if (id_input && pw_input)
        {
            // ��������
            OnClickConnect();
        }
    }


    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ������ ���� ����, �κ� ���� �� ���� �Ұ���
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }


    // ������ ������ ����, �κ� ���� �� ���� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // �г��� ���� ��Ʈ��ũ �ʿ�
        //PhotonNetwork.NickName = inputNickName.text; //"�͸���_" + Random.Range(1,10000);
        PhotonNetwork.NickName = inputId.text;
        print(PhotonNetwork.NickName);
        //print(photonView.name);

        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();

    }

    // �κ� ���� ���� �� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        bool roomset = false;
        print("�̰� �׳� �Ҹ��°ǰ�?");
              //  CreateRoom();
        
        
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
        // ������ ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;

        // ���� �����
        PhotonNetwork.CreateRoom("Lobby", roomOptions);

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
    public void JoinRoom()
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
        
    }
}
