using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private void Start()
    {
        // 동화책 제목 추가하면
        titles.Add("양치기 소년");
        titles.Add("신데렐라");
        titles.Add("오즈의 마법사");
        for (int i = 0; i < titles.Count; i++)
        {
            // 제목 추가된 개수만큼 동화책 생성
            GameObject book = Instantiate(bookFactory, booksParent.transform);
            // JSON에서 불러온 제목으로 텍스트 지정
            book.GetComponentInChildren<Text>().text = titles[i];
            book.GetComponent<Button>().onClick.AddListener(ClickBook);
        }
    }

    public void ClickBook()
    {
        pageNum = 0;
        GameObject book = EventSystem.current.currentSelectedGameObject;
        photonView.RPC("RPCSetActive", RpcTarget.All);
        ClickBook("Book1");
        //ClickBook(book.GetComponentInChildren<Text>().text);
        print(book.GetComponentInChildren<Text>().text);
    }
/*
    public void ClickBook1()
    {
        SelectBook(Book.양치기소년);
        photonView.RPC("RPCSetActive", RpcTarget.All);
        ClickBook("Book1");
    }

    public void ClickBook2()
    {
        SelectBook(Book.신데렐라);
        photonView.RPC("RPCSetActive", RpcTarget.All);
        ClickBook("Book2");
    }

    public void ClickBook3()
    {
        SelectBook(Book.오즈의마법사);
    }
    public void ClickBook4()
    {
        SelectBook(Book.용이야기);
        bookUI.SetActive(false);
        fairyTaleManager.SetActive(true);
    }*/

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

    private void Update()
    {
        // 오브젝트가 활성화 되어있을 때
        if (sceneObjects.Count > 0 && fairyTaleObject.gameObject.activeSelf && !isOpen)
        {
            // 첫 페이지의 오브젝트를 띄움
            InstantiateObject();
            isOpen = true;
        }
    }

    public void ClickBook(string jsonName)
    {
        // Json 파일 받아오기
        string fileName = jsonName;
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // 파싱
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;

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
        if (Resources.Load<AudioClip>("Page" + pageNum) != null)
        {
            print("Page" + pageNum);
            photonView.RPC("RPCCreateAudio", RpcTarget.All, pageNum);
        }
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
        audioSource.clip = Resources.Load<AudioClip>("Page" + index);
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
    }
    #endregion
}
