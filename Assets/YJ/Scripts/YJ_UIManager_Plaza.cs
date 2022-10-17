using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class YJ_UIManager_Plaza : MonoBehaviour
{

    #region 환경설정
    public GameObject settingSet;
    int settingCount = 0;

    public void Setting()
    {
        if (settingCount > 0)
        {
            settingSet.SetActive(false);
            settingCount = 0;
        }
        else
        {
            settingSet.SetActive(true);
            settingCount++;
        }
    }
    #endregion

    #region 방만들기
    // 방만들기 버튼 누르기
    public GameObject createRoomSet;
    int createCount = 0;
    public void CreateRoomBT()
    {
        if (createCount > 0)
        {
            createRoomSet.SetActive(false);
            createCount = 0;
        }
        else
        {
            createRoomSet.SetActive(true);
            createCount++;
        }
    }

    // 방정보를 담을 클래스
    public class CreateRoomInfo
    {
        public string roomName;
        public string roomPw;
        public int roomNumber;
        public int roomType;
    }

    public Button creatRoom;
    public InputField roomName;
    public InputField roomPw;
    public InputField roomNumber;
    public int roomType;

    CreateRoomInfo roomInfo = new CreateRoomInfo();

    // 방만들기 버튼을 눌렀을때
    public void CreateRoom()
    {
        if (creatRoom)
        {
            roomInfo.roomName = roomName.text;
            roomInfo.roomPw = roomPw.text;
            roomInfo.roomNumber = int.Parse(roomNumber.text);
            roomInfo.roomType = roomType;
        }
    }

    #endregion

    #region BGM On/Off
    public GameObject bgmHandle;
    // 228, 291
    public void BGMOnAndOff()
    {
        if (bgmHandle.GetComponent<Toggle>().isOn)
        {
            //handle.transform.position = new Vector2(228, 19);
            bgmHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(228, 19);
        }
        else
        {
            //handle.transform.position = new Vector2(291, 19);
            bgmHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(291, 19);
        }
    }
    #endregion

    #region EffectSound On/Off
    public GameObject ESHandle;
    // 228, 291
    public void ESOnAndOff()
    {
        if (ESHandle.GetComponent<Toggle>().isOn)
        {
            //handle.transform.position = new Vector2(228, 19);
            ESHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(228, 19);
        }
        else
        {
            //handle.transform.position = new Vector2(291, 19);
            ESHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(291, 19);
        }
    }
    #endregion

    #region Invitation On/Off
    public GameObject invitation;
    // 228, 291
    public void InvOnAndOff()
    {
        if (invitation.GetComponent<Toggle>().isOn)
        {
            //handle.transform.position = new Vector2(228, 19);
            invitation.GetComponent<RectTransform>().anchoredPosition = new Vector2(228, 19);
        }
        else
        {
            //handle.transform.position = new Vector2(291, 19);
            invitation.GetComponent<RectTransform>().anchoredPosition = new Vector2(291, 19);
        }
    }
    #endregion


}
