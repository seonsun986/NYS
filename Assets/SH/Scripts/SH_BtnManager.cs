using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using Photon.Pun;



// InputField 필요한 정보들
public class TextInfo
{
    public string inputs { get; set; }
    public int txtDropdown { get; set; }
    public int txtSize { get; set; }
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
    public string id;
    public string title;
    public string createAt;
    public List<PagesInfo> pages;

}

[System.Serializable]
public class PagesInfo
{
    public int page;
    //public List<PageInfo> data;
    public List<string> data;

    public string SerializePageInfo(PageInfo info)
    {
        string pageInfo = JsonUtility.ToJson(info);
        return pageInfo;
    }

    public PageInfo DeserializePageInfo(string s)
    {
        PageInfo pageInfo = JsonUtility.FromJson<PageInfo>(s);
        if(pageInfo.type == "text")
        {
            pageInfo = JsonUtility.FromJson<TxtInfo>(s);
        }
        if(pageInfo.type == "obj")
        {
            pageInfo = JsonUtility.FromJson<ObjInfo>(s);
        }
        return pageInfo;
    }
}

[System.Serializable]
public class PageInfo
{
    public string type;
    public Vector3 position;
}

[System.Serializable]
public class TxtInfo : PageInfo
{
    public string font;
    public int size;
    public string content;
}

[System.Serializable]
public class ObjInfo : PageInfo
{
    public string prefab;
    public Quaternion rotation;
    public Vector3 scale;
}


public class Back : YJ_PlazaManager
{
    public static new Back instance;

    private void Awake()
    {
        instance = this;
    }

    //[HideInInspector]
    //public new UserInfo userInfo;

    // 들어와 있는 인원 파악하기
    public new int liveCount = 0;

    public new Vector3[] spawnPos;

    public override void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        // XR_A라는 방으로 입장
        //PhotonNetwork.JoinRoom("Lobby");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom("Lobby");
    }

    public override string ChangeSceneName()
    {
        sceneName = "PlazaScene";
        return sceneName;
    }
}

//public class PageInfo
//{
//    // txt관련
//    public string type;
//    public string font;
//    public int size;
//    public string content;
//    // obj관련
//    public string prefab;
//    public Vector3 position;    // 공통!
//    public Quaternion rotation;
//    public Vector3 scale;
//}


public class SH_BtnManager : MonoBehaviour
{
    public static SH_BtnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;


    public GameObject inputField;       // inputField 프리팹
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // 현재 선택되어있는 드롭다운과 텍스트 사이즈
    public Dropdown txtDropdown;
    public Text txtSize;

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
    public List<string> objsInfo = new List<string>();
    // 멀티 책의 정보를 담을 클래스 리스트
    public List<PagesInfo> pages = new List<PagesInfo>();



