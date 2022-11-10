using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class YJ_HttpManager : MonoBehaviour
{
    public static YJ_HttpManager instance;
    public GameObject loginFail;

    private void Awake()
    {
        // ���࿡ instance�� null�̶��
        if (instance == null)
        {
            // instance�� ���� �ְڴ�.
            instance = this;
            // ���� �ı����� �ʵ��� �ϰڴ�.
            DontDestroyOnLoad(gameObject);
        }
        // �׷��� ������
        else
        {
            // ���� �ı��ϰڴ�.
            Destroy(gameObject);
        }
    }

    // �������� ��û
    // url(posts/1), GET
    public void SendRequest(YJ_HttpRequester requester)
    {
        StartCoroutine(Send(requester));
    }

    IEnumerator Send(YJ_HttpRequester requester)
    {
        // requesterType�� ���� ȣ��������Ѵ�.
        UnityWebRequest webRequest = null;
        switch (requester.requestType)
        {
            case RequestType.POST:
                webRequest = UnityWebRequest.Post(requester.url, requester.postData);
                byte[] data = Encoding.UTF8.GetBytes(requester.postData);
                webRequest.uploadHandler = new UploadHandlerRaw(data);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType.GET:
                webRequest = UnityWebRequest.Get(requester.url);
                if(requester.headers != null)
                {
                    SetCustomHeader(webRequest, requester.headers);
                }               

                break;
            case RequestType.PUT:
                webRequest = UnityWebRequest.Put(requester.url, requester.postData);                
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("accesstoken", YJ_DataManager.instance.myInfo.accessToken);
                break;
            case RequestType.DELETE:
                webRequest = UnityWebRequest.Delete(requester.url);
                break;
            case RequestType.IMAGE:
                webRequest = UnityWebRequestTexture.GetTexture(requester.url);
                break;
            case RequestType.AUDIO:
                // ���� ��θ� URL ���·� �ٲ���(File:// ��� ���·� �������)
                Uri uri = new Uri(requester.url);
                webRequest = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
                break;
<<<<<<< HEAD

=======
>>>>>>> YJ_test
        }
        // ������ ��û�� ������ ������ �ö����� ��ٸ���.
        yield return webRequest.SendWebRequest();

        // ���࿡ ������ �����ߴٸ�

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
<<<<<<< HEAD
            if(requester.requestType != RequestType.AUDIO)
=======
            if (requester.requestType != RequestType.AUDIO)
>>>>>>> YJ_test
            {
                print(webRequest.downloadHandler.text);
            }
            else
            {
                // ����� Ÿ���� ��
            }
            // �Ϸ�Ǿ��ٰ� requester.onComplete�� ����
            requester.onComplete(webRequest);
<<<<<<< HEAD


=======
>>>>>>> YJ_test
        }
        else
        { 
            // ������� ����...
            print("��� ����" + webRequest.result + "\n" + webRequest.error + "\n" + webRequest.responseCode);

            if (SceneManager.GetActiveScene().name == "ConnectionScene")
            {
                loginFail.SetActive(true);
            }
        }
        yield return null;
        webRequest.Dispose();
    }

    void SetCustomHeader(UnityWebRequest r, Dictionary<string, string> headers)
    {
        foreach(KeyValuePair<string, string> kvp in headers)
        {
            r.SetRequestHeader(kvp.Key, kvp.Value);
        }
    }
}
