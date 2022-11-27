using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// �Խñ� ����
public enum RequestType
{
    POST, //������
    GET, //�ޱ�
    PUT, //����
    DELETE, //����
    IMAGE, //�̹���
    AUDIO,  // �����
}

public class YJ_HttpRequester
{
    // url
    public string url;
    public bool tts;
    // ��û Ÿ�� (GET, POST, PUT, DELETE)
    public RequestType requestType;
    // Post Data
    public string postData; // -> body

    // Header Data
    public Dictionary<string, string> headers;

    // ������ ���� �� ȣ������ �Լ� (Action)
    // Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<UnityWebRequest> onComplete;

    // ��ȯ�ڷ����� void, �Ű������� ���� �Լ��� ���� �� �ִ�.

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


