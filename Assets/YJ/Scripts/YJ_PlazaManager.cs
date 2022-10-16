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
        PhotonNetwork.Instantiate("Cube", spawnPos[liveCount], Quaternion.identity);

    }

}
