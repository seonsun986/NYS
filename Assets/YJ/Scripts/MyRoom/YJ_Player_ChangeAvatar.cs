using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YJ_Player_ChangeAvatar : MonoBehaviour
{
    // ĳ���� ����
    public GameObject[] avatar;

    // ��Ƽ���� ����
    public Material[] catMt;
    public Material[] bearMt;
    public Material[] bunnyMt;
    // ������Ʈ ����
    public GameObject[] obj;

    // ĳ����
    public GameObject avt;
    // ���͸���
    SkinnedMeshRenderer mt;

    void Start()
    {
        // �� ������� ĳ���� ����
        avt = avatar[(int.Parse(UserInfo.animal))];
        avt.SetActive(true);
        mt = avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        if (avt.gameObject.name == "Cat")
        {
            mt.material = catMt[(int.Parse(UserInfo.material))];
        }
        else if (avt.gameObject.name == "Bear")
        {
            mt.material = bearMt[(int.Parse(UserInfo.material))];
        }
        else if (avt.gameObject.name == "Bunny")
        {
            mt.material = bunnyMt[(int.Parse(UserInfo.material))];
        }

    }

    void Update()
    {
        
    }

    #region �ƹ�Ÿ ����
    int avatarNum;
    public void OnClickAvatar(int i)
    {
        // �����ߴ� ������ ���ֱ�
        if (minibag0 != null)
            Destroy(minibag0);
        if (minibag1 != null)
            Destroy(minibag1);
        if (fish0 != null)
            Destroy(fish0);
        if (fish1 != null)
            Destroy(fish1);
        if (miniHat0 != null)
            Destroy(miniHat0);
        if(miniHat1 != null)
            Destroy(miniHat1);
        if (glass0 != null)
            Destroy(glass0);
        if (glass1 != null)
            Destroy(glass1);

        avt.SetActive(false);
        avt = avatar[i];
        mt = avt.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        avt.SetActive(true);
        avatarNum = i;
    }
    #endregion

    #region ���̺���
    // Mt ��ư
    int setmt = 0;
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
    GameObject bag; // obj�� �� �� (���)
    GameObject rightHand; // obj�� �� �� (������)
    GameObject head; // obj�� �� �� (����)
    GameObject eyes; // obj�� �� �� (�Ȱ�)

    // ����
    GameObject minibag0;
    GameObject minibag1;

    // ����
    GameObject fish0;
    GameObject fish1;

    // ����
    GameObject miniHat0;
    GameObject miniHat1;

    // �Ȱ�
    GameObject glass0;
    GameObject glass1;
        
    public void OnClickObj(int i)
    {
        if (i < 2)
        {
            bag = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            if (i == 0)
            {
                if (minibag0 != null)
                {
                    Destroy(minibag0);
                }
                else if (minibag1 != null)
                {
                    Destroy(minibag1);
                    minibag0 = Instantiate(obj[i].gameObject, bag.transform);
                }
                else
                {
                    minibag0 = Instantiate(obj[i].gameObject, bag.transform);
                }
            }
            else if (i == 1)
            {
                if (minibag1 != null)
                {
                    Destroy(minibag1);
                }
                else if (minibag0 != null)
                {
                    Destroy(minibag0);
                    minibag1 = Instantiate(obj[i].gameObject, bag.transform);
                }
                else
                {
                    minibag1 = Instantiate(obj[i].gameObject, bag.transform);
                }
            }
        }
        else if (i > 1 && i < 4)
        {
            rightHand = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).gameObject;

            if (i == 2)
            {
                if (fish0 != null)
                {
                    Destroy(fish0);
                }
                else if (fish1 != null)
                {
                    Destroy(fish1);
                    fish0 = Instantiate(obj[i].gameObject, rightHand.transform);
                }
                else
                {
                    fish0 = Instantiate(obj[i].gameObject, rightHand.transform);
                }
            }
            else if (i == 3)
            {
                if (fish1 != null)
                {
                    Destroy(fish1);
                }
                else if (fish0 != null)
                {
                    Destroy(fish0);
                    fish1 = Instantiate(obj[i].gameObject, rightHand.transform);
                }
                else
                {
                    fish1 = Instantiate(obj[i].gameObject, rightHand.transform);
                }
            }
        }
        else if (i > 3 && i < 6)
        {
            head = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;

            if (i == 4)
            {
                if (miniHat0 != null)
                {
                    Destroy(miniHat0);
                }
                else if (miniHat1 != null)
                {
                    Destroy(miniHat1);
                    miniHat0 = Instantiate(obj[i].gameObject, head.transform);
                }
                else
                {
                    miniHat0 = Instantiate(obj[i].gameObject, head.transform);
                }
            }
            else if (i == 5)
            {
                if (miniHat1 != null)
                {
                    Destroy(miniHat1);
                }
                else if (miniHat0 != null)
                {
                    Destroy(miniHat0);
                    miniHat1 = Instantiate(obj[i].gameObject, head.transform);
                }
                else
                {
                    miniHat1 = Instantiate(obj[i].gameObject, head.transform);
                }
            }
        }
        else if (i > 5 && i < 8)
        {
            eyes = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

            if (i == 6)
            {
                if (glass0 != null)
                {
                    Destroy(glass0);
                }
                else if (glass1 != null)
                {
                    Destroy(glass1);
                    glass0 = Instantiate(obj[i].gameObject, eyes.transform);
                }
                else
                {
                    glass0 = Instantiate(obj[i].gameObject, eyes.transform);
                }
            }
            else if (i == 7)
            {
                if (glass1 != null)
                {
                    Destroy(glass1);
                }
                else if (glass0 != null)
                {
                    Destroy(glass0);
                    glass1 = Instantiate(obj[i].gameObject, eyes.transform);
                }
                else
                {
                    glass1 = Instantiate(obj[i].gameObject, eyes.transform);
                }
            }
        }

    }
    #endregion

    #region �ڷΰ��� (����)

    public void OnclickSave()
    {
        AvatarSet avtSet = new AvatarSet();
        avtSet.animal = avatarNum.ToString();
        avtSet.metarial = setmt.ToString();
        avtSet.objectName = "0";

        //UserInfo.animal = avatarNum.ToString();
        //UserInfo.material = setmt.ToString();

        AvatarChange();
    }
    public void AvatarChange()
    {
        // ArrayJson -> json
        string tokenJson = JsonUtility.ToJson(UserInfo.accessToken, true);
        print(UserInfo.accessToken);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/avatar/update";
        requester.requestType = RequestType.PUT;
        requester.onComplete = (handler) => {
            print("���� ������!");
            
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    [Serializable]
    public class AvatarSet
    {
        public string animal;
        public string metarial;
        public string objectName;
    }

    #endregion
}
