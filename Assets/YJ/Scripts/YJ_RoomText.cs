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

    void Start()
    {
        print("나 생성됐어?");
        if (photonView.IsMine)
            roomSet = YJ_PlazaManager.instance.roomSet;

        //roomSet = GameObject.Find("Content");
        //transform.SetParent(roomSet.transform);
        //transform.localScale = Vector3.one;
    }

    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 1 && currentTime < 2)
        {
            if (photonView.IsMine)
                photonView.RPC("RpcPos", RpcTarget.All, roomSet);
        }
    }

    [PunRPC]
    void RpcPos(GameObject set)
    {
        roomSet = set;
        transform.parent = roomSet.transform;
        //transform.SetParent(roomSet.transform);
    }


    public void SetInfo(string roomName, string nickName)
    {
        roomText.text = roomName + " (" + nickName + "선생님 ) ";
    }
}
