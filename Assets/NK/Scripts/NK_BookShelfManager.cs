using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NK_BookUI;
using static System.Net.WebRequestMethods;

// å UI ����, Json�ް� ������
public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<Texture2D> images = new List<Texture2D>();
    public List<string> textContent = new List<string>();
    public GameObject booksParent;
    public GameObject bookFactory;
    // å ���� ���� ���⿡ �ʿ��� �Ӽ�
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // å ǥ�� ������ �ʿ��� �Ӽ�
    public GameObject bookCoverUI;
    // ���õ� å
    public GameObject selectedBook;
    public GameObject nextBtn;
    public GameObject prevBtn;
    string path;

    // ��ȸ�� TaleInfo ��� ����
    List<TaleInfo> taleInfos = new List<TaleInfo>();
    // �����Ǵ� å ������Ʈ
    List<GameObject> bookObjs = new List<GameObject>();
    // ���� ����
    List<string> titles = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //��ȭǥ�� GET
        TaleGet_API();

        // �ε� UI �ʱ�ȭ
        loadingBookContent.SetActive(false);
        loadingBookContent.transform.GetChild(2).GetComponent<Image>().fillAmount = 0f;

        path = Application.dataPath + "/BookCover/";
    }


    byte[] nullbytedata = new byte[1];
    string s = " ";

    public void TalePost_API(TaleInfo taleInfo)
    {
        // ArrayJson -> json
        string taleListJson = JsonUtility.ToJson(taleInfo, true);
        print(taleListJson);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/info";
        requester.requestType = RequestType.PUT;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.headers["Content-Type"] = "application/json";
        requester.postData = taleListJson;
        requester.onComplete = (handler) =>
        {
            print("��ū �޾ƿ��� �Ϸ�");

            //JObject tokenJson = JObject.Parse(handler.downloadHandler.text);

            // data �ȿ� accessToken���� ����
            Debug.Log(handler.downloadHandler.text);

            // ���̽���ü�� �޾Ƽ� data ��ü�� �ް� �� �ȿ��� ������ ���� ����
            //JObject keyData = tokenJson["data"].ToObject<JObject>();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    // ��ȸ�� ��ȭå ������
    Data[] data;
    // ��ȭå ��ϰ�������
    public void TaleGet_API()
    {
        Title title = new Title();
        taleInfos = new List<TaleInfo>();
        bookObjs = new List<GameObject>();
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/mylist";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) =>
        {

            Debug.Log("�� ��ȭ��� �޾ƿԾ�! \n" + handler.downloadHandler.text);

            title = JsonUtility.FromJson<Title>(handler.downloadHandler.text);

            data = title.data;
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

            for (int j = 0; j < data.Length; j++)
            {
                TaleInfo taleInfo = new TaleInfo();
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
                taleInfo.thumbNail = data[j].taleInfo.thumbNail;
                taleInfos.Add(taleInfo);
                data[j].taleInfo = taleInfo;
            }

            for (int i = 0; i < data.Length; i++)
            {
                // ���� �߰��� ������ŭ ��ȭå ����
                GameObject book = Instantiate(bookFactory, booksParent.transform);
                // JSON���� �ҷ��� �������� �ؽ�Ʈ ����
                book.GetComponentInChildren<Text>().text = titles[i];
                book.GetComponent<Button>().onClick.AddListener(ClickBook);
                bookObjs.Add(book);
                // �̹� ����� ǥ�� �̹��� �ҷ�����
                ReadImage(bookObjs.IndexOf(book));
            }
            //GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void ReadImage(int index)
    {
        // å ǥ�� �̹��� �޾ƿ���
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = taleInfos[index].thumbNail;
        requester.requestType = RequestType.IMAGE;
        requester.onComplete = (handler) =>
        {
            // å ǥ�� �̹��� �ؽ��ķ� �޾ƿ���
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            // å ǥ�� �̹��� ��������Ʈ�� ���� ������ å�� ���� �� ���� �ؽ�Ʈ ��Ȱ��ȭ
            Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            booksParent.transform.GetChild(index).GetComponent<Image>().sprite = tempSprite;
            booksParent.transform.GetChild(index).GetComponentInChildren<Text>().enabled = false;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void GetDetailImage()
    {
        // å �̸����� RawImage ��������
        print("��ȭå ���� ��.");
        Info title = new Info();
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/" + taleInfos[bookObjs.IndexOf(selectedBook)].id;
        requester.requestType = RequestType.GET;
        requester.onComplete = (handler) =>
        {
            JObject taleJObj = JObject.Parse(handler.downloadHandler.text);
            Debug.Log("�� ��ȭ �¾�? \n" + handler.downloadHandler.text);
            title = JsonUtility.FromJson<Info>(handler.downloadHandler.text);

            for (int i = 0; i < title.data.pages.Count; i++)
            {
                images.Add(null);
                ReadDetailImage(taleJObj["data"]["pages"][i]["rawImgUrl"].ToString(), i);
            }
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void ReadDetailImage(string url, int index)
    {
        // å ���� �̹��� �޾ƿ���
        NK_HttpMediaRequester requester = new NK_HttpMediaRequester();
        requester.url = url;
        requester.requestType = RequestType.IMAGE;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            // å ���� �̹��� �ؽ��ķ� �޾ƿ���
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            images[idx] = texture;
            if (idx == 0)
                rawImage.texture = images[idx];
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void DeleteBook()
    {
        // ��ȭå ����
        print("��ȭå �����ϱ�");
        Info title = new Info();
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/" + taleInfos[bookObjs.IndexOf(selectedBook)].id;
        requester.requestType = RequestType.DELETE;
        requester.onComplete = (handler) =>
        {
            print("��ȭ ������! \n" + handler.downloadHandler.text);
            popupBG.SetActive(false);
            deletePopup.SetActive(false);
            ExitDetail();
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
        public string thumbNail;
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
        public string thumbNail;
    }


    // Update is called once per frame
    void Update()
    {

    }

    int index = 0;
    public void ClickBook()
    {
        // å �����ϸ� å �̸����� ������
        selectedBook = EventSystem.current.currentSelectedGameObject;
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);
        nextBtn.SetActive(false);
        prevBtn.SetActive(false);

        // rawImage �ҷ����� �ʱ�ȭ
        images.Clear();
        index = 0;
        rawImage.rectTransform.sizeDelta = new Vector2(800, 500);
        GetDetailImage();
    }
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

    public void ClickBeforeBook()
    {
        Vector2 pos = booksParent.GetComponent<RectTransform>().anchoredPosition;
        booksParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(400, 0);
        //booksParent.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(pos, pos + new Vector2(400, 0), 0.05f);
    }

    public void ClickNextBook()
    {
        Vector2 pos = booksParent.GetComponent<RectTransform>().anchoredPosition;
        booksParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(-400, 0);
        //booksParent.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(pos, pos - new Vector2(400, 0), 0.05f);
    }

    public GameObject contentPos;
    public void NextBtn(float change)
    {
        //print(contentPos.GetComponent<RectTransform>().sizeDelta.x);
        //print(contentPos.GetComponent<RectTransform>().anchoredPosition.x + change);
        //iTween.MoveTo(contentPos, iTween.Hash("x", contentPos.GetComponent<RectTransform>().anchoredPosition.x + change, "time", 0.5f));

        float x = contentPos.GetComponent<RectTransform>().anchoredPosition.x;
/*        if (x + change < -900)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
          "from", x, "to", -840, "time", 0.3f,
          "onupdatetarget", gameObject, "onupdate", "�׳ɷ�����"));
        }
        else
        {*/
            iTween.ValueTo(gameObject, iTween.Hash(
            "from", x, "to", x + change, "time", 0.3f,
            "onupdatetarget", gameObject, "onupdate", "�׳ɷ�����"));
        //}

    }

    void �׳ɷ�����(float v)
    {
        RectTransform rt = contentPos.GetComponent<RectTransform>();
        Vector2 v2 = rt.anchoredPosition;
        v2.x = v;
        rt.anchoredPosition = v2;

    }

    public void UpdateBookCover()
    {
        // å ǥ�� ����
        bookCoverUI.SetActive(true);
        //NK_BookCover.instance.Initialization();
        NK_BookCover.instance.inputField.text = detailTitle.text;
        TaleInfo selectedBookInfo = taleInfos[bookObjs.IndexOf(selectedBook)];
        // ǥ�� �̹����� ���� ���� �������� �۾� �� ��ƼĿ �ҷ���
        if (selectedBook.GetComponent<Image>().sprite.name != "FairyTaleBook_White")
        {
            NK_BookCover.instance.SetBookCoverFont(selectedBookInfo.fontStyle, selectedBookInfo.fontColor, selectedBookInfo.fontSize, selectedBookInfo.fontPositionX, selectedBookInfo.fontPositionY);
            NK_BookCover.instance.SetBookCover(selectedBookInfo.coverColor, selectedBookInfo.sticker, selectedBookInfo.stickerPositionX, selectedBookInfo.stickerPositionY);
        }
    }

    // �ε� �� UI
    public GameObject loadingBookContent;

    public void UpdateBookContent()
    {
        loadingBookContent.SetActive(true);
        YJ_DataManager.instance.preScene = "BookShelfScene";
        YJ_DataManager.instance.updateBookId = taleInfos[bookObjs.IndexOf(selectedBook)].id;
        // å ���� ����
        StopAllCoroutines();
        StartCoroutine("Loading");
    }

    IEnumerator Loading()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("EditorScene");

        while (!asyncOperation.isDone)
        {
            yield return null;
            loadingBookContent.transform.GetChild(2).GetComponent<Image>().fillAmount += asyncOperation.progress;
        }
    }

    public GameObject savePopup;
    public GameObject deletePopup;
    public GameObject popupBG;
    public void ClickSave()
    {
        popupBG.SetActive(true);
        savePopup.SetActive(true);
    }

    public void ClickDeleteBook()
    {
        popupBG.SetActive(true);
        deletePopup.SetActive(true);
    }

    // taleInfo > ��ȭå ǥ�������� ������
    public void SaveBookCover()
    {
        // Json���� å ǥ�� ����
        TaleInfo taleInfo = NK_BookCover.instance.taleInfo;
        // ���õ� å�� TaleInfo�� id�� �޾ƿ�
        taleInfo.id = taleInfos[bookObjs.IndexOf(selectedBook)].id;
        //print(taleInfo.id);
        print(data.Length);
        // ���õ� å�� Json�� ���� TaleInfo�� ���� ����
        NK_BookCover.instance.SaveTaleInfo();

        // å ǥ�� ĸ�� �� ������ ������ �۾�
        StopAllCoroutines();
        StartCoroutine(TakeScreenShotRoutine());
    }

    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();

        // ȭ�� ũ���� �ؽ��� ����
        Texture2D screenTex = new Texture2D(300, 400, TextureFormat.RGB24, false);
        // ĸ���� ���� ����
        Rect area = new Rect(462f, 266f, 300f, 400f);
        // �ؽ��� �ȼ��� ����
        screenTex.ReadPixels(area, 0, 0);
        screenTex.Apply();

        // ������ �������� ������ ���� ����
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        string fileName = path + "book" + ".png";
        NK_BookCover.instance.taleInfo.inputImg = screenTex.EncodeToPNG();

        // ��ũ���� ����
        System.IO.File.WriteAllBytes(fileName, screenTex.EncodeToPNG());
        // �ؽ��� �޸� ����
        Destroy(screenTex);

        // ���õ� å�� ǥ�� ���� ������
        TalePost_API(NK_BookCover.instance.taleInfo);
        // ǥ�� ���� �˾� �ݱ�
        ExitPopup();
    }

    public void ClickYes()
    {
        popupBG.SetActive(false);
        savePopup.SetActive(false);
        SaveBookCover();
    }

    public void ClickNo()
    {
        popupBG.SetActive(false);
        savePopup.SetActive(false);
        deletePopup.SetActive(false);
    }


    public void ExitBookShelf()
    {
        SceneManager.LoadScene("MyRoomScene");
    }

    public void ExitDetail()
    {
        detailUI.SetActive(false);
        booksParent.SetActive(true);
        nextBtn.SetActive(true);
        prevBtn.SetActive(true);

        NK_BookTrigger[] childList = booksParent.GetComponentsInChildren<NK_BookTrigger>();
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // å ������Ʈ
        TaleGet_API();
    }

    public void ExitPopup()
    {
        bookCoverUI.SetActive(false);
    }
}
