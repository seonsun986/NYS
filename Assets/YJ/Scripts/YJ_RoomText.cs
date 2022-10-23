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
        print("나 생성됐어?");
        //if (photonView.IsMine)
        //    roomSet = YJ_PlazaManager.instance.roomSet;

        roomSet = GameObject.Find("Canvas").transform.Find("RoomList").transform.Find("RoomListSet").transform.Find("Viewport").transform.Find("Content").gameObject;

        //roomSet = GameObject.Find("Content");
        transform.SetParent(roomSet.transform);
        transform.localScale = Vector3.one;

        if (photonView.IsMine)
            roomNameSet = YJ_DataManager.CreateRoomInfo.roomName + " (" + PhotonNetwork.NickName + "선생님 )";
    }

    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 0.5 && currentTime < 1)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcRoomSet", RpcTarget.All, roomNameSet);
        }
    }

    [PunRPC]
    void RpcRoomSet(string roomSet)
    {
        roomNameSet = roomSet;
        transform.GetChild(0).GetComponent<Text>().text = roomNameSet;
    }

}
