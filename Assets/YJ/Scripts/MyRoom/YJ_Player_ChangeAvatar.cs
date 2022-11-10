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

    bool role = false;

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

        int objNum = int.Parse(UserInfo.objectName);
        role = (UserInfo.memberRole == "TEACHER");

        if (objNum < 2)
        {
            bag = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            if (objNum == 0)
            {
                minibag0 = Instantiate(obj[objNum].gameObject, bag.transform);
            }
            else if (objNum == 1)
            {
                minibag1 = Instantiate(obj[objNum].gameObject, bag.transform);
            }
        }
        else if (objNum > 1 && objNum < 4)
        {
            rightHand = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).gameObject;

            if (objNum == 2)
            {
                fish0 = Instantiate(obj[objNum].gameObject, rightHand.transform);
            }
            else if (objNum == 3)
            {
                fish1 = Instantiate(obj[objNum].gameObject, rightHand.transform);
            }
        }
        else if (objNum > 3 && objNum < 6)
        {
            head = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;

            if (objNum == 4)
            {
                miniHat0 = Instantiate(obj[objNum].gameObject, head.transform);
            }
            else if (objNum == 5)
            {
                miniHat1 = Instantiate(obj[objNum].gameObject, head.transform);
            }
        }
        else if (objNum > 5 && objNum < 8)
        {
            eyes = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

            if (objNum == 6)
            {
                glass0 = Instantiate(obj[objNum].gameObject, eyes.transform);
            }
            else if (objNum == 7)
            {
                glass1 = Instantiate(obj[objNum].gameObject, eyes.transform);
            }
        }

        GameObject crown;

        if (role)
        {
            crown = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            crown.SetActive(true);
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

        //�հ��� �ٽ� ���������
        GameObject crown;
        if (role)
        {
            crown = avt.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
            crown.SetActive(true);
        }
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
    
    // ���� ������Ʈ ���� �־��ֱ�
    List<int> objList = new List<int>();

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
                    objList.Remove(0);
                }
                else if (minibag1 != null)
                {
                    Destroy(minibag1);
                    objList.Remove(1);
                    minibag0 = Instantiate(obj[i].gameObject, bag.transform);
                    objList.Add(0);
                }
                else
                {
                    minibag0 = Instantiate(obj[i].gameObject, bag.transform);
                    objList.Add(0);
                }
            }
            else if (i == 1)
            {
                if (minibag1 != null)
                {
                    Destroy(minibag1);
                    objList.Remove(1);
                }
                else if (minibag0 != null)
                {
                    Destroy(minibag0);
                    objList.Remove(0);
                    minibag1 = Instantiate(obj[i].gameObject, bag.transform);
                    objList.Add(1);
                }
                else
                {
                    minibag1 = Instantiate(obj[i].gameObject, bag.transform);
                    objList.Add(1);
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
                    objList.Remove(2);
                }
                else if (fish1 != null)
                {
                    Destroy(fish1);
                    objList.Remove(3);
                    fish0 = Instantiate(obj[i].gameObject, rightHand.transform);
                    objList.Add(2);
                }
                else
                {
                    fish0 = Instantiate(obj[i].gameObject, rightHand.transform);
                    objList.Add(2);
                }
            }
            else if (i == 3)
            {
                if (fish1 != null)
                {
                    Destroy(fish1);
                    objList.Remove(3);
                }
                else if (fish0 != null)
                {
                    Destroy(fish0);
                    objList.Remove(2);
                    fish1 = Instantiate(obj[i].gameObject, rightHand.transform);
                    objList.Add(3);
                }
                else
                {
                    fish1 = Instantiate(obj[i].gameObject, rightHand.transform);
                    objList.Add(3);
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
                    objList.Remove(4);
                }
                else if (miniHat1 != null)
                {
                    Destroy(miniHat1);
                    objList.Remove(5);
                    miniHat0 = Instantiate(obj[i].gameObject, head.transform);
                    objList.Add(4);
                }
                else
                {
                    miniHat0 = Instantiate(obj[i].gameObject, head.transform);
                    objList.Add(4);
                }
            }
            else if (i == 5)
            {
                if (miniHat1 != null)
                {
                    Destroy(miniHat1);
                    objList.Remove(5);
                }
                else if (miniHat0 != null)
                {
                    Destroy(miniHat0);
                    objList.Remove(4);
                    miniHat1 = Instantiate(obj[i].gameObject, head.transform);
                    objList.Add(5);
                }
                else
                {
                    miniHat1 = Instantiate(obj[i].gameObject, head.transform);
                    objList.Add(5);
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
                    objList.Remove(6);
                }
                else if (glass1 != null)
                {
                    Destroy(glass1);
                    objList.Remove(7);
                    glass0 = Instantiate(obj[i].gameObject, eyes.transform);
                    objList.Add(6);
                }
                else
                {
                    glass0 = Instantiate(obj[i].gameObject, eyes.transform);
                    objList.Add(6);
                }
            }
            else if (i == 7)
            {
                if (glass1 != null)
                {
                    Destroy(glass1);
                    objList.Remove(7);
                }
                else if (glass0 != null)
                {
                    Destroy(glass0);
                    objList.Remove(6);
                    glass1 = Instantiate(obj[i].gameObject, eyes.transform);
                    objList.Add(7);
                }
                else
                {
                    glass1 = Instantiate(obj[i].gameObject, eyes.transform);
                    objList.Add(7);
                }
            }
        }

    }
    #endregion

    #region �ڷΰ��� (����)
    string objset;
    public AvatarSet avtSet;
    public void OnclickSave()
    {
        objset = null;
        avtSet = new AvatarSet();
        avtSet.animal = avatarNum.ToString();

        avtSet.material = setmt.ToString();

        for (int i = 0; i < objList.Count; i++)
        {
            objset += objList[i].ToString();
        }

        print("�ƹ�Ÿ���� : " + avtSet.animal + " �������� : " + avtSet.material + " ������Ʈ ���� : " + objset);

        if (objset == null)
            objset = 8.ToString();
        avtSet.objectName = objset;

        UserInfo.animal = avtSet.animal;
        UserInfo.material = avtSet.material;
        UserInfo.objectName = avtSet.objectName;


        AvatarChange();
    }
    public void AvatarChange()
    {
        // ArrayJson -> json
        string avtJson = JsonUtility.ToJson(avtSet, true);
        print(UserInfo.accessToken);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/avatar/update";
        requester.requestType = RequestType.PUT;
        requester.postData = avtJson;
        requester.onComplete = (handler) => {
            print("ĳ���� ���� ����Ϸ�");

        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    [Serializable]
    public class AvatarSet
    {
        public string animal;
        public string material;
        public string objectName;
    }

    #endregion
}
