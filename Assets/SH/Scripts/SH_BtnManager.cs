using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using Photon.Pun;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;



// InputField 필요한 정보들
public class TextInfo
{
    public string inputs { get; set; }
    public int txtDropdown { get; set; }
    public int txtSize { get; set; }
    public Color txtColor { get; set; }
}

public class Json
{
    public string creator;
    public string[] books;
    public List<BookInfo> book;
}

[System.Serializable]
public class BookInfo
{
    public string id; //서버에 생성되는 차례 (고정)
    public string memberCode;
    public string title; //책 제목
    public string createAt; //만든날짜
    public List<PagesInfo> pages; //페이지들 정보

}

[System.Serializable]
public class PagesInfo
{
    public int page; //페이지 번호
    public List<string> data; //텍스트 정보, 오브젝트 정보
    public string ttsText;
    public byte[] voice;
    public byte[] rawImg;

    public string SerializePageInfo(PageInfo info)
    {
        // Class -> Json
        // 텍스트, 오브젝트 스트링 형식으로 변환
        string pageInfo = JsonUtility.ToJson(info);
        return pageInfo;
    }

    public PageInfo DeserializePageInfo(string s)
    {
        // Json -> Class
        PageInfo pageInfo = JsonUtility.FromJson<PageInfo>(s);
        if (pageInfo.type == "text")
        {
            pageInfo = JsonUtility.FromJson<TxtInfo>(s);
        }
        if (pageInfo.type == "obj")
        {
            pageInfo = JsonUtility.FromJson<ObjInfo>(s);
        }
        return pageInfo;
    }
}

[System.Serializable]
public class PageInfo //공통정보 타입과 위치
{
    public string type;
    public Vector3 position;
}

[System.Serializable]
public class TxtInfo : PageInfo //텍스트일경우 가져올 정보
{
    public string font;
    public int size;
    public string content;
    public string color;
}

[System.Serializable]
public class ObjInfo : PageInfo //오브젝트일경우 가져올 정보
{
    public string prefab; //프리팹 이름
    //public string category;
    public Quaternion rotation;
    public Vector3 scale;
    public string anim;
}






public class SH_BtnManager : MonoBehaviour
{
    public static SH_BtnManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;

    public GameObject inputField;       // inputField 프리팹
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // 현재 선택되어있는 드롭다운과 텍스트 사이즈, 텍스트 컬러
    public Dropdown txtDropdown;
    public InputField InputtxtSize;
    // 씬 추가하기
    public GameObject voidScene;
    public GameObject rawImage;
    public Camera sceneCam;         // 씬 카메라
    public List<RawImage> rawImages = new List<RawImage>();
    public RenderTexture sceneCamRenderTexture;
    string path;                    // 캡쳐 정보 저장 경로
    private int captureWidth;
    private int captureHeight;

    // Object Instantiate를 위한 List
    public GameObject[] obj;
    // 첫 RawImage위치
    public Transform firstRawImage;
    // 씬 오브젝트들을 담을 빈 오브젝트를 담을 리스트
    public List<GameObject> Scenes = new List<GameObject>();
    public List<Vector3> ScenesPos = new List<Vector3>();
    // 씬 텍스트들을 담을 빈 오브젝트들을 담을 리스트(Canvas안에 있는)
    public List<GameObject> Scenes_txt = new List<GameObject>();
    public List<Vector3> Scenes_txtPos = new List<Vector3>();

    public GameObject newScene;
    public GameObject newScene_Canvas;

    // 멀티 페이지당 오브젝트 담을 클래스 리스트
    //public List<PageInfo> objsInfo = new List<PageInfo>();
    //public List<string> objsInfo = new List<string>();
    // 멀티 책의 정보를 담을 클래스 리스트
    public List<PagesInfo> pages = new List<PagesInfo>();

    public BookInfo bookinfo = new BookInfo();

    // 현재 내가 있는 씬 번호
    public int currentSceneNum;

    // 텍스트 컬러 hex Color List
    public List<string> hexColor;

    // 텍스트 컬러 반영된 이미지
    public Image txtcolorImage;


    void Start()
    {
        path = Application.dataPath + "/Capture/";
        captureWidth = /*Screen.width;*/640;
        captureHeight = /*Screen.height;*/360;

        txtDropdown.onValueChanged.AddListener(ChangeTextFont);
        InputtxtSize.onValueChanged.AddListener(ChangeFontSize);
    }

