using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;



// InputField 필요한 정보들
public class TextInfo
{
    public string inputs { get; set; }
    public int txtDropdown { get; set; }
    public int txtSize { get; set; }
}


public class SH_BtnManager : MonoBehaviour
{
    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;
    Animation sceneAnim;
    Animation objectAnim;
    bool sceneBG_view;
    bool objectBG_view;

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
    string path;                    // 캡쳐 정보 저장 경로
    private int captureWidth;
    private int captureHeight;
        
    // Object Instantiate를 위한 List
    public GameObject[] obj;

    // RawImage에 따른 씬 카메라 위치 리스트
    public List<Vector3> sceneCamPos = new List<Vector3>();
    public List<Vector3> mainCamPos = new List<Vector3>();
    // 첫 RawImage위치
    public Transform firstRawImage;
    // 씬 오브젝트들을 담을 빈 오브젝트를 담을 리스트
    public List<GameObject> Scenes = new List<GameObject>();
    void Start()
    {
        sceneAnim = sceneBG.GetComponent<Animation>();
        objectAnim = objectBG.GetComponent<Animation>();
        path = Application.dataPath + "/Capture/";
        captureWidth = Screen.width;
        captureHeight = Screen.height;
        // 처음 포지션을 추가해준다(SceneCamPos, MaincamPos)
        sceneCamPos.Add(new Vector3(0, 0.46f, -8.2f));
        mainCamPos.Add(Camera.main.transform.position);
    }

    void Update()
    {
        
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
        inputText.transform.SetParent(GameObject.Find("Canvas").transform);
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
    int i = 0;
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
        rawImages.Add(raw.GetComponent<RawImage>());
        sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        
        // 카메라 내리기(Scenecam, MainCamera 모두!)
        // Vector3(0,0.460000008,-8.18999958) 위치 저장 y값으로 -10만큼!
        sceneCamPos.Add(sceneCamPos[i] + new Vector3(0, -10, 0));
        mainCamPos.Add(mainCamPos[i] + new Vector3(0, -10, 0));
        sceneCam.transform.position = sceneCamPos[i + 1];
        Camera.main.transform.position = mainCamPos[i + 1];
                
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

}
