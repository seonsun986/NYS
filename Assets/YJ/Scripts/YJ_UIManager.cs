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

    enum UIState
    {
        Login,
        SignIn,
        Interest
    }
    UIState uiState = UIState.Login;

    void Start()
    {
        //nextButton = GameObject.Find("button_Next");
        //nextDefaultColor = nextButton.GetComponent<Image>().color;

        //loginUI.SetActive(true);
        //signInUI.SetActive(false);
        //InterestUI.SetActive(false);
    }

    void Update()
    {
        switch(uiState)
        {
            case UIState.Login:
                //LoginState();
                break;
            case UIState.SignIn:
                SignInState();
                break;
            case UIState.Interest:
                InterestState();
                break;
        }
    }

    //private void LoginState()
    //{
    //    if (loginDataFlag)
    //    {
    //        login_ID = GameObject.Find("login_ID").GetComponent<InputField>();
    //        login_PW = GameObject.Find("login_PW").GetComponent<InputField>();
    //        loginDataFlag = false;
    //    }
    //}

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

        // ��Ʈ��ũ �е����� url�� ����
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

    // ��Ʈ��ũ �α������� ���� > ��������ū �޾ƿ���
    IEnumerator UnityWebRequestPOSTTEST()
    {
        string url = "http://43.201.10.63:8080/auth/login";
        string loginJson = JsonUtility.ToJson(loginInfo, true);
        
        print(loginJson);

        using (UnityWebRequest www = UnityWebRequest.Post(url, loginJson))  // ���� �ּҿ� ������ �Է�
        {
            www.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(loginJson);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            yield return www.SendWebRequest();  // ���� ���

            if (www.error == null)
            {
                Debug.Log(www.downloadHandler.text);    // ������ ���
                
            }
            else
            {
                print(www.error);
                Debug.Log("error");
            }
        }
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
            print("�������� ���ԿϷ�");

            //string loginJson = JsonUtility.FromJson(handler.text);
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void Save2()
    {
        //ArrayJson arrayJson = new ArrayJson();
        //arrayJson.data = interestInfo.interest;

        //InterestJson interestJson = new InterestJson();
        //interestJson.ID = userInfo.ID;
        //interestJson.arrayJson = arrayJson;

        //interestInfo.ID = userInfo.ID;

        //string jsonData = JsonUtility.ToJson(interestInfo, true);

        //print(jsonData);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://192.168.1.29:8888/member/interest";
        requester.requestType = RequestType.POST;
        //requester.postData = jsonData;
        requester.onComplete = (handler) => {
            print("ȸ������ �Ϸ�");

            uiState = UIState.Login;

            InterestUI.SetActive(false);
            loginUI.SetActive(true);
        };
        //YJ_HttpRequester.instance.SendRequest(requester);
    }


    public void OnClickLoginButton()
    {
        // �α��� ���� �޾ƿ���
        loginInfo.memberId = login_ID.text;
        loginInfo.memberPwd = login_PW.text;

        //login_ID.text = loginInfo.memberId;
        //login_PW.text = loginInfo.memberPwd;


        // ���̽� ����
        //string jsonData = JsonUtility.ToJson(loginInfo, true);

        //print(jsonData);

        // ���� �˾ƾ���
        //YJ_HttpRequester requester = new YJ_HttpRequester();
        //requester.url = "http://192.168.1.29:8888/member/login";
        //requester.requestType = RequestType.POST;
        //requester.postData = jsonData;
        //requester.onComplete = (handler) => {
        //    print("�α��� �Ϸ�");

        //    SceneManager.LoadScene("PlazaScene");
        //};
        //YJ_HttpRequester.instance.SendRequest(requester);
    }

}
