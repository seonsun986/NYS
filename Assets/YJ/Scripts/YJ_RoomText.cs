using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_RoomText : MonoBehaviourPun
{
    // ���̸�
    public Text roomText;
    GameObject roomSet;
    string roomNameSet;

    public string createRoomerCode;

    // ���� ����� ���� ĵ���� ã��
    GameObject canvas;
    YJ_ButtonClickSound buttonSound;

    void Start()
    {

        // �θ� �� Content ã��
        roomSet = GameObject.Find("Canvas").transform.Find("RoomList").transform.Find("RoomListSet").transform.Find("Viewport").transform.Find("Content").gameObject;

        // �θ�����
        transform.SetParent(roomSet.transform);
        transform.localScale = Vector3.one;

        if (photonView.IsMine)
        {
            roomNameSet = " (" + PhotonNetwork.NickName + "������ )";
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
            createRoomerCode = YJ_DataManager.instance.myInfo.memberCode;
        }

        canvas = GameObject.Find("Canvas");
        buttonSound = canvas.GetComponent<YJ_ButtonClickSound>();

        gameObject.GetComponent<Button>().onClick.AddListener(buttonSound.OnClickSound);

    }

    // RPC ����
    float currentTime;
    void Update()
    {
        Debug.Log(transform.position.x);

        currentTime += Time.deltaTime;
        if (currentTime > 0.5 && currentTime < 1)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcRoomSet", RpcTarget.All, roomNameSet, roomName, roomType);
        }

        if (transform.position.x > 37 || transform.position.x < 30)
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    // �ؽ�Ʈ �������� RPC ���
    [PunRPC]
    void RpcRoomSet(string roomSet, string name, int type)
    {
        roomNameSet = roomSet;
        transform.GetChild(2).GetComponent<Text>().text = roomNameSet;

        roomName = name;
        roomType = type;
    }

    string roomName;
    int roomType;

    // ����Ʈ �������� ���̵��ϱ�
    public void OnClickRoomList()
    {
        YJ_PlazaManager.instance.goingRoom = roomName;
        YJ_PlazaManager.instance.goingRoomType = roomType;
        YJ_PlazaManager.instance.OutPlaza();
    }
}
