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

    public string createRoomerCode;

    // 사운드 재생을 위한 캔버스 찾기
    GameObject canvas;
    YJ_ButtonClickSound buttonSound;

    void Start()
    {

        // 부모가 될 Content 찾기
        roomSet = GameObject.Find("Canvas").transform.Find("RoomList").transform.Find("RoomListSet").transform.Find("Viewport").transform.Find("Content").gameObject;

        // 부모지정
        transform.SetParent(roomSet.transform);
        transform.localScale = Vector3.one;

        if (photonView.IsMine)
        {
            roomNameSet = " (" + PhotonNetwork.NickName + "선생님 )";
            roomName = YJ_DataManager.CreateRoomInfo.roomName;
            roomType = YJ_DataManager.CreateRoomInfo.roomType;
            createRoomerCode = YJ_DataManager.instance.myInfo.memberCode;
        }

        canvas = GameObject.Find("Canvas");
        buttonSound = canvas.GetComponent<YJ_ButtonClickSound>();

        gameObject.GetComponent<Button>().onClick.AddListener(buttonSound.OnClickSound);

    }

    // RPC 전송
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

    // 텍스트 변경해줄 RPC 쏘기
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

    // 리스트 눌렀을때 방이동하기
    public void OnClickRoomList()
    {
        YJ_PlazaManager.instance.goingRoom = roomName;
        YJ_PlazaManager.instance.goingRoomType = roomType;
        YJ_PlazaManager.instance.OutPlaza();
    }
}
