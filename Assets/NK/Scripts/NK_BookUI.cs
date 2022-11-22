using Newtonsoft.Json.Linq;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static NK_BookUI;

public class NK_BookUI : MonoBehaviourPun
{
    public List<string> titles = new List<string>();
    public GameObject bookUI;
    public GameObject fairyTaleManager;
    public List<PageInfo> objs;
    public GameObject textFactory;
    public Transform fairyTaleUI;
    public Transform fairyTaleObject;
    public GameObject booksParent;
    public GameObject bookFactory;

    public AudioSource audioSource;

    Dictionary<int, List<PageInfo>> sceneObjects = new Dictionary<int, List<PageInfo>>();
    List<TaleInfo> taleInfos;
    List<AudioClip> audioClips;
    List<GameObject> bookObjects;

    //동화 조회할 아이디
    public string id;

    private void Start()
    {
        audioClips = new List<AudioClip>();
        bookObjects = new List<GameObject>();
    }

    public void ClickBookList()
    {
        // 기존에 자식 리스트가 있으면 모두 삭제
        Transform[] childList = booksParent.GetComponentsInChildren<Transform>();

        if (childList.Length > 1)
        {
            return;
        }

        GetBookList();
    }

    public void ClickBook()
    {
        pageNum = 0;
        sceneObjects = new Dictionary<int, List<PageInfo>>();
        GameObject book = EventSystem.current.currentSelectedGameObject;
        photonView.RPC("RPCSetActive", RpcTarget.All);
        GetBookInfo(bookObjects.IndexOf(book));
        print(book.GetComponentInChildren<Text>().text);
    }

    [PunRPC]
    private void RPCSetActive()
    {
        bookUI.SetActive(false);
        fairyTaleManager.SetActive(true);
    }
    [PunRPC]
    private void RPCSetInactive()
    {
        fairyTaleManager.SetActive(false);
    }

    bool isOpen = false;

    public virtual void Update()
    {
        if (isOpen) return;
        // 오브젝트가 활성화 되어있을 때
        if (sceneObjects.Count > 0 && fairyTaleObject.gameObject.activeSelf && !isOpen)
        {
            // 첫 페이지의 오브젝트를 띄움
            InstantiateObject();
            isOpen = true;
        }
    }

    public void GetBookList()
    {
        // 동화책 목록가져오기
        taleInfos = new List<TaleInfo>();
        bookObjects = new List<GameObject>();
        YJ_HttpRequester requester1 = new YJ_HttpRequester();
        requester1.url = "http://43.201.10.63:8080/tale/mylist";
        requester1.requestType = RequestType.GET;
        requester1.headers = new Dictionary<string, string>();
        requester1.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester1.onComplete = (handler) =>
        {

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
                titles.Add(taleList.title);
                data[i].taleList = taleList;
            }

            for (int j = 0; j < data.Length; j++)
            {
                TaleInfo taleInfo = new TaleInfo();
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
                bookObjects.Add(book);
                GetBookImage(i);
            }
        };
        YJ_HttpManager.instance.SendRequest(requester1);
    }

    public void GetBookInfo(int index)
    {
        print("동화책 선택 완.");
        Info title = new Info();
        YJ_HttpRequester requester2 = new YJ_HttpRequester();
        requester2.url = "http://43.201.10.63:8080/tale/" + taleInfos[index].id;
        requester2.requestType = RequestType.GET;
        requester2.onComplete = (handler) =>
        {
            Debug.Log("이 동화 맞아? \n" + handler.downloadHandler.text);
            title = JsonUtility.FromJson<Info>(handler.downloadHandler.text);
            BookInfo bookInfo = title.data;
            List<PagesInfo> pagesInfos = bookInfo.pages;
            SetBook(pagesInfos);

            // 오디오 받아오기
            JObject taleJObj = JObject.Parse(handler.downloadHandler.text);
            for (int i = 0; i < title.data.pages.Count; i++)
            {
                audioClips.Add(null);
                if (taleJObj["data"]["pages"][i]["audioUrl"].ToString() != " ")
                {
                    GetBookAudio(taleJObj["data"]["pages"][i]["audioUrl"].ToString(), i);
                }
            }
        };
        YJ_HttpManager.instance.SendRequest(requester2);

    }

    public void GetBookImage(int index)
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

    public void GetBookAudio(string url, int index)
    {
        if (url == "")
            return;
        // 책 오디오 받아오기
        NK_HttpMediaRequester requester = new NK_HttpMediaRequester();
        requester.url = url;
        requester.requestType = RequestType.AUDIO;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            if (handler.downloadHandler.data.Length < 10)
                return;
            // 책 오디오 클립 받아오기
            AudioClip clip = DownloadHandlerAudioClip.GetContent(handler);
            audioClips[idx] = clip;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    [Serializable]
    public class Info
    {
        public string status;
        public string message;
        public BookInfo data;
    }

    ///

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
    }

    [Serializable]
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


    public void SetBook(List<PagesInfo> pagesInfos)
    {
        // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
        foreach (PagesInfo pagesInfo in pagesInfos)
        {
            objs = new List<PageInfo>();

            foreach (string pageInfo in pagesInfo.data)
            {
                print(pageInfo);
                objs.Add(pagesInfo.DeserializePageInfo(pageInfo));
                sceneObjects[pagesInfo.page] = objs;
            }
        }
    }

    public int pageNum;

    public void InstantiateObject()
    {
        // pageNum에 따른 씬 오브젝트 리스트에 저장
        List<PageInfo> objs = sceneObjects[pageNum];

        for (int i = 0; i < objs.Count; i++)
        {
            // 페이지마다 텍스트 띄우기
            if (objs[i].type == "text")
            {
                TxtInfo txt = (TxtInfo)objs[i];
                GameObject textObj = PhotonNetwork.Instantiate("NK/" + textFactory.name, Vector3.zero, Quaternion.identity);
                photonView.RPC("RPCCreateText", RpcTarget.All, textObj.GetPhotonView().ViewID, txt.content, txt.size, txt.position, txt.font, txt.color);
            }
            // 페이지마다 오브젝트 띄우기
            if (objs[i].type == "obj")
            {
                ObjInfo obj = (ObjInfo)objs[i];
                GameObject objPrefab = PhotonNetwork.Instantiate(obj.prefab, obj.position + new Vector3(0, 2.11f, -1.4f) - new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1)), obj.rotation);
                photonView.RPC("RPCCreateObject", RpcTarget.All, objPrefab.GetPhotonView().ViewID, obj.scale, obj.anim);
            }
        }

        // 페이지마다 음성파일 재생
