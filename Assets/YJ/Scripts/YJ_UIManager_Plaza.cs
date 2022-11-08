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

    // �г���
    public Text nickName;

    // �游��� ��ư
    public GameObject createRoom_T_only;
    private void Start()
    {
        // �г��� �־��ֱ�
        nickName.text = PhotonNetwork.NickName;

        // �������϶��� �游��� ��ư �����ֱ�
        if (UserInfo.memberRole != "TEACHER")
        {
            createRoom_T_only.SetActive(false);
        }
    }

    #region ���̷��̵�
    public void GoMyRoom()
    {

    }
    #endregion


    #region ȯ�漳��
    public GameObject settingSet;
    //int settingCount = 0;

    public void Setting()
    {
        settingSet.SetActive(!settingSet.activeSelf);
    }
    #endregion

    #region ����
    public GameObject roomList;

    public void RoomList()
    {
        roomList.SetActive(!roomList.activeSelf);
    }
    #endregion

    #region �濡 ���ðڽ��ϱ�?
    public GameObject auIn;
    public void AUIn()
    {
        
        auIn.SetActive(!auIn.activeSelf);
    }

    // O��ư�� �������� ������ ��
    public string goingRoomName;
    public int goingRoomType;
    public void EnterRoom()
    {
        YJ_PlazaManager.instance.goingRoom = YJ_DataManager.instance.goingRoomName;
        YJ_PlazaManager.instance.goingRoomType = YJ_DataManager.instance.goingRoomType;
        YJ_PlazaManager.instance.OutPlaza();
    }
    #endregion



    #region �游���
    // �游��� ��ư ������
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


    // �游��� ��ư�� ��������
    public void CreateRoom()
    {
        YJ_DataManager.CreateRoomInfo.roomName = roomName.text; 
        YJ_DataManager.CreateRoomInfo.roomPw = roomPw.text;
        int.TryParse(roomNumber.text, out YJ_DataManager.CreateRoomInfo.roomNumber);
        YJ_DataManager.CreateRoomInfo.roomType = roomType;

        print("���̸� : " + YJ_DataManager.CreateRoomInfo.roomName + " ��й�ȣ : " + YJ_DataManager.CreateRoomInfo.roomPw + " �ο� : " + YJ_DataManager.CreateRoomInfo.roomNumber + " �� Ÿ�� : " + YJ_DataManager.CreateRoomInfo.roomType);

        createRoomSet.SetActive(false);
    }



    #region ��Ÿ�� ��ư
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
        else if(!type1.isOn)
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
    // ������� On/Off
    public void BGMOnAndOff()
    {
        if (bgmOnOff.isOn)
        {
            bgmOff.SetActive(false);
            bgmOn.SetActive(true);
        }
        else
        {
            bgmOn.SetActive(false);
            bgmOff.SetActive(true);
        }
    }
    #endregion

    #region EffectSound On/Off
    public Toggle effectOnOff;
    public GameObject effectOn, effectOff;
    // 228, 291
    // ������� On/Off
    public void EffectOnAndOff()
    {
        if (effectOnOff.isOn)
        {
            effectOff.SetActive(false);
            effectOn.SetActive(true);
        }
        else
        {
            effectOn.SetActive(false);
            effectOff.SetActive(true);
        }
    }
    #endregion

    #region Invitation On/Off
    public Toggle invitOnOff;
    public GameObject invitOn, invitOff;
    // 228, 291
    // ������� On/Off
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

    #region ���������ư

    public void GameExit()
    {
        Application.Quit();
    }

    #endregion
}
