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

// 책 UI 위주, Json받고 보내기
public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<Texture2D> images = new List<Texture2D>();
    public List<string> textContent = new List<string>();
    public GameObject booksParent;
    public GameObject bookFactory;
    // 책 세부 내용 보기에 필요한 속성
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // 책 표지 수정에 필요한 속성
    public GameObject bookCoverUI;
    // 선택된 책
    public GameObject selectedBook;
    public GameObject nextBtn;
    public GameObject prevBtn;
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

        // 로딩 UI 초기화
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

            for (int i = 0; i < data.Length; i++)
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
        // 책 표지 이미지 받아오기
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = taleInfos[index].thumbNail;
        requester.requestType = RequestType.IMAGE;
        requester.onComplete = (handler) =>
        {
            // 책 표지 이미지 텍스쳐로 받아오기
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            // 책 표지 이미지 스프라이트로 만들어서 수정된 책에 적용 및 제목 텍스트 비활성화
            Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            booksParent.transform.GetChild(index).GetComponent<Image>().sprite = tempSprite;
            booksParent.transform.GetChild(index).GetComponentInChildren<Text>().enabled = false;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void GetDetailImage()
    {
        // 책 미리보기 RawImage 가져오기
        print("동화책 선택 완.");
        Info title = new Info();
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/" + taleInfos[bookObjs.IndexOf(selectedBook)].id;
        requester.requestType = RequestType.GET;
        requester.onComplete = (handler) =>
        {
            JObject taleJObj = JObject.Parse(handler.downloadHandler.text);
            Debug.Log("이 동화 맞아? \n" + handler.downloadHandler.text);
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
        // 책 내용 이미지 받아오기
        NK_HttpMediaRequester requester = new NK_HttpMediaRequester();
        requester.url = url;
        requester.requestType = RequestType.IMAGE;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            // 책 내용 이미지 텍스쳐로 받아오기
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            images[idx] = texture;
            if (idx == 0)
                rawImage.texture = images[idx];
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void DeleteBook()
    {
        // 동화책 삭제
        print("동화책 삭제하기");
        Info title = new Info();
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/" + taleInfos[bookObjs.IndexOf(selectedBook)].id;
        requester.requestType = RequestType.DELETE;
        requester.onComplete = (handler) =>
        {
            print("동화 삭제됨! \n" + handler.downloadHandler.text);
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
        // 책 선택하면 책 미리보기 보여짐
        selectedBook = EventSystem.current.currentSelectedGameObject;
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);
        nextBtn.SetActive(false);
        prevBtn.SetActive(false);

        // rawImage 불러오고 초기화
        images.Clear();
        index = 0;
        rawImage.rectTransform.sizeDelta = new Vector2(800, 500);
        GetDetailImage();
    }
    public void ClickBefore()
    {
        // 책 미리보기에서 이전 버튼 클릭 시
        if (0 < index)
        {
            index--;
            rawImage.texture = images[index];
        }
    }

    public void ClickNext()
    {
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
          "onupdatetarget", gameObject, "onupdate", "그냥러프써"));
        }
        else
        {*/
            iTween.ValueTo(gameObject, iTween.Hash(
            "from", x, "to", x + change, "time", 0.3f,
            "onupdatetarget", gameObject, "onupdate", "그냥러프써"));
        //}

    }

    void 그냥러프써(float v)
    {
        RectTransform rt = contentPos.GetComponent<RectTransform>();
        Vector2 v2 = rt.anchoredPosition;
        v2.x = v;
        rt.anchoredPosition = v2;

    }

    public void UpdateBookCover()
    {
        // 책 표지 수정
        bookCoverUI.SetActive(true);
        //NK_BookCover.instance.Initialization();
        NK_BookCover.instance.inputField.text = detailTitle.text;
        TaleInfo selectedBookInfo = taleInfos[bookObjs.IndexOf(selectedBook)];
        // 표지 이미지가 있을 때만 서버에서 글씨 및 스티커 불러옴
        if (selectedBook.GetComponent<Image>().sprite.name != "FairyTaleBook_White")
        {
            NK_BookCover.instance.SetBookCoverFont(selectedBookInfo.fontStyle, selectedBookInfo.fontColor, selectedBookInfo.fontSize, selectedBookInfo.fontPositionX, selectedBookInfo.fontPositionY);
            NK_BookCover.instance.SetBookCover(selectedBookInfo.coverColor, selectedBookInfo.sticker, selectedBookInfo.stickerPositionX, selectedBookInfo.stickerPositionY);
        }
    }

    // 로딩 중 UI
    public GameObject loadingBookContent;

    public void UpdateBookContent()
    {
        loadingBookContent.SetActive(true);
        YJ_DataManager.instance.preScene = "BookShelfScene";
        YJ_DataManager.instance.updateBookId = taleInfos[bookObjs.IndexOf(selectedBook)].id;
        // 책 내용 수정
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

    // taleInfo > 동화책 표지정보를 가져옴
    public void SaveBookCover()
    {
        // Json으로 책 표지 저장
        TaleInfo taleInfo = NK_BookCover.instance.taleInfo;
        // 선택된 책의 TaleInfo의 id를 받아옴
        taleInfo.id = taleInfos[bookObjs.IndexOf(selectedBook)].id;
        //print(taleInfo.id);
        print(data.Length);
        // 선택된 책의 Json을 만들 TaleInfo에 정보 저장
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
        Rect area = new Rect(462f, 266f, 300f, 400f);
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
        System.IO.File.WriteAllBytes(fileName, screenTex.EncodeToPNG());
        // 텍스쳐 메모리 해제
        Destroy(screenTex);

        // 선택된 책의 표지 정보 보내기
        TalePost_API(NK_BookCover.instance.taleInfo);
        // 표지 수정 팝업 닫기
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

        // 책 업데이트
        TaleGet_API();
    }

    public void ExitPopup()
    {
        bookCoverUI.SetActive(false);
    }
}