    void Start()
    {
        path = Application.dataPath + "/Capture/";
        captureWidth = Screen.width;
        captureHeight = Screen.height;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GoScene();
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
    void MoveObj(GameObject go, float destination, string completeFun = "")
    {
        Hashtable hash = iTween.Hash("x", destination,
            "time", 0.5f);
            
        if(completeFun.Length > 0)
        {
            hash.Add("oncompletetarget", gameObject);
            hash.Add("oncomplete", completeFun);
        }
        iTween.MoveTo(go, hash);
        //"easetype", iTween.EaseType.linear));
    }

    int bgDir = 1;      // 씬 BG가 나타나 있지 않을때(나타나 있을 때는 -1이다)
    public void MoveSceneBG()
    {
        float x = sceneBG.transform.position.x + sceneBG.GetComponent<RectTransform>().sizeDelta.x * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene");
        if(bgDir== 1)       // 나타나 있지 않는 상태 -> 나타나는 상태
        {
            SceneBtn.rotation = new Quaternion(0, 0, 180 * -(bgDir), 0);
            // 이때 objDir이 나타나있는 상태라면(objDir = 1)
            // objDir을 돌려준다
            ObjectBtn.rotation = new Quaternion(0, 0, 180, 0);
        }
        else
        {
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
        }
        bgDir *= -1;

        MoveObj(objectBG.gameObject, Screen.width);
        objDir = -1;
    }

    int objDir = -1;
    public void MoveObjectBG()
    {
        float x = objectBG.transform.position.x + objectBG.GetComponent<RectTransform>().sizeDelta.x * objDir;
        MoveObj(objectBG.gameObject, x, "OnCompleteObject");
        if(objDir == -1)
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);

        }
        else
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 180 * -objDir, 0);
        }
        objDir *= -1;
       

        MoveObj(sceneBG.gameObject, 0);
        bgDir = 1;
    }
    #endregion


    // 텍스트 추가 함수
    // 텍스트를 추가할 때 처음엔 기본 값으로 세팅한다
    // 드롭다운이 변형될 때마다 폰트를 변경한다
    // 텍스트 사이즈를 변경할 때마다 폰트 크기를 변경한다
    // 변경할 때마다의 값을 각자만의 클래스에 저장해놓는다.
    // 해당 함수는 시작할 때만 값을 할당한다
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();
        inputText.info = new TextInfo
        {
            inputs = inputText.GetComponent<InputField>().text,
            txtDropdown = txtDropdown.value,
            txtSize = int.Parse(txtSize.text),
        };
        // 선택되어있는 dropdown과 textSize값에 따라서 글자 크기를 바꾸기 위함
        inputFields.Add(inputText);
        inputText.transform.SetParent(Scenes_txt[i].transform);
        inputText.transform.localPosition = new Vector3(0, 0, 0);
    }

    #region 글씨 크기 조절
    public void PlusSize()
    {
        int size = int.Parse( txtSize.text);
        size++;
        txtSize.text = size.ToString();
    }

    public void MinusSize()
    {
        int size = int.Parse(txtSize.text);
        size--;
        txtSize.text = size.ToString();
    }
    #endregion

    // 씬 추가하기 함수
    // rawImage를 리스트에 담는다
    // Save를 누르거나, 씬을 추가하는 순간 해당 씬을 캡쳐한다.
    // 캡쳐한 후에 rawImage에 해당 이미지를 담고
    // sceneCam에 추가한 RawImage에 있는 RenderTexture을 다시 넣어준다
    // 새로운 rawImage를 추가한다
    // 씬 카메라를 아래로 내린다 --> 변경 : 오브젝트들을 씬 마다의 빈오브젝트를 만들어 그 안에 넣어주고 위로 올려준다

    string fileName;            // 파일 저장 이름
    public int i = 0;
    public void AddScene()
    {
        #region 캡쳐하기
        // 파일이 없다면
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        
        // 캡쳐파일 이름 정하기
        fileName = path + "_" + i + ".png";
        
        // 캡쳐하기 
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        sceneCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        sceneCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes);
        #endregion


        // 캡쳐파일 RawImage에 넣기
        byte[] textureBytes = File.ReadAllBytes(fileName);
        if(textureBytes.Length>0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);
            rawImages[i].GetComponent<RawImage>().texture = loadedTexture;
        }
        
        // 새로운 Rawimage 추가
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform);
        raw.transform.position = firstRawImage.position + transform.up * (-180* (i+1));
        raw.name = "RawImage_" + (i + 1);
        rawImages.Add(raw.GetComponent<RawImage>());
        sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;

        // 오브젝트들 올리기(빈 오브젝트가 올라가면 되지 않을까?)
        // 된다 : 빈오브젝트 List를 다 올려야한다
        // 현재 Scene에 들어있는 
        // 카메라 내리지 않기로 결정(Scenecam, MainCamera 모두!)
        for(int j =0;j<Scenes.Count;j++)
        {
            Scenes[j].transform.position += new Vector3(0, 20, 0);
        }
        for(int k =0;k<Scenes_txt.Count;k++)
        {
            Scenes_txt[k].transform.position += new Vector3(0, Screen.height, 0);
        }


        // 이제 오브젝트들을 싹 다 올렸으니 새로운 빈 오브젝트들을 만들고
        GameObject n_Scene = Instantiate(newScene);
        // 빈 오브젝트들의 이름도 바꿔야한다!
        n_Scene.name = "Scene" + (i + 1);       // 씬 이름 : Scene0, Scene1, Scene2....
        GameObject n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene" + (i + 1) + "_txt";      // 씬 이름 : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // 빈 오브젝트들의 위치도 설정하자
        n_Scene.transform.position = new Vector3(0, 0, 0);
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position;
        // 이 오브젝트들을 List에 추가해볼까?
        Scenes.Add(n_Scene);
        Scenes_txt.Add(n_Scene_Canvas);

        i++;

    }

    // Object 생성 함수
    // 해당 Object를 Scenes List에 담는다
    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        string clickText = clickBtn.name.Substring(0,clickBtn.name.Length - 3);
        // 이름에 해당하는 Object를 Instantiate 한다
        for(int j =0;j<obj.Length;j++)
        {
            if(obj[j].name.Contains(clickText))
            {
                GameObject createObj = Instantiate(obj[j]);
                createObj.transform.SetParent(Scenes[i].transform);
                createObj.transform.position = new Vector3(0, 0, 0);
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
            // 해당 y값이 0이면 내가 지금 scene0에 있다는 소리고 
            // 20이면 내가 지금 Scene1에 있다는 소리다
            int currentScene = (int)Scenes[0].transform.position.y / 20;
            // 원래 있었던 씬을 캡쳐해서 바꿔준다
            // 캡쳐하기 
            // 캡쳐파일 이름 정하기
            fileName = path + "_CurrentScene_" + currentScene + ".png";

            // 캡쳐하기 
            RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
            sceneCam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
            sceneCam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(fileName, bytes);

            // 캡쳐파일 RawImage에 넣기
            byte[] textureBytes = File.ReadAllBytes(fileName);
            if (textureBytes.Length > 0)
            {
                Texture2D loadedTexture = new Texture2D(0, 0);
                loadedTexture.LoadImage(textureBytes);
                rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
            }

            // 씬을 클릭했다는 뜻이므로
            // 해당 씬으로 돌아가야한다
            if (raycastResults[j].gameObject.name.Contains("RawImage"))
            {
                // 다시 캡쳐이미지에서 RawImage로 바꾼다
                int sceneNum = int.Parse(raycastResults[j].gameObject.name.Substring(9));
                rawImages[sceneNum].texture = sceneCamRenderTexture;
                sceneCam.targetTexture = rawImages[sceneNum].texture as RenderTexture;

                // 어떤 씬을 눌렀냐에 따라서 내리는 정도를 설정한다(전역변수 i값이 현재 씬의 개수라고 생각하면 된다)
                // (전체 씬 개수 - 클릭한 씬 넘버) * 10을 빼준다(모두다) 빈 오브젝트를 빼주면 된다
                // txt는 (전체 씬 개수 - 클릭한 씬 넘버) * screen.Height를 빼준다
                //(i - sceneNum) * 10
                // 현재씬이 선택한 씬보다 나중에 만들어졌을 경우
                if(currentScene > sceneNum)
                {

                    for (int k = 0; k < Scenes.Count; k++)
                    {
                       
                        Scenes[k].transform.position -= new Vector3(0, (i - sceneNum) * 20, 0);
                        Scenes_txt[k].transform.position -= new Vector3(0, (i - sceneNum) * Screen.height, 0);
                    }
                }
                else
                {

                    for (int k = 0; k < Scenes.Count; k++)
                    {
                        Scenes[k].transform.position += new Vector3(0, (i - sceneNum) * 20, 0);
                        Scenes_txt[k].transform.position += new Vector3(0, (i - sceneNum) * Screen.height, 0);
                    }
                }
                
                break;
            }
        }

    }


    // 제이슨 저장
    // PageInfo -> PagesInfo -> BookInfo -> Json
    public void Save()
    {
        BookInfo bookinfo = new BookInfo();
        // PageInfo 클래스에서 부터 오브젝트와 텍스트의 정보를 넣어보자
        for(int i =0;i<Scenes.Count;i++)
        {
            PagesInfo pagesInfo = new PagesInfo();
            objsInfo = new List<string>();
            pagesInfo.page = i;

            // 씬 하나
            // 오브젝트 담기(type, prefab, position, rotation, scale 필요함)
            // 그 안에 자식이 있을때만 for문을 돌리자!
            if(Scenes[i].transform.childCount>0)
            {
                for (int j = 0; j < Scenes[i].transform.childCount; j++)
                {
                    ObjInfo objInfo = new ObjInfo();
                    SH_SceneObj obj = Scenes[i].transform.GetChild(j).GetComponent<SH_SceneObj>();
                    objInfo.type = obj.objType.ToString();
                    objInfo.prefab = obj.name.Substring(0, obj.name.Length - 7);     //("(clone)" 빼고 저장해야함)
                    objInfo.position = obj.transform.position;
                    objInfo.rotation = obj.transform.rotation;
                    objInfo.scale = obj.transform.localScale;
                    // 멀티 오브젝트 클래스 리스트에 담아준다
                    objsInfo.Add(pagesInfo.SerializePageInfo(objInfo));
                }
            }
           
            if(Scenes_txt[i].transform.childCount>0)
            {
                // 텍스트 담기
                for (int k = 0; k < Scenes_txt[i].transform.childCount; k++)
                {
                    TxtInfo txtInfo = new TxtInfo();
                    SH_SceneObj txt = Scenes_txt[i].transform.GetChild(k).GetComponent<SH_SceneObj>();
                    SH_InputField txt2 = Scenes_txt[i].transform.GetChild(k).GetComponent<SH_InputField>();
                    txtInfo.type = txt.objType.ToString();
                    txtInfo.position = txt.gameObject.GetComponent<RectTransform>().anchoredPosition;
                    txtInfo.font = txt2.info.txtDropdown.ToString();       // 아마도 int 값으로 나올거야
                    txtInfo.size = txt2.info.txtSize;
                    txtInfo.content = txt2.transform.GetChild(3).GetComponent<Text>().text;
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
        bookinfo.id = "심선혜 최고";
        bookinfo.title = "위인전 : 심선혜";
        bookinfo.createAt = DateTime.Now.ToString("yyyy - MM - dd");
        // 이제 BookInfo 중 Pages에 이 정보들을 담아보자
        bookinfo.pages = pages;
        string jsonData = JsonUtility.ToJson(bookinfo, true);
        print(jsonData);

        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        File.WriteAllText(path, jsonData);
    }
}
