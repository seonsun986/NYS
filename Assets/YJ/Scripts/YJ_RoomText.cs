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

    void Start()
    {
        // �θ� �� Content ã��
        roomSet = GameObject.Find("Canvas").transform.Find("RoomList").transform.Find("RoomListSet").transform.Find("Viewport").transform.Find("Content").gameObject;

        // �θ�����
        transform.SetParent(roomSet.transform);
        transform.localScale = Vector3.one;

        if (photonView.IsMine)
        {
            roomNameSet = YJ_DataManager.CreateRoomInfo.roomName + " (" + PhotonNetwork.NickName + "������ )";
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
        }
    }

    // RPC ����
    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 0.5 && currentTime < 1)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcRoomSet", RpcTarget.All, roomNameSet, roomName, roomType);
        }
    }

    // �ؽ�Ʈ �������� RPC ���
    [PunRPC]
    void RpcRoomSet(string roomSet, string name, int type)
    {
        roomNameSet = roomSet;
        transform.GetChild(0).GetComponent<Text>().text = roomNameSet;

        roomName = name;
        roomType = type;
    }

    string roomName;
    int roomType;

    // ����Ʈ �������� ���̵��ϱ�
    public void OnClickRoomList()
    {
    //    if (photonView.IsMine)
    //    {
            YJ_PlazaManager.instance.goingRoom = roomName;
            YJ_PlazaManager.instance.goingRoomType = roomType;
            YJ_PlazaManager.instance.OutPlaza();
    //    }
    }
}
