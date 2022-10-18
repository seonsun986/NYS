using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class YJ_UIManager_Plaza : MonoBehaviour
{

    #region ȯ�漳��
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

    #region �游���
    // �游��� ��ư ������
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



    public Button creatRoom;
    public InputField roomName;
    public InputField roomPw;
    public InputField roomNumber;
    public int roomType = 0;

    public static YJ_DataManager.CreateRoomInfo roomInfo = new YJ_DataManager.CreateRoomInfo();

    // �游��� ��ư�� ��������
    public void CreateRoom()
    {
        roomInfo.roomName = roomName.text;
        roomInfo.roomPw = roomPw.text;
        int.TryParse(roomNumber.text, out roomInfo.roomNumber);
        roomInfo.roomType = roomType;

        print("���̸� : " + roomInfo.roomName + " ��й�ȣ : " + roomInfo.roomPw + " �ο� : " + roomInfo.roomNumber + " �� Ÿ�� : " + roomInfo.roomType);

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