    void Update()
    {
        if (YJ_DataManager.instance.preScene == "BookShelfScene" || YJ_DataManager.instance.preScene == "PreviewScene")
        {
            titlePanel.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name != "EditorScene")
            return;
        if (Input.GetMouseButtonDown(0))
        {
            GoScene();

        }
        currentScene = (int)Scenes[0].transform.position.y / 20;


        if (isTTS == true && ttsSound.clip != null)
        {

        }
    }


    #region 에디터 배경 나오기 버튼 함수들
    void OnCompleteObject()
    {
        print("Object");
    }
    void OnCompleteScene()
    {
        print("Scene");
    }
    void MoveObj(GameObject go, float destination, string completeFun = "", string axis = "")
    {
        Hashtable hash = iTween.Hash(axis, destination,
            "time", 0.5f);

        if (completeFun.Length > 0)
        {
            hash.Add("oncompletetarget", gameObject);
            hash.Add("oncomplete", completeFun);
        }
        iTween.MoveTo(go, hash);
        //"easetype", iTween.EaseType.linear));
    }

    public GameObject Scenebuttonon;
    public GameObject Scenebuttonoff;
    int bgDir = 1;      // 씬 BG가 나타나 있지 않을때(나타나 있을 때는 -1이다)
    public void MoveSceneBG()
    {
        float x = sceneBG.transform.position.x + (sceneBG.GetComponent<RectTransform>().sizeDelta.x - 70) * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene", "x");
        if (bgDir == 1)       // 나타나 있지 않는 상태 -> 나타나는 상태
        {
            SceneBtn.rotation = new Quaternion(0, 0, 180 * -(bgDir), 0);
            // 이때 objDir이 나타나있는 상태라면(objDir = 1)
            // objDir을 돌려준다
            ObjectBtn.rotation = new Quaternion(0, 0, 180, 0);
            //soundBtn.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
        }
        bgDir *= -1;

        MoveObj(objectBG.gameObject, /*Screen.width*/1851, "OnCompleteScene", "x");
        //MoveObj(soundBG.gameObject, -210, "OnCompleteObject", "y");
        objDir = -1;
        soundDir = 1;
    }

