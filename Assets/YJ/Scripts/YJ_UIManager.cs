using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

//[System.Serializable]
//public class InterestJson
//{
//    public string ID;
//    public ArrayJson arrayJson;
//}

//[System.Serializable]
//public class ArrayJson
//{
//    public bool[] data;
//}


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

    // 로그인 버튼 눌렀을때
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
            avetarSet avatar = login_2.data.avatar;
            memberSet member = login_2.data.member;

            // 아바타 정보 세팅
            UserInfo.animal = data.avatar.animal;
            UserInfo.material = data.avatar.material;
            UserInfo.objectName = data.avatar.objectName;

            // 유저 정보 세팅
            UserInfo.memberName = data.member.memberName;
            UserInfo.nickname = data.member.nickname;
            UserInfo.memberRole = data.member.memberRole;
            GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }
    //public void Login_3_API()
    //{
    //    // ArrayJson -> json
    //    string tokenJson = JsonUtility.ToJson(UserInfo.accessToken, true);
    //    print(UserInfo.accessToken);

    //    YJ_HttpRequester requester = new YJ_HttpRequester();
    //    requester.url = "http://43.201.10.63:8080/avatar";
    //    requester.requestType = RequestType.GET;
    //    requester.onComplete = (handler) => {
    //        print("캐릭터 정보 세팅");
    //        Login_3 login_3 = JsonUtility.FromJson<Login_3>(handler.text);
    //        Login_3_data data = login_3.data;
    //        UserInfo.memberCode = data.memberCode;
    //        UserInfo.animal = data.animal;
    //        UserInfo.material = data.material;
    //        UserInfo.objectName = data.objectName;
            
    //    };
    //    YJ_HttpManager.instance.SendRequest(requester);
    //}
    // 로그인 ID, PW 저장
    public void OnClickLoginButton()
    {
        loginInfo.memberId = login_ID.text;
        loginInfo.memberPwd = login_PW.text;
    }

    #region 로그인 시 받아올 정보 목록
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
        public avetarSet avatar;
        public memberSet member;
    }

    // 세번째] 캐릭터 정보
    [Serializable]
    public class avetarSet
    {
        public string memberCode;
        public string animal;
        public string material;
        public string objectName;
    }
    // 세번째] 내 캐릭터 정보
    [Serializable]
    public class memberSet
    {
        public string memberCode;
        public string memberName;
        public string nickname;
        public string memberRole;
    }
    #endregion

    // 잘못 입력했을때 창 끄기
    public void ButtonClose()
    {
        login_ID.text = "";
        login_PW.text = "";
        loginFail.SetActive(!loginFail.activeSelf);
    }


}
