using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NK_BookUI;

public class NK_LoadPreview : MonoBehaviour
{
    public GameObject newScene;
    public GameObject newScene_Canvas;
    public GameObject inputField;
    GameObject n_Scene;
    GameObject n_Scene_Canvas;

    Animator animator;
    List<PageInfo> objs;

    Dictionary<int, List<PageInfo>> sceneObjects = new Dictionary<int, List<PageInfo>>();

    private void Start()
    {
        // 이전 씬이 프리뷰씬이라면
        if (YJ_DataManager.instance.preScene == "PreviewScene")
        {
            LoadObjects();
            YJ_DataManager.instance.preScene = null;
        }

        // 이전 씬이 책장씬이라면
        if (YJ_DataManager.instance.preScene == "BookShelfScene")
        {
            LoadObjects(YJ_DataManager.instance.updateBookId);
            YJ_DataManager.instance.preScene = null;
        }
    }

    public void LoadPreview()
    {
        SceneManager.LoadScene("PreviewScene");
    }

    public void LoadEditor()
    {
        // 이전 씬 이름을 프리뷰씬이라고 저장
        YJ_DataManager.instance.preScene = "PreviewScene";
        SceneManager.LoadScene("EditorScene");
    }

    #region LoadObjects // 프리뷰씬
    public void LoadObjects()
    {
        // Json 파일 받아오기
        string path = Application.dataPath + "/" + "in" + ".Json";
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

        for (int i = 0; i < sceneObjects.Count; i++)
            InstantiateObject();
    }
    #endregion

    #region LoadObjects // 책장씬에서 책 수정 불러오기
    public void LoadObjects(string id)
    {
        // 불러오기
        print("수정할 동화책 선택 완.");
        Info title = new Info();
        YJ_HttpRequester requester2 = new YJ_HttpRequester();
        requester2.url = "http://43.201.10.63:8080/tale/" + id;
        requester2.requestType = RequestType.GET;
        requester2.onComplete = (handler) =>
        {
            Debug.Log("이 동화 맞아? \n" + handler.downloadHandler.text);
            JObject taleJObj = JObject.Parse(handler.downloadHandler.text);
            title = JsonUtility.FromJson<Info>(handler.downloadHandler.text);
            BookInfo bookInfo = title.data;
            List<PagesInfo> pagesInfos = bookInfo.pages;

            // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
            for(int i = 0; i < pagesInfos.Count; i++)
            {
                objs = new List<PageInfo>();
                images.Add(null);
                GetRawImage(taleJObj["data"]["pages"][i]["rawImgUrl"].ToString(), i);

                PagesInfo pagesInfo = pagesInfos[i];
                foreach (string pageInfo in pagesInfo.data)
                {
                    print(pageInfo);
                    objs.Add(pagesInfo.DeserializePageInfo(pageInfo));
                    sceneObjects[pagesInfo.page] = objs;
                }
            }
            print("씬 : " + sceneObjects.Count);
            for (int i = 0; i < sceneObjects.Count; i++)
                InstantiateObject();
        };
        YJ_HttpManager.instance.SendRequest(requester2);
    }
    #endregion

    public int pageNum;

    public void InstantiateObject()
    {
        // pageNum에 따른 씬 오브젝트 리스트에 저장
        List<PageInfo> objs = sceneObjects[pageNum];
        SH_BtnManager.Instance.currentSceneNum = pageNum;
        AddImage(pageNum);
        AddScene(pageNum);
        for (int i = 0; i < objs.Count; i++)
        {
            // 페이지마다 텍스트 띄우기
            if (objs[i].type == "text")
            {
                TxtInfo txt = (TxtInfo)objs[i];
                CreateText(txt);
            }
            // 페이지마다 오브젝트 띄우기
            if (objs[i].type == "obj")
            {
                ObjInfo obj = (ObjInfo)objs[i];
                CreateObject(obj);

            }
        }
    }

