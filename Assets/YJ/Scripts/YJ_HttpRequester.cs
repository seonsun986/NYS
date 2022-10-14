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
    IMAGE //�̹���
}

public class YJ_HttpRequester
{
    // url
    public string url;
    // ��û Ÿ�� (GET, POST, PUT, DELETE)
    public RequestType requestType;
    // Post Data
    public string postData; // -> body

    // ������ ���� �� ȣ������ �Լ� (Action)
    // Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<DownloadHandler> onComplete;

    // ��ȯ�ڷ����� void, �Ű������� ���� �Լ��� ���� �� �ִ�.

}