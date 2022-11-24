using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YJ_Player_ChangeAvatar : YJ_AvatarSet
{
    public static YJ_Player_ChangeAvatar instance;
    // ���͸���
    SkinnedMeshRenderer mt;

    bool role = false;

    private void Awake()
    {
        instance = this;
    }

    public override void Start()
    {
        base.Start();

        userInfo = YJ_DataManager.instance.myInfo;

        AvtSet();
        mt = avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        avatarNum = int.Parse(userInfo.animal);
        setmt = int.Parse(userInfo.material);
    }

    void Update()
    {
        
    }

    bool RemoveChild(Transform parent)
    {
        bool isExist = false;
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
            isExist = true;
        }
        return isExist;
    }

    #region �ƹ�Ÿ ����
    public int avatarNum;
    public void OnClickAvatar(int i)
    {
        RemoveChild(hatParent);
        RemoveChild(bagParent);
        RemoveChild(fishParent);
        RemoveChild(glassParent);
        
        avt.SetActive(false);
        avt = avatar[i];
        mt = avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        avt.SetActive(true);
        avatarNum = i;
        setmt = 8;

        SetItemParent();
        bagId = hatId = fishId = glassId = 8;

        //�հ��� �ٽ� ���������
        if (userInfo.memberRole == "TEACHER")
        {
            crown.SetActive(true);
        }
    }
    #endregion

    #region ���̺���
    // Mt ��ư
    public int setmt = 0;
    public void OnClickCatMt(int i)
    {
        if (avatarNum == 0)
        {
            mt.material = catMt[i];
        }
        else if (avatarNum == 1)
        {
            mt.material = bearMt[i];
        }
        else if (avatarNum == 2)
        {
            mt.material = bunnyMt[i];
        }

        setmt = i;
    }
    #endregion

    #region ������Ʈ �ٹ̱�

    // obj ��ư
      

    public void OnClickObj(int i)
    {
        if (i < 2)
        {
            RemoveChild(bagParent);
            if(bagId != i)
            {
                Instantiate(obj[i].gameObject, bagParent);
                bagId = i;
            }
            else
            {
                bagId = 8;
            }
        }

        else if(i < 4)
        {
            RemoveChild(fishParent);
            if (fishId != i)
            {
                Instantiate(obj[i].gameObject, fishParent);
                fishId = i;
            }
            else
            {
                fishId = 8;
            }
        }
        else if(i < 6)
        {
            RemoveChild(hatParent);
            if (hatId != i)
            {
                Instantiate(obj[i].gameObject, hatParent);
                hatId = i;
            }
            else
            {
                hatId = 8;
            }
        }
        else if(i < 8)
        {
            RemoveChild(glassParent);
            if (glassId != i)
            {
                Instantiate(obj[i].gameObject, glassParent);
                glassId = i;
            }
            else
            {
                glassId = 8;
            }
        }
    }
    #endregion

    #region �ڷΰ��� (����)
    public void OnclickSave()
    {
        AvatarChange();
    }
    public void AvatarChange()
    {
        JObject jsonData = new JObject();
        jsonData["animal"] = avatarNum.ToString();
        jsonData["material"] = setmt.ToString();

        JObject objData = new JObject();
        objData["bag"] = bagId;
        objData["fish"] = fishId;
        objData["hat"] = hatId;
        objData["glass"] = glassId;

        jsonData["objectName"] = objData.ToString();

        print(jsonData.ToString());
        
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/avatar/update";
        requester.requestType = RequestType.PUT;
        requester.postData = jsonData.ToString();
        requester.onComplete = (handler) => {
            print("ĳ���� ���� ����Ϸ�");
            userInfo.animal = avatarNum.ToString();
            userInfo.material = setmt.ToString();
            userInfo.objectName = objData.ToString();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }
       
    #endregion
}
