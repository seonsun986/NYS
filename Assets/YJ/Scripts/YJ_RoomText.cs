using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_RoomText : MonoBehaviourPun
{
    // 방이름
    public Text roomText;
    GameObject roomSet;
    string roomNameSet;

    void Start()
    {
        // 부모가 될 Content 찾기
        roomSet = GameObject.Find("Canvas").transform.Find("RoomList").transform.Find("RoomListSet").transform.Find("Viewport").transform.Find("Content").gameObject;

        // 부모지정
        transform.SetParent(roomSet.transform);
        transform.localScale = Vector3.one;

        if (photonView.IsMine)
        {
            roomNameSet = YJ_DataManager.CreateRoomInfo.roomName + " (" + PhotonNetwork.NickName + "선생님 )";
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
        }
    }

    // RPC 전송
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

    // 텍스트 변경해줄 RPC 쏘기
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

    // 리스트 눌렀을때 방이동하기
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
