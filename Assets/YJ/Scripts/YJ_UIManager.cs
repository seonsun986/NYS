using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
//2021 ���� ���� ����
using Newtonsoft.Json.Linq;




public class YJ_UIManager : MonoBehaviour
{

    public GameObject loginUI;
    public GameObject signInUI;
    public GameObject InterestUI;

    public InputField login_ID;
    public InputField login_PW;

    public GameObject loginFail;

    void Start()
    {

    }

    void Update()
    {

    }

    // �α��� ��ư ��������
    public void OnclickHttp()
    {
        Login_1_API();
    }

    public void Login_1_API()
    {
        //ToJson
        JObject jsonData = new JObject();
        jsonData["memberId"] = login_ID.text;
        jsonData["memberPwd"] = login_PW.text;

        // ArrayJson -> json
        string loginJson = jsonData.ToString();// JsonUtility.ToJson(loginInfo, true);
        print(loginJson);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/auth/login";
        requester.requestType = RequestType.POST;
        requester.postData = loginJson;
        requester.onComplete = (handler) => {
            print("��ū �޾ƿ��� �Ϸ�");

            JObject tokenJson = JObject.Parse(handler.downloadHandler.text);
            
            // data �ȿ� accessToken���� ����
            YJ_DataManager.instance.myInfo.accessToken = tokenJson["data"]["accessToken"].ToString();

            // ���̽���ü�� �޾Ƽ� data ��ü�� �ް� �� �ȿ��� ������ ���� ����
            //JObject keyData = tokenJson["data"].ToObject<JObject>();

            print("�̷��� �޾ƿ��°ǰ� ? : " + YJ_DataManager.instance.myInfo.accessToken);


            Login_2_API();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }
    public void Login_2_API()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/members/optional-info";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) => {

            JObject jsonData = JObject.Parse(handler.downloadHandler.text);

            UserInfo myInfo = YJ_DataManager.instance.myInfo;
            myInfo.animal = jsonData["data"]["avatar"]["animal"].ToString();
            myInfo.material = jsonData["data"]["avatar"]["material"].ToString();
            myInfo.objectName = jsonData["data"]["avatar"]["objectName"].ToString();
            myInfo.nickname = jsonData["data"]["member"]["nickname"].ToString();
            myInfo.memberRole = jsonData["data"]["member"]["memberRole"].ToString();
            myInfo.memberCode = jsonData["data"]["member"]["memberCode"].ToString();

            GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }



    // �߸� �Է������� â ����
    public void ButtonClose()
    {
        login_ID.text = "";
        login_PW.text = "";
        loginFail.SetActive(!loginFail.activeSelf);
    }


}
