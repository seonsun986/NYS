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
    IMAGE //이미지
}

public class YJ_HttpRequester
{
    // url
    public string url;
    // 요청 타입 (GET, POST, PUT, DELETE)
    public RequestType requestType;
    // Post Data
    public string postData; // -> body

    // 응답이 왔을 때 호출해줄 함수 (Action)
    // Action : 함수를 넣을 수 있는 자료형
    public Action<DownloadHandler> onComplete;

    // 반환자료형이 void, 매개변수가 없는 함수를 넣을 수 있다.

}