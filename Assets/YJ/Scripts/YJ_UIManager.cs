using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class InterestJson
{
    public string ID;
    public ArrayJson arrayJson;
}

[System.Serializable]
public class ArrayJson
{
    public bool[] data;
}


public class YJ_UIManager : MonoBehaviour
{
    public LoginInfo loginInfo = new LoginInfo();
    //public UserInfo userInfo = new UserInfo();

    public GameObject loginUI;
    public GameObject signInUI;
    public GameObject InterestUI;

    public InputField login_ID;
    public InputField login_PW;

    InputField ID;
    InputField accessCode;
    InputField PW;
    InputField PWCheck;
    InputField userName;
    InputField birth;

    GameObject nextButton;
    Color nextDefaultColor;

    bool loginDataFlag = true;
    bool signDataFlag = true;
    bool interestDataFlag = true;
    bool PWSuccess = false;

    int maleButtonConut = 0;
    int femaleButtonConut = 0;


    void Start()
    {

    }

    void Update()
    {

    }


    private void SignInState()
    {
        if (signDataFlag)
        {
            ID = GameObject.Find("input_ID").GetComponent<InputField>();
            accessCode = GameObject.Find("input_AccessCode").GetComponent<InputField>();
            PW = GameObject.Find("input_PW").GetComponent<InputField>();
            PWCheck = GameObject.Find("input_PWCheck").GetComponent<InputField>();
            userName = GameObject.Find("input_Name").GetComponent<InputField>();
            birth = GameObject.Find("input_Birth").GetComponent<InputField>();
            signDataFlag = false;
        }

        

        if (!PWSuccess)
        {
            nextButton.GetComponent<Image>().color = Color.gray;
            nextButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            nextButton.GetComponent<Image>().color = nextDefaultColor;
            nextButton.GetComponent<Button>().interactable = true;
        }
    }

    private void InterestState()
    {
        if (interestDataFlag)
        {
            for (int i = 0; i < 20; i++)
            {
                //interestArray[i] = InterestUI.transform.GetChild(i + 1).gameObject;
            }

            interestDataFlag = false;
        }
    }

    bool Login()
    {
        string jsonData = JsonUtility.ToJson(loginInfo, true);

        print(jsonData);

        return false;

        // 네트워크 분들한테 url로 전달
    }

    public void SendAccesscode()
    {
        string jsonData = JsonUtility.ToJson(loginInfo.memberId, true);

        print(jsonData);
    }

    public void VerifyAccessCode()
    {
        //string jsonData = JsonUtility.ToJson(code, true);

        //print(jsonData);
    }

    public void OnclickHttp()
    {
        OnClickLoginButton();
        Save();
        //StartCoroutine(UnityWebRequestPOSTTEST());
    }

    public void Save()
    {
        // ArrayJson -> json
        string loginJson = JsonUtility.ToJson(loginInfo, true);
        print(loginJson);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/auth/login";
        requester.requestType = RequestType.POST;
        requester.postData = loginJson;
        requester.onComplete = (handler) => {
            print("유저정보 기입완료");
            Login_1 login_1 = JsonUtility.FromJson<Login_1>(handler.text);
            Login_1_data data = login_1.data;

            data.grantType = login_1.data.grantType;
            data.memberName = login_1.data.memberName;
            data.accessToken = login_1.data.accessToken;
            data.accessTokenExpiresIn = login_1.data.accessTokenExpiresIn;
            UserInfo.accessToken = data.accessToken;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void OnClickLoginButton()
    {
        // 로그인 정보 받아오기
        loginInfo.memberId = login_ID.text;
        loginInfo.memberPwd = login_PW.text;
    }


    // 첫번째 ] 로그인 시 받아올 토큰
    [Serializable]
    public class Login_1
    {
        public string status;
        public string message;
        public Login_1_data data;// = new Login_1_data();
    }

    // 두번째 ] 로그인 시 받아올 토큰
    [Serializable]
    public class Login_1_data
    {
        public string grantType;
        public string memberName;
        public string accessToken;
        public string accessTokenExpiresIn;
    }


}
