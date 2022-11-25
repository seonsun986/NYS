using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;


public class YJ_UIManager_Plaza : MonoBehaviourPun
{

    // 닉네임
    public Text nickName;

    // 방만들기 버튼
    public GameObject createRoom_T_only;
    private void Start()
    {
        // 닉네임 넣어주기
        nickName.text = PhotonNetwork.NickName;

        // 선생님일때만 방만들기 버튼 보여주기
        if (YJ_DataManager.instance.myInfo.memberRole != "TEACHER")
        {
            createRoom_T_only.SetActive(false);
        }
    }

    #region 마이룸이동
    public void GoMyRoom()
    {

    }
    #endregion


    #region 환경설정
    public GameObject settingSet;
    //int settingCount = 0;

    public void Setting()
    {
        settingSet.SetActive(!settingSet.activeSelf);
        bgmOnOff.isOn = YJ_AudioManager.instance.bgmOnOff;
        effectOnOff.isOn = YJ_AudioManager.instance.effectOnOff;
    }
    #endregion

    #region 방목록
    public GameObject roomList;
    public RectTransform roomPos1;
    public RectTransform roomPos2;
    private bool isShow = false;
    public bool IsShow
    {
        get
        {
            if (isShow)
                isShow = false;
            else
                isShow = true;
            return isShow;
        }
    }

    public void RoomList()
    {
        if (!IsShow)
        {
            roomList.GetComponent<RectTransform>().anchoredPosition = roomPos2.anchoredPosition;
        }
        else
        {
            iTween.ScaleFrom(roomList, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
            roomList.GetComponent<RectTransform>().anchoredPosition = roomPos1.anchoredPosition;
        }
    }
    #endregion

    #region 방에 들어가시겠습니까?
    public GameObject auIn;
    public void AUIn()
    {
        auIn.SetActive(!auIn.activeSelf);
    }

    // O버튼을 눌렀을때 실행할 것
    public string goingRoomName;
    public int goingRoomType;
    public void EnterRoom()
    {
        YJ_PlazaManager.instance.goingRoom = YJ_DataManager.instance.goingRoomName;
        YJ_PlazaManager.instance.goingRoomType = YJ_DataManager.instance.goingRoomType;
        YJ_PlazaManager.instance.OutPlaza();
    }
    #endregion

    #region 방만들기
    // 방만들기 버튼 누르기
    public GameObject createRoomSet;
    int createCount = 0;
    public void CreateRoomBT()
    {
        createRoomSet.SetActive(!createRoomSet.activeSelf);
    }



    public Button creatRoom;
    public InputField roomName;
    public InputField roomPw;
    public InputField roomNumber;
    public int roomType = 0;


    // 방만들기 버튼을 눌렀을때
    public void CreateRoom()
    {
        YJ_DataManager.CreateRoomInfo.roomName = roomName.text;
        YJ_DataManager.CreateRoomInfo.roomPw = roomPw.text;
        int.TryParse(roomNumber.text, out YJ_DataManager.CreateRoomInfo.roomNumber);
        YJ_DataManager.CreateRoomInfo.roomType = roomType;

        print("방이름 : " + YJ_DataManager.CreateRoomInfo.roomName + " 비밀번호 : " + YJ_DataManager.CreateRoomInfo.roomPw + " 인원 : " + YJ_DataManager.CreateRoomInfo.roomNumber + " 방 타입 : " + YJ_DataManager.CreateRoomInfo.roomType);

        createRoomSet.SetActive(false);
    }



    #region 룸타입 버튼
    public Toggle type1;
    public Toggle type2;
    public Toggle type3;

    public void RoomType1()
    {
        if (type1.isOn)
        {
            roomType = 1;
            type2.interactable = false;
            type3.interactable = false;
        }
        else if (!type1.isOn)
        {
            roomType = 0;
            type2.interactable = true;
            type3.interactable = true;
        }
    }

    public void RoomType2()
    {
        if (type2.isOn)
        {
            roomType = 2;
            type1.interactable = false;
            type3.interactable = false;
        }
        else if (!type2.isOn)
        {
            roomType = 0;
            type1.interactable = true;
            type3.interactable = true;
        }
    }

    public void RoomType3()
    {
        if (type3.isOn)
        {
            roomType = 3;
            type1.interactable = false;
            type2.interactable = false;
        }
        else if (!type3.isOn)
        {
            roomType = 0;
            type1.interactable = true;
            type2.interactable = true;
        }
    }

    #endregion

    #endregion

    #region BGM On/Off
    public Toggle bgmOnOff;
    public GameObject bgmOn, bgmOff;
    // 228, 291
    // 배경음악 On/Off
    public void BGMOnAndOff()
    {
        if (bgmOnOff.isOn)
        {
            YJ_AudioManager.instance.bgmOnOff = true;
            //Camera.main.GetComponent<AudioSource>().volume = 1;
            bgmOff.SetActive(false);
            bgmOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.bgmOnOff = false;
            //Camera.main.GetComponent<AudioSource>().volume = 0;
            bgmOn.SetActive(false);
            bgmOff.SetActive(true);
        }
    }
    #endregion

    #region EffectSound On/Off
    public Toggle effectOnOff;
    public GameObject effectOn, effectOff;

    GameObject canvas;

    // 228, 291
    // 배경음악 On/Off
    public void EffectOnAndOff()
    {
        canvas = GameObject.Find("Canvas");
        if (effectOnOff.isOn)
        {
            YJ_AudioManager.instance.effectOnOff = true;
            //canvas.GetComponent<AudioSource>().volume = 1;
            effectOff.SetActive(false);
            effectOn.SetActive(true);
        }
        else
        {
            YJ_AudioManager.instance.effectOnOff = false;
            //canvas.GetComponent<AudioSource>().volume = 0;
            effectOn.SetActive(false);
            effectOff.SetActive(true);
        }
    }
    #endregion

    #region Invitation On/Off
    public Toggle invitOnOff;
    public GameObject invitOn, invitOff;
    // 228, 291
    // 배경음악 On/Off
    public void invitOnAndOff()
    {
        if (invitOnOff.isOn)
        {
            invitOff.SetActive(false);
            invitOn.SetActive(true);
        }
        else
        {
            invitOn.SetActive(false);
            invitOff.SetActive(true);
        }
    }
    #endregion

    #region 게임종료버튼

    public void GameExit()
    {
        Application.Quit();
    }

    #endregion


    public GameObject contentPos;
    public void NextBtn(float change)
    {
        float x = contentPos.GetComponent<RectTransform>().anchoredPosition.x;
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", x, "to", x + change, "time", 0.3f,
            "onupdatetarget", gameObject, "onupdate", "GoMove"));
    }

    void GoMove(float v)
    {
        RectTransform rt = contentPos.GetComponent<RectTransform>();
        Vector2 v2 = rt.anchoredPosition;
        v2.x = v;
        rt.anchoredPosition = v2;

    }
}
