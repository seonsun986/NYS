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

    public GameObject loginFail;

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

    public void OnclickHttp()
    {
        OnClickLoginButton();
        Login_1_API();

    }

    public void Login_1_API()
    {
        // ArrayJson -> json
        string loginJson = JsonUtility.ToJson(loginInfo, true);
        print(loginJson);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/auth/login";
        requester.requestType = RequestType.POST;
        requester.postData = loginJson;
        requester.onComplete = (handler) => {
            print("토큰 받아오기 완료");
            Login_1 login_1 = JsonUtility.FromJson<Login_1>(handler.text);
            Login_1_data data = login_1.data;
            UserInfo.accessToken = data.accessToken;
            Login_2_API();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }
    public void Login_2_API()
    {
        // ArrayJson -> json
        string tokenJson = JsonUtility.ToJson(UserInfo.accessToken, true);
        print(UserInfo.accessToken);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/members/optional-info";
        requester.requestType = RequestType.GET;
        requester.onComplete = (handler) => {
            print("정보 가져옴!");
            Login_2 login_2 = JsonUtility.FromJson<Login_2>(handler.text);
            Login_2_data data = login_2.data;
            UserInfo.memberName = data.memberName;
            UserInfo.nickname = data.nickname;
            UserInfo.memberRole = data.memberRole;
            GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
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
        public Login_1_data data;
    }

    [Serializable]
    public class Login_1_data
    {
        public string grantType;
        public string memberName;
        public string accessToken;
        public string accessTokenExpiresIn;
    }

    // 두번째 ] 로그인 시 받아올 토큰
    [Serializable]
    public class Login_2
    {
        public string status;
        public string message;
        public Login_2_data data;
    }
    [Serializable]
    public class Login_2_data
    {
        public string memberCode;
        public string memberId;
        public string memberPwd;
        public string memberName;
        public string email;
        public string phone;
        public string nickname;
        public string memberRole;
        public string authorities;
        public string password;
        public string enabled;
        public string username;
        public string accountNonExpired;
        public string credentialsNonExpired;
        public string accountNonLocked;
    }

    public void ButtonClose()
    {
        login_ID.text = "";
        login_PW.text = "";
        loginFail.SetActive(!loginFail.activeSelf);
    }
}
