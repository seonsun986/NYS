using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<string> titles = new List<string>();
    public List<Texture2D> images = new List<Texture2D>();
    public GameObject booksParent;
    public GameObject bookFactory;
    // å ���� ���� ���⿡ �ʿ��� �Ӽ�
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // å ǥ�� ������ �ʿ��� �Ӽ�
    public GameObject bookCoverUI;
    // ������ ��ƼĿ
    public GameObject delSticker;
    // ���õ� å
    public GameObject selectedBook;
    string path;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //��ȭǥ�� GET
        TaleGet_API();

        //��ȭǥ�� POST
        //TalePost_API();

        //titles.Add("��ġ��ҳ�");
        //titles.Add("�ŵ�����");
        //titles.Add("������ ��");

        path = Application.dataPath + "/BookCover/";

    }


    byte[] nullbytedata = new byte[1];
    string s = " ";

    public void TalePost_API(TaleInfo taleInfo)
    {
/*        TaleInfo taleInfo = new TaleInfo();

        taleInfo.id = "6370bc341a1b09093c81512d";
        taleInfo.fontStyle = s;
        taleInfo.fontSize = s;
        taleInfo.fontColor = s;
        taleInfo.fontPositionX = s;
        taleInfo.fontPositionY = s;
        taleInfo.coverColor = s;
        taleInfo.sticker = s;
        taleInfo.stickerPositionX = s;
        taleInfo.stickerPositionY = s;
        taleInfo.inputImg = nullbytedata;*/


        // ArrayJson -> json
        string taleListJson = JsonUtility.ToJson(taleInfo, true);
        print(taleListJson);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/info";
        requester.requestType = RequestType.POST;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.headers["Content-Type"] = "application/json";
        requester.postData = taleListJson;
        requester.onComplete = (handler) => {
            print("��ū �޾ƿ��� �Ϸ�");

            //JObject tokenJson = JObject.Parse(handler.downloadHandler.text);

            // data �ȿ� accessToken���� ����
            Debug.Log(handler.downloadHandler.text);

            // ���̽���ü�� �޾Ƽ� data ��ü�� �ް� �� �ȿ��� ������ ���� ����
            //JObject keyData = tokenJson["data"].ToObject<JObject>();

        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    // ��ȭå ��ϰ�������
    public void TaleGet_API()
    {
        Title title = new Title();

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/mylist";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) => {

            Debug.Log("�� ��ȭ��� �޾ƿԾ�! \n" + handler.downloadHandler.text);

            title = JsonUtility.FromJson<Title>(handler.downloadHandler.text);

            Data[] data = title.data;
            print("������ ����Ծ�? : " + data.Length);

            TaleList taleList = new TaleList();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].taleList.id == null) continue;
                taleList.id = data[i].taleList.id;
                taleList.memberCode = data[i].taleList.memberCode;
                taleList.title = data[i].taleList.title;
                taleList.createAt = data[i].taleList.createAt;
                titles.Add(taleList.title);
                data[i].taleList = taleList;
            }

            TaleInfo taleInfo = new TaleInfo();
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].taleInfo.id == null) continue;
                taleInfo.id = data[j].taleInfo.id;
                taleInfo.fontStyle = data[j].taleInfo.fontStyle;
                taleInfo.fontSize = data[j].taleInfo.fontSize;
                taleInfo.fontColor = data[j].taleInfo.fontColor;
                taleInfo.fontPositionX = data[j].taleInfo.fontPositionX;
                taleInfo.fontPositionY = data[j].taleInfo.fontPositionY;
                taleInfo.coverColor = data[j].taleInfo.coverColor;
                taleInfo.sticker = data[j].taleInfo.sticker;
                taleInfo.stickerPositionX = data[j].taleInfo.stickerPositionX;
                taleInfo.stickerPositionY = data[j].taleInfo.stickerPositionY;
                taleInfo.inputImg = data[j].taleInfo.inputImg;

                data[j].taleInfo = taleInfo;
            }

            for (int i = 0; i < titles.Count; i++)
            {
                // ���� �߰��� ������ŭ ��ȭå ����
                GameObject book = Instantiate(bookFactory, booksParent.transform);
                // JSON���� �ҷ��� �������� �ؽ�Ʈ ����
                book.GetComponentInChildren<Text>().text = titles[i];
                book.GetComponent<Button>().onClick.AddListener(ClickBook);
            }

            //GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    [Serializable]
    public class Title
    {
        public string status;
        public string message;
        public Data[] data;
    }

    [Serializable]
    public class Data
    {
        public TaleList taleList;
        public TaleInfo taleInfo;
    }

    [Serializable]
    public class TaleList
    {
        public string id;
        public string memberCode;
        public string title;
        public string createAt;
        public string thumbnail;
    }

    [Serializable]
    public class TaleInfo
    {
        public string id;
        public string fontStyle;
        public string fontColor;
        public string fontSize;
        public string fontPositionX;
        public string fontPositionY;
        public string coverColor;
        public string sticker;
        public string stickerPositionX;
        public string stickerPositionY;
        public byte[] inputImg;
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
    /// //////////////////////////////////////////////////////////////////////////
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
/// ///////////////////////////////////////////////////////////////////////////////////
    public void SaveBookCover()
    {
        // å ǥ�� ĸ��
        StopAllCoroutines();
        StartCoroutine(TakeScreenShotRoutine());
        // Json���� å ǥ�� ����
        TaleInfo taleInfo = NK_BookCover.instance.taleInfo;

        taleInfo.id = "6370bc341a1b09093c81512d";
        taleInfo.fontPositionX = NK_BookCover.instance.inputField.GetComponent<RectTransform>().anchoredPosition.x.ToString();
        taleInfo.fontPositionY = NK_BookCover.instance.inputField.GetComponent<RectTransform>().anchoredPosition.y.ToString();
        taleInfo.inputImg = nullbytedata;

        TalePost_API(NK_BookCover.instance.taleInfo);
    }

    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();

        // ȭ�� ũ���� �ؽ��� ����
        Texture2D screenTex = new Texture2D(310, 410, TextureFormat.RGB24, false);
        // ĸ���� ���� ����
        Rect area = new Rect(485f, 400f, 310f, 410f);
        // �ؽ��� �ȼ��� ����
        screenTex.ReadPixels(area, 0, 0);
        
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

    public void ExitBookShelf()
    {
        SceneManager.LoadScene("MyRoomScene");
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