    private void CreateText(TxtInfo txt)
    {
        GameObject textObj = Instantiate(inputField);
        InputField textInfo = textObj.GetComponent<InputField>();
        textInfo.text = txt.content;
        // 폰트 적용
        Font fontInfo;
        if (txt.font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(txt.font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(txt.font);
        textInfo.textComponent.font = fontInfo;
        // 색깔 적용
        UnityEngine.Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + txt.color, out colorInfo);
        textInfo.textComponent.color = colorInfo;
        // 폰트 사이즈 변경
        textInfo.textComponent.fontSize = txt.size;
        textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(textInfo.preferredWidth, textInfo.preferredHeight);
        textObj.transform.SetParent(n_Scene_Canvas.transform);
        textObj.GetComponent<RectTransform>().anchoredPosition = txt.position;
    }

    private void CreateObject(ObjInfo obj)
    {
        GameObject objPrefab = (GameObject)Instantiate(Resources.Load(obj.prefab));
        SH_EditorManager.Instance.activeObj = objPrefab;
        objPrefab.transform.SetParent(n_Scene.transform);
        objPrefab.transform.position = obj.position;
        objPrefab.transform.localRotation = obj.rotation;
        objPrefab.transform.localScale = obj.scale;
        // 애니메이션이 있다면
        if (obj.anim != "")
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
            StartCoroutine(PlayAnim(animator, obj.anim));
        }
    }

    IEnumerator PlayAnim(Animator animator, string anim)
    {
        // 애니메이션 플레이
        yield return null;
        animator.Play(anim);
    }

    private void AddScene(int i)
    {
        // Scene0 오브젝트는 이미 있으므로
        if (i == 0)
        {
            n_Scene_Canvas = SH_BtnManager.Instance.Scenes_txt[0];
            n_Scene = SH_BtnManager.Instance.Scenes[0];
            // 빈 오브젝트들의 위치도 설정하자
            n_Scene.transform.position = new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
            n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
            pageNum++;
            return;
        }
        // 이제 오브젝트들을 싹 다 올렸으니 새로운 빈 오브젝트들을 만들고
        n_Scene = Instantiate(newScene);
        // 빈 오브젝트들의 이름도 바꿔야한다!
        n_Scene.name = "Scene" + (i);       // 씬 이름 : Scene0, Scene1, Scene2....
        n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene_txt" + (i);      // 씬 이름 : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // 빈 오브젝트들의 위치도 설정하자
        n_Scene.transform.position = new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
        // 이 오브젝트들을 List에 추가해볼까?
        SH_BtnManager.Instance.Scenes.Add(n_Scene);
        SH_BtnManager.Instance.Scenes_txt.Add(n_Scene_Canvas);
        pageNum++;
    }


    List<Texture2D> images = new List<Texture2D>();
    public void GetRawImage(string url, int index)
    {
        // 책 내용 이미지 받아오기
        NK_HttpDetailImage requester = new NK_HttpDetailImage();
        requester.url = url;
        requester.requestType = RequestType.IMAGE;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            // 책 내용 이미지 텍스쳐로 받아오기
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            images[idx] = texture;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    private void AddImage(int i)
    {
        // RawImage 불러오기
        // 지금은 로컬에 저장된 파일을 불러온다
        // 서버 구축되면 서버에 저장된 RawImage 불러올 것
        //string path = Application.dataPath + "/Capture/_CurrentScene_" + pageNum + ".png";
        //byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(0, 0);
        // RawImage0 오브젝트는 이미 있으므로
        if (i == 0)
        {
            texture2D = images[0];
            SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture = texture2D;
            return;
        }
        // 새로운 Rawimage 추가
        // 맨 밑에 추가해야한다
        GameObject raw = Instantiate(SH_BtnManager.Instance.rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = SH_BtnManager.Instance.firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + i;
        texture2D = images[i];
        raw.GetComponent<RawImage>().texture = texture2D;
        SH_BtnManager.Instance.rawImages.Add(raw.GetComponent<RawImage>());
        // 마지막 페이지의 RawImage만 RenderTexture로
        if (i == sceneObjects.Count - 1)
        {
            raw.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
            SH_BtnManager.Instance.sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        }
    }
}
