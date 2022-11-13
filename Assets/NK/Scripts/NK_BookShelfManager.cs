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
    // 책 세부 내용 보기에 필요한 속성
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // 책 표지 수정에 필요한 속성
    public GameObject bookCoverUI;
    // 삭제될 스티커
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

        //titles.Add("양치기소년");
        //titles.Add("신데렐라");
        //titles.Add("강아지 똥");

        path = Application.dataPath + "/BookCover/";

    }

    // 동화책 목록가져오기
    public void TaleSet_API()
    {

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale/mylist";
        requester.requestType = RequestType.GET;
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.onComplete = (handler) => {

            Debug.Log("자 동화목록 받아왔어! \n" + handler.downloadHandler.text);

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

            //동화책 제목 추가하면
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].taleList.title == null) continue;
                titles.Add(data[j].taleList.title);
            }

            for (int i = 0; i < titles.Count; i++)
            {
                // 제목 추가된 개수만큼 동화책 생성
                GameObject book = Instantiate(bookFactory, booksParent.transform);
                // JSON에서 불러온 제목으로 텍스트 지정
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
        // 책 선택하면 책 미리보기 보여짐
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);

        // rawImage 불러오고 초기화
        index = 0;
        rawImage.texture = images[index];
    }

    int index = 0;
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

    public void UpdateBookCover()
    {
        // 책 표지 수정
        bookCoverUI.SetActive(true);
        NK_BookCover.instance.inputField.text = detailTitle.text;
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

    public void SaveBookCover()
    {
        // 책 표지 캡쳐
        StopAllCoroutines();
        StartCoroutine(TakeScreenShotRoutine());
        // Json으로 책 표지 저장
        // NK_BookCover.instance.bookCover.gameObject
        //ExitPopup();
    }

    private IEnumerator TakeScreenShotRoutine()
    {
        yield return new WaitForEndOfFrame();

        // 화면 크기의 텍스쳐 생성
        Texture2D screenTex = new Texture2D(310, 410, TextureFormat.RGB24, false);
        // 캡쳐할 영역 지정
        Rect area = new Rect(485f, 400f, 310f, 410f);
        // 텍스쳐 픽셀에 지정
        screenTex.ReadPixels(area, 0, 0);
        
        // 폴더가 존재하지 않으면 새로 생성
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        string fileName = path + "book" + ".png";

        // 스크린샷 저장
        File.WriteAllBytes(fileName, screenTex.EncodeToPNG());
        // 텍스쳐 메모리 해제
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
