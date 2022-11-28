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
        // 만약에 instance가 null이라면
        if (instance == null)
        {
            // instance에 나를 넣겠다.
            instance = this;
            // 내가 파괴되지 않도록 하겠다.
            DontDestroyOnLoad(gameObject);
        }
        // 그렇지 않으면
        else
        {
            // 나를 파괴하겠다.
            Destroy(gameObject);
        }
    }

    // 서버에게 요청
    // url(posts/1), GET
    public void SendRequest(YJ_HttpRequester requester)
    {
        StartCoroutine(Send(requester));
    }

    IEnumerator Send(YJ_HttpRequester requester)
    {
        // requesterType에 따라서 호출해줘야한다.
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
                // 파일 경로를 URL 형태로 바꿔줌(File:// 경로 형태로 만들어줌)
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
        // 서버에 요청을 보내고 응답이 올때까지 기다린다.
        yield return webRequest.SendWebRequest();

        // 만약에 응답이 성공했다면

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            if (requester.requestType != RequestType.AUDIO && requester.requestType != RequestType.DELETE)
            {
                print(webRequest.downloadHandler.text);
            }
            else
            {
                // 오디오 타입일 때
            }
            // 완료되었다고 requester.onComplete를 실행
            requester.OnComplete(webRequest);
        }
        else
        { 
            // 서버통신 실패...
            print("통신 실패" + webRequest.result + "\n" + webRequest.error + "\n" + webRequest.responseCode);

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
