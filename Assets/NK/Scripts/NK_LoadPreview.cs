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
    public int pageNum;
    GameObject n_Scene;
    GameObject n_Scene_Canvas;

    Animator animator;
    List<PageInfo> objs;
    Dictionary<int, List<PageInfo>> sceneObjects = new Dictionary<int, List<PageInfo>>();

    private void Start()
    {
        pageNum = 0;
        images = new List<Texture2D>();
        voices = new List<AudioClip>();
        // 이전 씬이 프리뷰씬이라면
        if (YJ_DataManager.instance.preScene == "PreviewScene")
        {
            LoadObjects();
        }

        // 이전 씬이 책장씬이라면
        if (YJ_DataManager.instance.preScene == "BookShelfScene")
        {
            LoadObjects(YJ_DataManager.instance.updateBookId);
        }
    }

    public void LoadPreview()
    {
        YJ_DataManager.instance.bookTitle = SH_BtnManager.Instance.title;
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
        // 임시로 저장한 제목 넣어주기
        SH_BtnManager.Instance.title = YJ_DataManager.instance.bookTitle;

        // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
        // pagesInfo.ttsText 여부에 따라서 다시 설정해주기
        // TTS면 Null넣어주기
        // 녹음이면 -> 0페이지면 Insert // 아니라면 ADD
        foreach (PagesInfo pagesInfo in pagesInfos)
        {

            VoiceInfo voiceInfo = new VoiceInfo();
            // 녹음을 선택했을 때
            if (pagesInfo.ttsText == "")
            {
                // 클래스 세팅
                voiceInfo.ttsBtn = SH_VoiceRecord.Instance.ttsUnCheked;
                voiceInfo.recordNum = 1;
                SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
                SH_VoiceRecord.Instance.ttsBtn.interactable = false;
                SH_VoiceRecord.Instance.recordBtn.interactable = true;
                if (pagesInfo.page == 0)
                {
                    SH_VoiceRecord.Instance.voiceClip.RemoveAt(0);
                    SH_VoiceRecord.Instance.voiceClip.Add(Resources.Load<AudioClip>("Page" + pagesInfo.page));
                }
                else
                {
                    SH_VoiceRecord.Instance.voiceClip.Add(Resources.Load<AudioClip>("Page" + pagesInfo.page));


                }
                print("녹음 선택");
            }
            // TTS를 선택했을 때
            else if (pagesInfo.ttsText != "")
            {
                voiceInfo.ttsBtn = SH_VoiceRecord.Instance.ttsChecked;
                voiceInfo.recordNum = 0;
                SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsChecked;
                SH_VoiceRecord.Instance.ttsBtn.interactable = true;
                SH_VoiceRecord.Instance.recordBtn.interactable = false;
                if (pagesInfo.page != 0)
                {
                    SH_VoiceRecord.Instance.voiceClip.Add(null);

                }

                print("TTS 선택");


            }
            //else
            //{
            //    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
            //    SH_VoiceRecord.Instance.ttsBtn.interactable = true;
            //    SH_VoiceRecord.Instance.recordBtn.interactable = true;
            //}
            SH_VoiceRecord.Instance.voiceInfos.Add(voiceInfo);
            objs = new List<PageInfo>();

            foreach (string pageInfo in pagesInfo.data)
            {
                print(pageInfo);
                objs.Add(pagesInfo.DeserializePageInfo(pageInfo));
                sceneObjects[pagesInfo.page] = objs;
            }
        }
        print("씬 개수 : " + sceneObjects.Count);
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
            SH_BtnManager.Instance.title = bookInfo.title;

            // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
            for (int i = 0; i < pagesInfos.Count; i++)
            {
                objs = new List<PageInfo>();
                images.Add(null);
                GetRawImage(taleJObj["data"]["pages"][i]["rawImgUrl"].ToString(), i);
                voices.Add(null);
                GetVoice(taleJObj["data"]["pages"][i]["audioUrl"].ToString(), i);

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

            // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
            // pagesInfo.ttsText 여부에 따라서 다시 설정해주기
            // TTS면 Null넣어주기
            // 녹음이면 -> 0페이지면 Insert // 아니라면 ADD
            for(int i = 0; i < pagesInfos.Count; i++)
            
            //foreach (PagesInfo pagesInfo in pagesInfos)
            {
                PagesInfo pagesInfo = pagesInfos[i];
                VoiceInfo voiceInfo = new VoiceInfo();
                voiceInfo.ttsBtn = SH_VoiceRecord.Instance.ttsUnCheked;
                // 녹음을 선택했을 때
                if (taleJObj["data"]["pages"][pagesInfo.page]["audioUrl"].ToString() != "")
                {
                    // 클래스 세팅                    
                    voiceInfo.recordNum = 1;                    

                    if (pagesInfo.page == 0)
                    {
                        SH_VoiceRecord.Instance.voiceClip.RemoveAt(0);
                        SH_VoiceRecord.Instance.voiceClip.Add(Resources.Load<AudioClip>("Page" + pagesInfo.page));
                    }
                    else
                    {
                        SH_VoiceRecord.Instance.voiceClip.Add(Resources.Load<AudioClip>("Page" + pagesInfo.page));
                    }
                }
                else
                {
                    SH_VoiceRecord.Instance.voiceClip.Add(null);
                }
                SH_VoiceRecord.Instance.voiceInfos.Add(voiceInfo);

                if(i == pagesInfos.Count - 1)
                {
                    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
                    SH_VoiceRecord.Instance.recordBtn.interactable = true;
                    if (voiceInfo.recordNum == 1)
                    {
                        SH_VoiceRecord.Instance.ttsBtn.interactable = false;
                    }
                    else
                    {
                        SH_VoiceRecord.Instance.ttsBtn.interactable = true;
                    }
                }

                print("녹음 선택");
                //else
                //{
                //    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
                //    SH_VoiceRecord.Instance.ttsBtn.interactable = true;
                //    SH_VoiceRecord.Instance.recordBtn.interactable = true;
                //}
            }
        };
        YJ_HttpManager.instance.SendRequest(requester2);
    }
    #endregion

    public void InstantiateObject()
    {
        // pageNum에 따른 씬 오브젝트 리스트에 저장
        print(pageNum);
        List<PageInfo> objs = sceneObjects[pageNum];
        SH_BtnManager.Instance.currentSceneNum = pageNum;
        SH_BtnManager.Instance.currentScene = pageNum;
        SH_BtnManager.Instance.i = pageNum;
        if (YJ_DataManager.instance.preScene == "PreviewScene")
        {
            AddPreviewImage(pageNum);
        }
        else
        {
            AddImage(pageNum);
            AddVoice(pageNum);
        }
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
        SH_InputField sh_textInfo = textObj.GetComponent<SH_InputField>();
        textInfo.text = txt.content;
        // 폰트 적용
        Font fontInfo;
        if (txt.font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(txt.font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(txt.font);
        textInfo.textComponent.font = fontInfo;
        for (int i = 0; i < SH_EditorManager.Instance.fonts.Length; i++)
        {
            if (SH_EditorManager.Instance.fonts[i].name == txt.font)
                sh_textInfo.info.txtDropdown = i;
        }
        // 색깔 적용
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + txt.color, out colorInfo);
        textInfo.textComponent.color = colorInfo;
        // 폰트 사이즈 변경
        sh_textInfo.info.txtSize = txt.size;
        textInfo.textComponent.fontSize = txt.size;
        textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(textInfo.preferredWidth, textInfo.preferredHeight);
        textObj.transform.SetParent(n_Scene_Canvas.transform);
        textObj.GetComponent<RectTransform>().anchoredPosition = txt.position;
    }

    private void CreateObject(ObjInfo obj)
    {
        GameObject objPrefab = (GameObject)Instantiate(Resources.Load(obj.prefab));
        //SH_SceneObj sceneObj = objPrefab.GetComponent<SH_SceneObj>();
        SH_EditorManager.Instance.activeObj = objPrefab;
        objPrefab.transform.SetParent(n_Scene.transform);
        objPrefab.transform.position = obj.position;
        objPrefab.transform.localRotation = obj.rotation;
        objPrefab.transform.localScale = obj.scale;
        // 애니메이션이 있다면
        if (obj.anim != "")
        {
            objPrefab.GetComponent<SH_SceneObj>().currentAnim = obj.anim;
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
            n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 1080, 0) * (sceneObjects.Count - (pageNum + 1));
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
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 1080, 0) * (sceneObjects.Count - (pageNum + 1));
        // 이 오브젝트들을 List에 추가해볼까?
        SH_BtnManager.Instance.Scenes.Add(n_Scene);
        SH_BtnManager.Instance.Scenes_txt.Add(n_Scene_Canvas);

        //SH_VoiceRecord.Instance.voiceClip.Add(null);
        // TTS 버튼과 녹음 버튼도 초기화 시켜볼까?
        //SH_VoiceRecord.Instance.Reset();

        pageNum++;
    }


    List<Texture2D> images;
    public void GetRawImage(string url, int index)
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
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    private void AddImage(int i)
    {
        // RawImage 불러오기
        // 서버에 저장된 RawImage 불러올 것
        // RawImage0 오브젝트는 이미 있으므로
        if (i == 0)
        {
            // 한 페이지밖에 없을 때
            if (sceneObjects.Count == 1)
            {
                SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
                SH_BtnManager.Instance.sceneCam.targetTexture = SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture as RenderTexture;
            }
            else
            {
                StartCoroutine(ApplyTexture(SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>(), i));
            }
            return;
        }
        // 새로운 Rawimage 추가
        // 맨 밑에 추가해야한다
        GameObject raw = Instantiate(SH_BtnManager.Instance.rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = SH_BtnManager.Instance.firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + i;
        SH_BtnManager.Instance.rawImages.Add(raw.GetComponent<RawImage>());
        StartCoroutine(ApplyTexture(raw.GetComponent<RawImage>(), i));
    }

    IEnumerator ApplyTexture(RawImage raw, int i)
    {
        yield return new WaitUntil(() => images[i] != null);
        raw.texture = images[i];
        SH_BtnManager.Instance.rawImageList.Add(images[i].EncodeToJPG());
        // 마지막 페이지의 RawImage만 RenderTexture로
        if (i == sceneObjects.Count - 1)
        {
            raw.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
            SH_BtnManager.Instance.sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        }
    }

    private void AddPreviewImage(int i)
    {
        // 프리뷰씬일 때는 서버에 저장이 안되므로 로컬에서 불러와야함
        string path = Application.dataPath + "/Capture/_" + pageNum + ".jpg";
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(0, 0);
        // RawImage0 오브젝트는 이미 있으므로
        if (i == 0)
        {
            texture2D.LoadImage(byteTexture);
            SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture = texture2D;
            return;
        }
        // 새로운 Rawimage 추가
        // 맨 밑에 추가해야한다
        GameObject raw = Instantiate(SH_BtnManager.Instance.rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = SH_BtnManager.Instance.firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + i;
        texture2D.LoadImage(byteTexture);
        raw.GetComponent<RawImage>().texture = texture2D;
        SH_BtnManager.Instance.rawImages.Add(raw.GetComponent<RawImage>());
        SH_BtnManager.Instance.rawImageList.Add(byteTexture);
        // 마지막 페이지의 RawImage만 RenderTexture로
        if (i == sceneObjects.Count - 1)
        {
            raw.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
            SH_BtnManager.Instance.sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        }
    }

    List<AudioClip> voices;
    public void GetVoice(string url, int index)
    {
        if (url == "")
            return;
        // 책 녹음 받아오기
        NK_HttpMediaRequester requester = new NK_HttpMediaRequester();
        requester.url = url;
        requester.requestType = RequestType.AUDIO;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            //File.WriteAllBytes(Application.dataPath + "/aaaa.wav", handler.downloadHandler.data);
            // 책 녹음 오디오 클립으로 받아오기
            AudioClip audio = DownloadHandlerAudioClip.GetContent(handler);
            voices[idx] = audio;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    private void AddVoice(int i)
    {
        StartCoroutine(ApplyVoice(i));
    }

    IEnumerator ApplyVoice(int i)
    {
        yield return new WaitUntil(() => voices[i] != null);
        SH_VoiceRecord.Instance.voiceClip[i] = voices[i];
        SH_SavWav.Save("Page" + i, voices[i]);
        SH_VoiceRecord.Instance.num = i;
    }
}
