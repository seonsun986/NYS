using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NK_Emotion;

public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<string> titles = new List<string>();
    public List<Texture2D> images = new List<Texture2D>();
    public GameObject booksParent;
    public GameObject bookFactory;
    public float spacing = 4;
    // å ���� ���� ���⿡ �ʿ��� �Ӽ�
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // å ǥ�� ������ �ʿ��� �Ӽ�
    public GameObject bookCoverUI;
    // ������ ��ƼĿ
    public GameObject delSticker;
    string path;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/BookCover/";
        // ��ȭå ���� �߰��ϸ�
        titles.Add("�� �� �÷�!!!");
        titles.Add("�� ���� �÷�!!!");
        titles.Add("�� ���� �÷�!!!");
        for (int i = 0; i < titles.Count; i++)
        {
            // ���� �߰��� ������ŭ ��ȭå ����
            GameObject book = Instantiate(bookFactory, booksParent.transform);
            // JSON���� �ҷ��� �������� �ؽ�Ʈ ����
            book.GetComponentInChildren<Text>().text = titles[i];
            book.GetComponent<Button>().onClick.AddListener(ClickBook);
        }
    }

    // ��ȭå ��ϰ�������
    public void Login_2_API()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/mylist";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) => {

            JObject jsonData = JObject.Parse(handler.downloadHandler.text);

            UserInfo myInfo = YJ_DataManager.instance.myInfo;
            myInfo.animal = jsonData["data"]["avatar"]["animal"].ToString();
            myInfo.material = jsonData["data"]["avatar"]["material"].ToString();
            myInfo.objectName = jsonData["data"]["avatar"]["objectName"].ToString();
            myInfo.nickname = jsonData["data"]["member"]["nickname"].ToString();
            myInfo.memberRole = jsonData["data"]["member"]["memberRole"].ToString();
            myInfo.memberCode = jsonData["data"]["member"]["memberCode"].ToString();

            GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ClickBook()
    {
        // å �����ϸ� å �̸����� ������
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);

        // rawImage �ҷ����� �ʱ�ȭ
        index = 0;
        rawImage.texture = images[index];
    }

    int index = 0;
    public void ClickBefore()
    {
        // å �̸����⿡�� ���� ��ư Ŭ�� ��
        if (0 < index)
        {
            index--;
            rawImage.texture = images[index];
        }
    }

    public void ClickNext()
    {
        // å �̸����⿡�� ���� ��ư Ŭ�� ��
        if (index < images.Count - 1)
        {
            index++;
            rawImage.texture = images[index];
        }
    }

    public void UpdateBookCover()
    {
        // å ǥ�� ����
        bookCoverUI.SetActive(true);
        NK_BookCover.instance.inputField.text = detailTitle.text;
    }

    public void UpdateBookContent()
    {
        // å ���� ����
        SceneManager.LoadScene("EditorScene");
    }

    public void DeleteObj()
    {
        // ���� ��ư ������ �������� Ŭ���� ��ƼĿ ����
        if (delSticker != null)
            Destroy(delSticker);
    }

    public void SaveBookCover()
    {
        // å ǥ�� ĸ��
        StopAllCoroutines();
        StartCoroutine(TakeScreenShotRoutine());
        // Json���� å ǥ�� ����
        // NK_BookCover.instance.bookCover.gameObject
        //ExitPopup();
    }

    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();

        // ȭ�� ũ���� �ؽ��� ����
        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        // ĸ���� ���� ����
        Rect area = new Rect(450f, 370f, 350f, 430f);
        // �ؽ��� �ȼ��� ����
        screenTex.ReadPixels(area, 0, 0);
        Texture2D resizeTexture = new Texture2D((int)area.width, (int)area.height, TextureFormat.RGB24, false);
        
        // ������ �������� ������ ���� ����
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        string fileName = path + "book" + ".png";

        // ��ũ���� ����
        File.WriteAllBytes(fileName, screenTex.EncodeToPNG());
        // �ؽ��� �޸� ����
        Destroy(screenTex);
    }

    public void ExitDetail()
    {
        detailUI.SetActive(false);
        booksParent.SetActive(true);
    }

    public void ExitPopup()
    {
        bookCoverUI.SetActive(false);
    }
}
