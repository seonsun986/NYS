using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 게시글 정보
public enum RequestType
{
    POST, //보내기
    GET, //받기
    PUT, //수정
    DELETE, //삭제
    IMAGE, //이미지
    AUDIO,  // 오디오
}

public class YJ_HttpRequester
{
    // url
    public string url;
    public bool tts;
    // 요청 타입 (GET, POST, PUT, DELETE)
    public RequestType requestType;
    // Post Data
    public string postData; // -> body

    // Header Data
    public Dictionary<string, string> headers;

    // 응답이 왔을 때 호출해줄 함수 (Action)
    // Action : 함수를 넣을 수 있는 자료형
    public Action<UnityWebRequest> onComplete;

    // 반환자료형이 void, 매개변수가 없는 함수를 넣을 수 있다.

    public virtual void OnComplete(UnityWebRequest webRequest)
    {
        onComplete(webRequest);
    }
}


public class NK_HttpMediaRequester : YJ_HttpRequester
{
    public int index;

    public Action<UnityWebRequest,int> onCompleteDownloadImage;

    public override void OnComplete(UnityWebRequest webRequest)
    {
        onCompleteDownloadImage(webRequest, index);
    }
}


