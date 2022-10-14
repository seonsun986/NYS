using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
        inputId.onEndEdit.AddListener(Login_ID);
        inputPw.onEndEdit.AddListener(Login_PW);
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
    public string login_ID;
    public string login_PW;
    public void Login_ID(string s)
    {
        // ������ ���� ���̵�
        login_ID = s;

        print("ID : " + s);
    }

    public void Login_PW(string s)
    {
        // ������ ���� ��й�ȣ
        login_PW = s;

        print("ID : " + s);
    }
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

        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();

    }

    // �κ� ���� ���� �� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // LobbyScene �̵�
        //PhotonNetwork.LoadLevel("LobbyScene");
    }

    void Update()
    {
        
    }
}