    int objDir = -1;
    public void MoveObjectBG()
    {
        float x = objectBG.transform.position.x + (objectBG.GetComponent<RectTransform>().sizeDelta.x - 70) * objDir;
        MoveObj(objectBG.gameObject, x, "OnCompleteObject", "x");
        if (objDir == -1)
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
            //soundBtn.rotation = Quaternion.Euler(0, 0, 90);

        }
        else
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 180 * -objDir, 0);
        }
        objDir *= -1;


        MoveObj(sceneBG.gameObject, 64, "OnCompleteScene", "x");
        //MoveObj(soundBG.gameObject, -210, "OnCompleteObject", "y");
        bgDir = 1;
        soundDir = 1;
    }

    int soundDir = 1;
    //public GameObject soundBG;
    //public RectTransform soundBtn;
    //public void SoundBG()
    //{
    //    float y = soundBG.transform.position.y + 300 * soundDir;
    //    MoveObj(soundBG.gameObject, y, "OnCompleteObject", "y");
    //    if(soundDir == 1)
    //    {
    //        soundBtn.rotation = Quaternion.Euler(0, 0, -90);
    //        ObjectBtn.rotation = new Quaternion(0, 0, 180, 0);
    //        SceneBtn.rotation = new Quaternion(0, 0, 0, 0);

    //    }
    //    else
    //    {
    //        soundBtn.rotation = Quaternion.Euler(0, 0, 90);
    //    }
    //    soundDir *= -1;

    //    MoveObj(sceneBG.gameObject, 50, "OnCompleteScene", "x");
    //    MoveObj(objectBG.gameObject, /*Screen.width*/1865, "OnCompleteScene", "x");
    //    bgDir = 1;
    //    objDir = -1;
    //}
    #endregion


    // 텍스트 추가 함수
    // 텍스트를 추가할 때 처음엔 기본 값으로 세팅한다
    // 드롭다운이 변형될 때마다 폰트를 변경한다
    // 텍스트 사이즈를 변경할 때마다 폰트 크기를 변경한다
    // 변경할 때마다의 값을 각자만의 클래스에 저장해놓는다.
    // 해당 함수는 시작할 때만 값을 할당한다
    // 현재 있는 씬을 기억한다
    int text;
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();

        SH_EditorManager.Instance.active_InputField = inputText;
        inputFields.Add(inputText);
        // 초기값 세팅
        SetInfo(0, 40, Color.black);

        inputText.Initialize(Scenes_txt[currentSceneNum].transform, text, new Vector3(-170, -350, 0));
        inputText.SetInfo(txtDropdown.value, int.Parse(InputtxtSize.text), txtcolorImage.color);

        text++;
    }

    public void SetInfo(int dropdown, int inputTextSize, Color txtColor)
    {
        txtDropdown.value = dropdown;
        InputtxtSize.text = inputTextSize.ToString();
        txtcolorImage.color = txtColor;
    }

    #region 글씨 크기 조절
    public void PlusSize()
    {
        int size = int.Parse(InputtxtSize.text);
        size++;
        InputtxtSize.text = size.ToString();
    }

    public void MinusSize()
    {
        int size = int.Parse(InputtxtSize.text);
        size--;
        InputtxtSize.text = size.ToString();

    }

    public void ChangeFontSize(string size)
    {
        SH_EditorManager.Instance.active_InputField.SetFontSize(int.Parse(size));
    }


    #endregion

    public void ChangeTextColor()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        Color color;
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out color);
        txtcolorImage.color = color;
        SH_EditorManager.Instance.active_InputField.SetFontColor(color);
    }

    void ChangeTextFont(int index)
    {
        SH_EditorManager.Instance.active_InputField.SetFontType(index);
    }

    public GameObject palette;
    public void PaletteOnOff()
    {
        if (palette.activeSelf == true)
        {
            palette.SetActive(false);
        }
        else
        {
            palette.SetActive(true);
        }
    }
    // 씬 추가하기 함수
    // rawImage를 리스트에 담는다
    // Save를 누르거나, 씬을 추가하는 순간 해당 씬을 캡쳐한다.
    // 캡쳐한 후에 rawImage에 해당 이미지를 담고
    // sceneCam에 추가한 RawImage에 있는 RenderTexture을 다시 넣어준다
    // 새로운 rawImage를 추가한다
    // 씬 카메라를 아래로 내린다 --> 변경 : 오브젝트들을 씬 마다의 빈오브젝트를 만들어 그 안에 넣어주고 위로 올려준다

    string fileName;            // 파일 저장 이름
    public int i = 0;
    public int currentScene;
    public List<byte[]> rawImageList = new List<byte[]>();
    public Button ttsBtn;
    public Button recordBtn;

    public void AddScene()
    {
        #region 캡쳐하기
        // 파일이 없다면
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }

        // 캡쳐파일 이름 정하기(동화이름으로 바꾸기)
        fileName = path + "_" + currentScene + ".jpg";

        // 캡쳐하기 
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        sceneCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        sceneCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
        #endregion

        // 현재 선택되어 있는 오브젝트의 버튼을 꺼준다
        if (SH_EditorManager.Instance.activeObj != null)
        {
            List<GameObject> buttons = SH_EditorManager.Instance.activeObj.GetComponent<SH_SceneObj>().buttons;
            for (int k = 0; k < buttons.Count; k++)
            {
                buttons[k].SetActive(false);
            }
        }



        // 해당 y값이 0이면 내가 지금 scene0에 있다는 소리고 
        // 20으로 나눈 몫이 1이면 내가 지금 Scene1에 있다는 소리다
        currentScene = (int)Scenes[0].transform.position.y / 20;

        if (rawImageList.Count == currentSceneNum) rawImageList.Add(bytes);
        else rawImageList[currentSceneNum] = bytes;

        if (bytes.Length > 0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(bytes);
            rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
        }


        // 새로운 Rawimage 추가
        // 맨 밑에 추가해야한다
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + (i + 1);
        rawImages.Add(raw.GetComponent<RawImage>());
        sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;

        // 오브젝트들 올리기(빈 오브젝트가 올라가면 되지 않을까?)
        // 된다 : 빈오브젝트 List를 다 올려야한다
        // 현재 Scene에 들어있는 
        // 카메라 내리지 않기로 결정(Scenecam, MainCamera 모두!)
        for (int j = 0; j < Scenes.Count; j++)
        {
            Scenes[j].transform.position += new Vector3(0, 20 * ((i + 1) - currentScene), 0);
        }
        for (int k = 0; k < Scenes_txt.Count; k++)
        {
            Scenes_txt[k].transform.position += new Vector3(0, Screen.height * ((i + 1) - currentScene), 0);
        }


        // 이제 오브젝트들을 싹 다 올렸으니 새로운 빈 오브젝트들을 만들고
        GameObject n_Scene = Instantiate(newScene);
        // 빈 오브젝트들의 이름도 바꿔야한다!
        n_Scene.name = "Scene" + (i + 1);       // 씬 이름 : Scene0, Scene1, Scene2....
        GameObject n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene_txt" + (i + 1);      // 씬 이름 : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // 빈 오브젝트들의 위치도 설정하자
        n_Scene.transform.position = new Vector3(0, 0, 0);
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position;
        // 이 오브젝트들을 List에 추가해볼까?
        Scenes.Add(n_Scene);
        Scenes_txt.Add(n_Scene_Canvas);

        SH_VoiceRecord.Instance.voiceClip.Add(null);
        // TTS 버튼과 녹음 버튼도 초기화 시켜볼까?
        SH_VoiceRecord.Instance.Reset();
        i++;
        currentSceneNum = i;   // 씬 추가했으므로 새 씬으로 가고 따라서 현재씬을 i값으로 해준다

    }

    // Object 생성 함수
    // 해당 Object를 Scenes List에 담는다
    GameObject assetPath;
    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        string clickText = clickBtn.name.Substring(0, clickBtn.name.Length - 3);
        // 이름에 해당하는 Object를 Instantiate 한다
        for (int j = 0; j < obj.Length; j++)
        {
            if (obj[j].name.Contains(clickText))
            {
                GameObject createObj = Instantiate(obj[j]);
                SH_EditorManager.Instance.activeObj = createObj;
                createObj.transform.SetParent(Scenes[currentSceneNum].transform);
                createObj.transform.position = new Vector3(0, -1, 0);
                break;
            }
        }

    }


    // 스크린을 누르면 
    // 다시 카메라를 RawImage로 바꾼다
    // 오브젝트를 무엇을 눌렀냐에 따라서 내리는 정도를 바꾼다
    // 클릭한 씬을 0으로 내린다
    // 다른 씬들도 같이 내려야한다
    public void GoScene()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        for (int j = 0; j < raycastResults.Count; j++)
        {

            // 씬을 클릭했다는 뜻이므로
            // 해당 씬으로 돌아가야한다
            if (raycastResults[j].gameObject.name.Contains("RawImage") && Scenes[0] != null)
            {
                // 현재 선택되어 있는 오브젝트의 버튼을 꺼준다
                if (SH_EditorManager.Instance.activeObj == null) return;
                List<GameObject> buttons = SH_EditorManager.Instance.activeObj.GetComponent<SH_SceneObj>().buttons;
                for (int k = 0; k < buttons.Count; k++)
                {
                    buttons[k].SetActive(false);
                }

                // 해당 y값이 0이면 내가 지금 scene0에 있다는 소리고 
                // 20으로 나눈 몫이 1이면 내가 지금 Scene1에 있다는 소리다
                int currentScene = (int)Scenes[0].transform.position.y / 20;
                // 원래 있었던 씬을 캡쳐해서 바꿔준다
                // 캡쳐하기 
                // 캡쳐파일 이름 정하기
                fileName = path + "_CurrentScene_" + currentScene + ".jpg";

                // 캡쳐하기 
                RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
                sceneCam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
                Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
                sceneCam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
                screenShot.Apply();

                // 이미지 바이트를 PNG로 변환
                byte[] bytes = screenShot.EncodeToJPG();
                File.WriteAllBytes(fileName, bytes);

                if (rawImageList.Count == currentSceneNum) rawImageList.Add(bytes);
                else rawImageList[currentSceneNum] = bytes;

                // 캡쳐파일 RawImage에 넣기
                // 경로를 통해서 바이트 받아오기
                byte[] textureBytes = File.ReadAllBytes(fileName);
                // 안 봐도됨!
                if (textureBytes.Length > 0)
                {
                    Texture2D loadedTexture = new Texture2D(0, 0);
                    loadedTexture.LoadImage(textureBytes);
                    rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
                }

                // 다시 캡쳐이미지에서 RawImage로 바꾼다
                int sceneNum = int.Parse(raycastResults[j].gameObject.name.Substring(9));
                rawImages[sceneNum].texture = sceneCamRenderTexture;
                sceneCam.targetTexture = rawImages[sceneNum].texture as RenderTexture;

                // 어떤 씬을 눌렀냐에 따라서 내리는 정도를 설정한다(전역변수 i값이 현재 씬의 개수라고 생각하면 된다)
                // (전체 씬 개수 - 클릭한 씬 넘버) * 10을 빼준다(모두다) 빈 오브젝트를 빼주면 된다
                // txt는 (전체 씬 개수 - 클릭한 씬 넘버) * screen.Height를 빼준다
                //(i - sceneNum) * 10
                // 현재 있는 씬이 클릭씬보다 나중에 만들어졌을 경우(번호가 더 크다)
                if (currentScene > sceneNum)
                {

                    for (int k = 0; k < Scenes.Count; k++)
                    {
                        Scenes[k].transform.position -= new Vector3(0, (currentScene - sceneNum) * 20, 0);
                        Scenes_txt[k].transform.position -= new Vector3(0, (currentScene - sceneNum) * Screen.height, 0);
                    }
                }
                // 클릭한 씬보다 먼저 만들어 졌을 경우(번호가 더 작다)
                else
                {

                    for (int k = 0; k < Scenes.Count; k++)
                    {
                        print("sceneNum : " + sceneNum);
                        Scenes[k].transform.position += new Vector3(0, (sceneNum - currentScene) * 20, 0);
                        Scenes_txt[k].transform.position += new Vector3(0, (sceneNum - currentScene) * Screen.height, 0);
                    }
                }


                SH_VoiceRecord.Instance.Change();
                // 클릭한 씬 넘버의 보이스 상태를 불러온다
                // 해당 페이지에 적용시켜준다
                // 만약 녹음 버튼을 활성화 시킨 페이지라면

                // 아무것도 선택하지 않은 상태
                if (SH_VoiceRecord.Instance.voiceClip[sceneNum] == null && SH_VoiceRecord.Instance.voiceInfos[sceneNum].ttsBtn == SH_VoiceRecord.Instance.ttsUnCheked)
                {
                    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
                    SH_VoiceRecord.Instance.ttsBtn.interactable = true;
                    SH_VoiceRecord.Instance.recordBtn.interactable = true;
                }

                else if (SH_VoiceRecord.Instance.voiceInfos[sceneNum].ttsBtn == SH_VoiceRecord.Instance.ttsUnCheked)
                {
                    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsUnCheked;
                    SH_VoiceRecord.Instance.ttsBtn.interactable = false;
                    SH_VoiceRecord.Instance.recordBtn.interactable = true;
                }
                else
                {
                    SH_VoiceRecord.Instance.ttsBtn.interactable = true;
                    SH_VoiceRecord.Instance.ttsBtn.GetComponent<Image>().sprite = SH_VoiceRecord.Instance.ttsChecked;
                    SH_VoiceRecord.Instance.recordBtn.interactable = false;
                }
                SH_VoiceRecord.Instance.num = SH_VoiceRecord.Instance.voiceInfos[sceneNum].recordNum;
                currentSceneNum = sceneNum;            // 현재 씬 넘버를 선택한 씬 넘버로 저장해준다
                break;
            }
        }
    }



    [Header("소리 관련 변수")]
    public Sprite playing;
    public Sprite notPlaying;
    public AudioClip preClip;
    public AudioClip curClip;
    GameObject currentBtn;
    GameObject preBtn;
    public Image nonePlaying;
    string BGClipName;
    // 효과음 리스트
    public List<AudioClip> BGClips;
    // 효과음 오디오소스
    public AudioSource bgSoundSource;
    public AudioSource bgDefaultSource;

    // 선택한 버튼에 대한 소리를 바꾼다
    // 선택한 버튼에 대한 이미지를 Playing으로 바꾼다.
    // 만약 그전에 재생중인 버튼이 있다면
    // 해당의 버튼의 이미지를 notPlaying으로 바꾼다
    // 만약 재생중인 버튼을 다시한번 클릭한 것이라면
    // 재생중인 버튼의 이미지를 notPlaying으로 바꾸고
    // 소리 재생을 멈춘다

    // 1. 재생 중인 소리의 버튼을 멈춘다

    public void SelectSound2()
    {
        // 클릭한 버튼 이름
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // 클릭한 버튼의 이미지
        Image clickBtnImage = clickBtn.GetComponent<Image>();
        // 클릭한버튼에서 Btn 뺀 이름
        string clickText = clickBtn.name.Substring(0, clickBtn.name.Length - 3);

        for (int i = 0; i < BGClips.Count; i++)
        {
            // 배경음악 클립이 있다면
            if (BGClips[i] != null)
            {
                // i번째 BG클립의 이름
                BGClipName = BGClips[i].name;
            }

            // 만약 클릭한 버튼이름과 리스트의 이름이 동일하다면
            if (clickText == BGClipName)
            {
                // 그 때 PreClip이 비어있다면(처음 선택했을 때라면)
                if (preClip == null && curClip == null)
                {
                    preClip = BGClips[i];
                    preBtn = clickBtn;

                    curClip = preClip;
                    currentBtn = preBtn;

                    // 다 채웠으니 이미지를 바꿔볼까
                    currentBtn.GetComponent<Image>().sprite = playing;
                    // 다 채웠으니 재생을 해볼까?
                    bgSoundSource.clip = BGClips[i];
                    bgSoundSource.Play();
                }
                // 비어있지 않다면(그 전에 선택한 전적이 있다.)
                else
                {
                    // 만약 이때 그전 클립과 현재 선택한 클립이 같다면
                    if (curClip == BGClips[i])
                    {
                        // 재생을 멈춘다
                        if (bgSoundSource.isPlaying)
                        {
                            bgSoundSource.Stop();
                            currentBtn.GetComponent<Image>().sprite = notPlaying;
                        }
                        else
                        {
                            bgSoundSource.Play();
                            currentBtn.GetComponent<Image>().sprite = playing;
                        }
                    }

                    // 아니라면 현재클립을 바꿔준다
                    else
                    {
                        //그 전 소리가 멈춰야 한다
                        bgSoundSource.Stop();
                        // 현재 있던 버튼을 옛날 버튼으로 바꾸고
                        preClip = curClip;
                        preBtn = currentBtn;

                        // 현재 버튼과 클립을 다시 업데이트 해준다
                        curClip = BGClips[i];
                        currentBtn = clickBtn;

                        // 현재 버튼과 과거버튼의 이미지를 바꿔볼까?
                        preBtn.GetComponent<Image>().sprite = notPlaying;
                        currentBtn.GetComponent<Image>().sprite = playing;
                        // 다 채웠으니 재생을 해볼까?
                        bgSoundSource.clip = BGClips[i];
                        bgSoundSource.Play();
                    }
                }

            }
        }


    }

    public void SoundNone()
    {
        bgSoundSource.Stop();

        if (preBtn != null)
        {
            preBtn.GetComponent<Image>().sprite = notPlaying;
            preClip = null;
            preBtn = null;
        }

        if (currentBtn != null)
        {
            currentBtn.GetComponent<Image>().sprite = notPlaying;
            curClip = null;
            currentBtn = null;
        }

    }

    public string title;
    public GameObject titlePanel;
    public GameObject titlePopUp;
    public void TitleOk()
    {
        title = titlePopUp.transform.GetChild(2).GetChild(2).GetComponent<Text>().text;
        iTween.ScaleTo(titlePopUp, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));

        titlePanel.SetActive(false);
    }


    // 배경음 적용하기 버튼 클릭 시
    public AudioSource bgSelectSound;
    public void ClickApplySound()
    {
        // 현재 프로젝트의 오디오 소스에 접근
        bgSelectSound.clip = bgSoundSource.clip;
        YJ_DataManager.instance.bgClip = bgSelectSound.clip;
        // UI 밑으로 내리기
        MoveSceneBG();
    }

    private AnimatorClipInfo[] clipInfo;
    public void Save()
    {
        pages.Clear();
        SaveInfo(true);

    }

    byte[] nullbytedata = new byte[0];

    // 제이슨 저장
    // PageInfo -> PagesInfo -> BookInfo -> Json
    private void SaveInfo(bool send)
    {
        SH_VoiceRecord voice = this.gameObject.GetComponent<SH_VoiceRecord>();
        //objsInfo = new List<string>();

        // PageInfo 클래스에서 부터 오브젝트와 텍스트의 정보를 넣어보자
        for (int i = 0; i < Scenes.Count; i++)
        {
            PagesInfo pagesInfo = new PagesInfo();
            List<string> objsInfo = new List<string>();

            pagesInfo.page = i;
            if (Scenes_txt[i].transform.childCount > 0)
            {
                for (int j = 0; j < Scenes_txt[i].transform.childCount; j++)
                {
                    pagesInfo.ttsText += Scenes_txt[i].transform.GetChild(j).GetComponent<InputField>().text;
                    pagesInfo.ttsText += " ";
                }
            }
            else
            {
                pagesInfo.ttsText = "";
            }


            // wav > byte로 변환하기
            if (voice.voiceClip.Count > i && voice.voiceClip[i] != null)
            {
                float[] floatData = new float[voice.voiceClip[i].samples * voice.voiceClip[i].channels];
                voice.voiceClip[i].GetData(floatData, 0);

                // byte 배열 만들기
                byte[] byteData = new byte[floatData.Length * 4];
                Buffer.BlockCopy(floatData, 0, byteData, 0, byteData.Length);

                pagesInfo.voice = byteData;
            }
            else
            {
                pagesInfo.voice = nullbytedata;
            }

            #region 마지막 페이지 캡쳐 및 로우 이미지 배열에 넣기(지금은 꼭 마지막 페이지에서 저장해야함)
            // 로우이미지 세팅
            // 마지막 페이지 캡쳐한다
            // 캡쳐하기 
            // 마지막 페이지로 모든걸 올린다
            if (i == currentSceneNum)
            {
                RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
                sceneCam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
                Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
                sceneCam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
                screenShot.Apply();
                byte[] bytes = screenShot.EncodeToJPG();
                string fileName = Application.dataPath + "/Capture/" + "_" + i + ".jpg";
                File.WriteAllBytes(fileName, bytes);

                // rawImageList에 넣는다
                // 캡쳐파일 RawImage에 넣기
                byte[] textureBytes = File.ReadAllBytes(fileName);

                if (rawImageList.Count == i) rawImageList.Add(bytes);
                else rawImageList[i] = bytes;

                if (textureBytes.Length > 0)
                {
                    Texture2D loadedTexture = new Texture2D(0, 0);
                    loadedTexture.LoadImage(textureBytes);
                    // 현재는 꼭 마지막 씬에서만 저장해야함..
                    rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
                }
            }

            #endregion

            if (rawImageList.Count > i)
            {
                pagesInfo.rawImg = rawImageList[i];
            }
            else
            {
                pagesInfo.rawImg = nullbytedata;
            }

            // 씬 하나
            // 오브젝트 담기(type, prefab, position, rotation, scale 필요함)
            // 그 안에 자식이 있을때만 for문을 돌리자!
            if (Scenes[i].transform.childCount > 0)
            {
                for (int j = 0; j < Scenes[i].transform.childCount; j++)
                {
                    // 현재 씬 넘버에 따라서 y값 조절하자(마지막 씬을 0으로)
                    ObjInfo objInfo = new ObjInfo();
                    SH_SceneObj obj = Scenes[i].transform.GetChild(j).GetComponent<SH_SceneObj>();
                    objInfo.type = obj.objType.ToString();
                    objInfo.prefab = obj.name.Substring(0, obj.name.Length - 7);     //("(clone)" 빼고 저장해야함)
                    objInfo.position = new Vector3(obj.transform.position.x, obj.transform.position.y + (Scenes.Count - 1 - currentSceneNum) * 20, obj.transform.position.z);
                    objInfo.rotation = obj.transform.rotation;
                    objInfo.scale = obj.transform.localScale;
                    objInfo.anim = obj.GetComponent<SH_SceneObj>().currentAnim;
                    // 멀티 오브젝트 클래스 리스트에 담아준다
                    objsInfo.Add(pagesInfo.SerializePageInfo(objInfo));

                }
            }

            if (Scenes_txt[i].transform.childCount > 0)
            {
                // 텍스트 담기
                for (int k = 0; k < Scenes_txt[i].transform.childCount; k++)
                {
                    TxtInfo txtInfo = new TxtInfo();
                    SH_SceneObj txt = Scenes_txt[i].transform.GetChild(k).GetComponent<SH_SceneObj>();
                    SH_InputField txt2 = Scenes_txt[i].transform.GetChild(k).GetComponent<SH_InputField>();
                    txtInfo.type = txt.objType.ToString();
                    txtInfo.position = txt.gameObject.GetComponent<RectTransform>().anchoredPosition;
                    txtInfo.font = SH_EditorManager.Instance.fonts[txt2.info.txtDropdown].name;
                    txtInfo.size = txt2.info.txtSize;
                    txtInfo.content = txt2.transform.GetChild(3).GetComponent<Text>().text;
                    txtInfo.color = ColorUtility.ToHtmlStringRGBA(txt2.transform.GetChild(3).GetComponent<Text>().color);
                    // 멀티 오브젝트 클래스 리스트에 담아준다
                    objsInfo.Add(pagesInfo.SerializePageInfo(txtInfo));
                }
            }

            // 페이지 당 오브젝트를 다 담았으면 data를 할당해준다
            // objsInfo의 List를 초기화해준다
            pagesInfo.data = objsInfo;
            pages.Add(pagesInfo);
            //objsInfo.Clear();
        }
        // 하나의 책에 페이지 당 오브젝트 정보들이 모두 담겼다
        bookinfo.title = title;
        //bookinfo.createAt = DateTime.Now.ToString("yyyy / MM / dd");
        // 이제 BookInfo 중 Pages에 이 정보들을 담아보자
        bookinfo.pages = pages;

        string pageJson = JsonUtility.ToJson(bookinfo, true);
        string path = Application.dataPath + "/" + "in" + ".Json";
        File.WriteAllText(path, pageJson);

        // 미리보기인지 서버로 보내는 json인지 확인
        if (send)
            SaveJson();
    }


    // 제이슨 저장
    private void SaveJson()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale";
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.headers["Content-Type"] = "application/json";
        // 책장씬에서 책 수정 버튼 클릭한 후 다시 저장시킬 때
        if (YJ_DataManager.instance.preScene == "BookShelfScene")
        {
            bookinfo.id = YJ_DataManager.instance.updateBookId;
            requester.requestType = RequestType.PUT;
            print("PUTTT");
        }
        else
        {
            requester.requestType = RequestType.POST;
        }
        // ArrayJson -> json
        string pageJson = JsonUtility.ToJson(bookinfo, true);
        //tring path = Application.dataPath + "/" + "실험" + ".txt";
        string path = Application.dataPath + "/" + "out" + ".Json";
        File.WriteAllText(path, pageJson);
        //print(pageJson);
        requester.postData = pageJson;
        requester.onComplete = (handler) =>
        {
            print("동화책 내용 받아오기 결과 : " + handler.downloadHandler.text);

        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    #region PreviewScene // 동화 미리보기
    public void PreviewScene()
    {
        SaveInfo(false);
    }
    #endregion


    [Serializable]
    public class Test_m
    {
        public string str;
    }

    public void Mp3_Test(string path, string text)
    {
        //byte[] data = File.ReadAllBytes(Application.dataPath + "/Resources/Audio_0.wav");
        //AudioClip audioClip = WAV.ToAudioClip(Application.dataPath + "/Resources/Audio_0.wav");
        //ttsSound.clip = audioClip;

        Test_m test = new Test_m();
        test.str = text;
        // ArrayJson -> json
        string getmp3 = JsonUtility.ToJson(test, true);
        print(getmp3);

        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tts/play";
        requester.requestType = RequestType.POST;
        requester.postData = getmp3;
        requester.onComplete = (handler) =>
        {
            print("mp3파일생성!");
            //print(handler.downloadHandler.text);
            byte[] byteData = handler.downloadHandler.data;

            File.WriteAllBytes(/*Application.streamingAssetsPath + "/" + "ex"*/path + ".mp3", byteData);
            // 빌드파일에서만 오디오 파일 재생
            //#if !UNITY_EDITOR
            ReadAudio();
            //#endif
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    public void ReadAudio()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = Application.dataPath + "/Resources" + "/" + "Audio_" + currentSceneNum + ".mp3";
        requester.requestType = RequestType.AUDIO;
        requester.onComplete = (handler) =>
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(handler);
            //DownloadHandlerTexture.GetContent(handler);
            ttsSound.clip = clip;
            ttsSound.Play();

            StartCoroutine(IESoundLength());
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }



    // 해당 씬에 텍스트가 비어있지 않다면
    // UIManager에 있는 str에 넣어주고
    // 다운 받은 파일을 재생시켜준다
    public AudioSource ttsSound;
    [Header("TTS 관련 이미지")]
    public Sprite ttsPlayImage;
    public Sprite ttsNotPlayImage;
    public Image ttsBtnImg;
    bool isTTS;

    public void RealTTS()
    {
        print(isTTS);
        ttsSound.Stop();
        StopAllCoroutines();

        if (isTTS == false)
        {
            TTS();
            ttsBtnImg.sprite = ttsNotPlayImage;
            isTTS = true;
        }
        else
        {

            ttsBtnImg.sprite = ttsPlayImage;
            isTTS = false;
        }
    }


    // TTS 재생이 끝나면 알아서 ttsSound를 멈추고 이미지도 바꾸고 isTTS도 false로 바꾼다
    IEnumerator IESoundLength()
    {
        yield return new WaitForSeconds(ttsSound.clip.length);
        // 반복문 멈추기
        isTTS = false;
        ttsSound.Stop();
        ttsBtnImg.sprite = ttsPlayImage;
    }

    string ttstext;

    public void TTS()
    {
        string filePath = Application.dataPath + "/Resources" + "/" + "Audio_" + currentSceneNum;
        ttstext = "";
        if (Scenes_txt[currentSceneNum].transform.childCount < 1) return;
        for (int i = 0; i < Scenes_txt[currentSceneNum].transform.childCount; i++)
        {
            ttstext += Scenes_txt[currentSceneNum].transform.GetChild(i).GetComponent<InputField>().text;
            ttstext += "\n";
        }

        Mp3_Test(filePath, ttstext);

    }

    public void BackBtn()
    {
        if (YJ_DataManager.instance.preScene == "BookShelfScene")
        {
            SceneManager.LoadScene("BookShelfScene");
        }
        else
        {
            SceneManager.LoadScene("MyRoomScene");
        }
        YJ_DataManager.instance.preScene = "EditorScene";
    }
}
