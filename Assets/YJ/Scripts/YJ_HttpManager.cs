using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class YJ_HttpManager : MonoBehaviour
{
    public static YJ_HttpManager instance;


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
                break;
            case RequestType.PUT:
                webRequest = UnityWebRequest.Put(requester.url, requester.postData);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType.DELETE:
                webRequest = UnityWebRequest.Delete(requester.url);
                break;
            case RequestType.IMAGE:
                webRequest = UnityWebRequestTexture.GetTexture(requester.url);
                break;

        }
        // ������ ��û�� ������ ������ �ö����� ��ٸ���.
        yield return webRequest.SendWebRequest();

        // ���࿡ ������ �����ߴٸ�
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            print(webRequest.downloadHandler.text);

            // �Ϸ�Ǿ��ٰ� requester.onComplete�� ����
            requester.onComplete(webRequest.downloadHandler);


        }
        // �׷��� �ʴٸ�
        else
        {

            // ������� ����...
            print("��� ����" + webRequest.result + "\n" + webRequest.error + "\n" + webRequest.responseCode);
        }
        yield return null;
        webRequest.Dispose();
    }
}
