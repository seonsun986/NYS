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
    public GameObject popupBG;

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
                if (requester.headers == null)
                {
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                }
                else
                {
                    SetCustomHeader(webRequest, requester.headers);
                }
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
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                if (requester.headers != null)
                {
                    SetCustomHeader(webRequest, requester.headers);
                }
                break;
            case RequestType.IMAGE:
                webRequest = UnityWebRequestTexture.GetTexture(requester.url);
                break;
            case RequestType.AUDIO:
                // ���� ��θ� URL ���·� �ٲ���(File:// ��� ���·� �������)
                Uri uri = new Uri(requester.url);
                webRequest = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV);

                //if (requester.record == false)
                //{
                //    webRequest = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG);
                //}
                //else
                //{
                //}
                break;
        }
        // ������ ��û�� ������ ������ �ö����� ��ٸ���.
        yield return webRequest.SendWebRequest();

        // ���࿡ ������ �����ߴٸ�

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            if (requester.requestType != RequestType.AUDIO && requester.requestType != RequestType.DELETE)
            {
                print(webRequest.downloadHandler.text);
            }
            else
            {
                // ����� Ÿ���� ��
            }
            // �Ϸ�Ǿ��ٰ� requester.onComplete�� ����
            requester.OnComplete(webRequest);
        }
        else
        { 
            // ������� ����...
            print("��� ����" + webRequest.result + "\n" + webRequest.error + "\n" + webRequest.responseCode);

            if (SceneManager.GetActiveScene().name == "ConnectionScene")
            {
                popupBG.SetActive(true);
                loginFail.SetActive(true);
            }
        }
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
