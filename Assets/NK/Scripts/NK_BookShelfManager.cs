using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<Texture2D> images = new List<Texture2D>();
    public GameObject booksParent;
    public GameObject bookFactory;
    // 책 세부 내용 보기에 필요한 속성
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // 책 표지 수정에 필요한 속성
    public GameObject bookCoverUI;
    // 삭제될 스티커
    public GameObject delSticker;
    // 선택된 책
    public GameObject selectedBook;
    string path;

    // 조회된 TaleInfo 모두 저장
    List<TaleInfo> taleInfos = new List<TaleInfo>();
    // 생성되는 책 오브젝트
    List<GameObject> bookObjs = new List<GameObject>();
    // 제목 저장
    List<string> titles = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //동화표지 GET
        TaleGet_API();

        //동화표지 POST
        //TalePost_API();

        //titles.Add("양치기소년");
        //titles.Add("신데렐라");
        //titles.Add("강아지 똥");

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
            print("토큰 받아오기 완료");

            //JObject tokenJson = JObject.Parse(handler.downloadHandler.text);

            // data 안에 accessToken으로 접근
            Debug.Log(handler.downloadHandler.text);

            // 제이슨자체로 받아서 data 전체를 받고 그 안에서 접근할 수도 있음
            //JObject keyData = tokenJson["data"].ToObject<JObject>();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    // 조회된 동화책 데이터
    Data[] data;
    // 동화책 목록가져오기
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

            Debug.Log("자 동화목록 받아왔어! \n" + handler.downloadHandler.text);

            title = JsonUtility.FromJson<Title>(handler.downloadHandler.text);

            data = title.data;
            print("데이터 몇개들어왔어? : " + data.Length);

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

            for (int i = 0; i < titles.Count; i++)
            {
                // 제목 추가된 개수만큼 동화책 생성
                GameObject book = Instantiate(bookFactory, booksParent.transform);
                // JSON에서 불러온 제목으로 텍스트 지정
                book.GetComponentInChildren<Text>().text = titles[i];
                book.GetComponent<Button>().onClick.AddListener(ClickBook);
                bookObjs.Add(book);
                // 이미 저장된 표지 이미지 불러오기
                ReadImage(bookObjs.IndexOf(book));
            }
            //GameObject.Find("ConnectionManager").GetComponent<YJ_ConnectionManager>().OnSubmit();
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void ReadImage(int index)
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = taleInfos[index].thumbNail;
        print(requester.url);
        requester.requestType = RequestType.IMAGE;
        requester.onComplete = (handler) =>
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            booksParent.transform.GetChild(index).GetComponent<Image>().sprite = tempSprite;
            booksParent.transform.GetChild(index).GetComponentInChildren<Text>().enabled = false;
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

    public void ClickBook()
    {
        // 책 선택하면 책 미리보기 보여짐
        selectedBook = EventSystem.current.currentSelectedGameObject;
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);

        // rawImage 불러오고 초기화
        index = 0;
        rawImage.texture = images[index];
        //rawImage.rectTransform.sizeDelta = new Vector2(300, 400);
        //rawImage.texture = selectedBook.GetComponent<Image>().sprite.texture;
    }

    int index = 1;
    public void ClickBefore()
    {
        rawImage.rectTransform.sizeDelta = new Vector2(800, 500);
        // 책 미리보기에서 이전 버튼 클릭 시
        if (0 < index)
        {
            index--;
            rawImage.texture = images[index];
        }
    }

    public void ClickNext()
    {
        rawImage.rectTransform.sizeDelta = new Vector2(800, 500);
        // 책 미리보기에서 다음 버튼 클릭 시
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

    public void UpdateBookCover()
    {
        // 책 표지 수정
        bookCoverUI.SetActive(true);
        //NK_BookCover.instance.Initialization();
        NK_BookCover.instance.inputField.text = detailTitle.text;
        TaleInfo selectedBookInfo = taleInfos[bookObjs.IndexOf(selectedBook)];
        NK_BookCover.instance.SetBookCoverFont(selectedBookInfo.fontStyle, selectedBookInfo.fontColor, selectedBookInfo.fontSize, selectedBookInfo.fontPositionX, selectedBookInfo.fontPositionY);
        NK_BookCover.instance.SetBookCover(selectedBookInfo.coverColor, selectedBookInfo.sticker, selectedBookInfo.stickerPositionX, selectedBookInfo.stickerPositionY);
    }

    public void UpdateBookContent()
    {
        // 책 내용 수정
        SceneManager.LoadScene("EditorScene");
    }

    public void DeleteObj()
    {
        // 삭제 버튼 눌러서 마지막에 클릭된 스티커 삭제
        if (delSticker != null)
            Destroy(delSticker);
    }
    /// ///////////////////////////////////////////////////////////////////////////////////
    public void SaveBookCover()
    {
        // Json으로 책 표지 저장
        TaleInfo taleInfo = NK_BookCover.instance.taleInfo;

        taleInfo.id = taleInfos[bookObjs.IndexOf(selectedBook)].id;
        //print(taleInfo.id);
        print(data.Length);
        NK_BookCover.instance.SaveTaleInfo();

        // 책 표지 캡쳐 및 서버에 보내기 작업
        StopAllCoroutines();
        StartCoroutine(TakeScreenShotRoutine());
    }

    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();

        // 화면 크기의 텍스쳐 생성
        Texture2D screenTex = new Texture2D(300, 400, TextureFormat.RGB24, false);
        // 캡쳐할 영역 지정
        Rect area = new Rect(452f, 320f, 300f, 400f);
        // 텍스쳐 픽셀에 지정
        screenTex.ReadPixels(area, 0, 0);
        screenTex.Apply();

        // 폴더가 존재하지 않으면 새로 생성
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        string fileName = path + "book" + ".png";
        NK_BookCover.instance.taleInfo.inputImg = screenTex.EncodeToPNG();

        // 스크린샷 저장
        //File.WriteAllBytes(fileName, screenTex.EncodeToPNG());
        // 텍스쳐 메모리 해제
        Destroy(screenTex);

        // 표지 정보 보내기
        TalePost_API(NK_BookCover.instance.taleInfo);
    }

    public void ExitBookShelf()
    {
        SceneManager.LoadScene("MyRoomScene");
    }

    public void ExitDetail()
    {
        detailUI.SetActive(false);
        booksParent.SetActive(true);

        NK_BookTrigger[] childList = booksParent.GetComponentsInChildren<NK_BookTrigger>();
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        // 책 업데이트
        TaleGet_API();
    }

    public void ExitPopup()
    {
        bookCoverUI.SetActive(false);
    }
}
