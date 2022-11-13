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
    string path;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TaleSet_API();

        //titles.Add("��ġ��ҳ�");
        //titles.Add("�ŵ�����");
        //titles.Add("������ ��");

        path = Application.dataPath + "/BookCover/";

    }

    // ��ȭå ��ϰ�������
    public void TaleSet_API()
    {

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/mylist";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) => {

            Debug.Log("�� ��ȭ��� �޾ƿԾ�! \n" + handler.downloadHandler.text);

            Title title = JsonUtility.FromJson<Title>(handler.downloadHandler.text);

            Data[] data = title.data;


            TaleList taleList = new TaleList();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].taleList.id == null) continue;
                taleList.id = data[i].taleList.id;
                taleList.memberCode = data[i].taleList.memberCode;
                taleList.title = data[i].taleList.title;
                taleList.createAt = data[i].taleList.createAt;

                data[i].taleList = taleList;
            }

            TaleInfo taleInfo = new TaleInfo();
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].taleInfo.id == null) continue;
                taleInfo.id = data[j].taleInfo.id;
                taleInfo.fontStyle = data[j].taleInfo.fontStyle;
                taleInfo.fontPosition_x = data[j].taleInfo.fontPosition_x;
                taleInfo.fontPosition_y = data[j].taleInfo.fontPosition_y;
                taleInfo.fontSize = data[j].taleInfo.fontSize;
                taleInfo.fontColor = data[j].taleInfo.fontColor;
                taleInfo.coverColor = data[j].taleInfo.coverColor;
                taleInfo.sticker = data[j].taleInfo.sticker;
                taleInfo.stickerPosition_x = data[j].taleInfo.stickerPosition_x;
                taleInfo.stickerPosition_y = data[j].taleInfo.stickerPosition_y;
                taleInfo.thumbNail = data[j].taleInfo.thumbNail;

                data[j].taleInfo = taleInfo;
            }

            //��ȭå ���� �߰��ϸ�
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].taleList.title == null) continue;
                titles.Add(data[j].taleList.title);
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

    //[SerializeField]
    [Serializable]
    public class Title
    {
        public string status;
        public string message;
        public Data[] data;
    }

    [SerializeField]
    public class Data
    {
        public TaleList taleList;
        public TaleInfo taleInfo;
    }

    [SerializeField]
    public class TaleList
    {
        public string id;
        public string memberCode;
        public string title;
        public string createAt;
    }

    [SerializeField]
    public class TaleInfo
    {
        public string id;
        public string fontStyle;
        public string fontPosition_x;
        public string fontPosition_y;
        public string fontSize;
        public string fontColor;
        public string coverColor;
        public string sticker;
        public string stickerPosition_x;
        public string stickerPosition_y;
        public string thumbNail;
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