/*        if (Resources.Load<AudioClip>("fairyTale1/Page" + pageNum) != null)
        {
            // 양치기 소년을 위한 스크립트!!!
            print("Page" + pageNum);
            photonView.RPC("RPCCreateAudio", RpcTarget.All, pageNum);
        }
        else
        {*/
            photonView.RPC("RPCCreateTTS", RpcTarget.All, pageNum);
        //}
    }

    IEnumerator PlayAnim(Animator animator, string anim)
    {
        // 애니메이션 플레이
        yield return null;
        animator.Play(anim);
    }

    [PunRPC]
    private void RPCDestroyObject()
    {
        // 씬이 달라지면 오브젝트 지우기
        Transform[] texts = fairyTaleUI.GetComponentsInChildren<Transform>();
        Transform[] objects = fairyTaleObject.GetComponentsInChildren<Transform>();

        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] != fairyTaleUI.transform)
                    Destroy(texts[i].gameObject);
            }
        }

        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != fairyTaleObject.transform)
                    Destroy(objects[i].gameObject);
            }
        }

        if (audioSource.clip != null)
        {
            audioSource.clip = null;
        }
    }

    [PunRPC]
    private void RPCCreateText(int viewId, string content, int size, Vector3 position, string font, string color)
    {
        PhotonView view = PhotonView.Find(viewId);
        GameObject textObj = view.gameObject;
        Text textInfo = textObj.GetComponent<Text>();
        textInfo.text = content;
        // 폰트 적용
        Font fontInfo;
        if (font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(font);
        textInfo.font = fontInfo;
        // 색깔 적용
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + color, out colorInfo);
        textInfo.color = colorInfo;
        // 폰트 사이즈 변경
        textInfo.fontSize = size;
        textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(textInfo.preferredWidth, textInfo.preferredHeight);
        textObj.transform.SetParent(fairyTaleUI);
        position = new Vector3(position.x, position.y / 3, position.z);
        textObj.GetComponent<RectTransform>().anchoredPosition = position;
    }

    Animator animator;
    [PunRPC]
    private void RPCCreateObject(int viewId, Vector3 scale, string anim)
    {
        PhotonView view = PhotonView.Find(viewId);
        GameObject objPrefab = view.gameObject;
        objPrefab.transform.SetParent(fairyTaleObject);
        objPrefab.transform.localScale = scale;
        // 애니메이션이 있다면
        if (anim != "")
        {
            // 애니메이터를 가져옴
            if (objPrefab.GetComponent<Animator>() == null)
            {
                animator = objPrefab.transform.GetChild(0).GetComponent<Animator>();
            }
            else
            {
                animator = objPrefab.GetComponent<Animator>();
            }
            StartCoroutine(PlayAnim(animator, anim));
        }
        // 크기 커지면서 보이도록
        iTween.ScaleFrom(objPrefab, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", "easeInOutBack"));
    }

    [PunRPC]
    private void RPCCreateAudio(int index)
    {
        // 로컬로 오디오 소스 불러오기
        audioSource.clip = Resources.Load<AudioClip>("fairyTale1/Page" + index);
        audioSource.Play();
    }
    
    [PunRPC]
    private void RPCCreateTTS(int index)
    {
        // 서버에서 오디오 소스 불러오기
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }


    #region ClickNext // 다음 버튼 클릭
    public void ClickNext()
    {
        // 현재 페이지와 총 페이지 수 비교
        if (sceneObjects.Count > pageNum + 1 && fairyTaleUI.gameObject.activeSelf)
        {
            pageNum++;
            photonView.RPC("RPCDestroyObject", RpcTarget.All);
            InstantiateObject();
        }
    }
    #endregion

    #region ClickBefore // 이전 버튼 클릭
    public void ClickBefore()
    {
        if (0 <= pageNum - 1 && fairyTaleUI.gameObject.activeSelf)
        {
            pageNum--;
            photonView.RPC("RPCDestroyObject", RpcTarget.All);
            InstantiateObject();
        }
    }
    #endregion

    #region ClickEnd // 동화책 끄기
    public void ClickEnd()
    {
        photonView.RPC("RPCDestroyObject", RpcTarget.All);
        photonView.RPC("RPCSetInactive", RpcTarget.All);
        isOpen = false;
        NK_UIController.instance.ClickControl(false);
    }
    #endregion
}
