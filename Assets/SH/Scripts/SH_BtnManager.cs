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



// InputField �ʿ��� ������
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
    public string id; //������ �����Ǵ� ���� (����)
    public string memberCode;
    public string title; //å ����
    public string createAt; //���糯¥
    public List<PagesInfo> pages; //�������� ����

}

[System.Serializable]
public class PagesInfo
{
    public int page; //������ ��ȣ
    public List<string> data; //�ؽ�Ʈ ����, ������Ʈ ����
    public string ttsText;
    public byte[] voice;
    public byte[] rawImg;

    public string SerializePageInfo(PageInfo info)
    {
        // Class -> Json
        // �ؽ�Ʈ, ������Ʈ ��Ʈ�� �������� ��ȯ
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
public class PageInfo //�������� Ÿ�԰� ��ġ
{
    public string type;
    public Vector3 position;
}

[System.Serializable]
public class TxtInfo : PageInfo //�ؽ�Ʈ�ϰ�� ������ ����
{
    public string font;
    public int size;
    public string content;
    public string color;
}

[System.Serializable]
public class ObjInfo : PageInfo //������Ʈ�ϰ�� ������ ����
{
    public string prefab; //������ �̸�
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

    public GameObject inputField;       // inputField ������
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // ���� ���õǾ��ִ� ��Ӵٿ�� �ؽ�Ʈ ������, �ؽ�Ʈ �÷�
    public Dropdown txtDropdown;
    public InputField InputtxtSize;
    // �� �߰��ϱ�
    public GameObject voidScene;
    public GameObject rawImage;
    public Camera sceneCam;         // �� ī�޶�
    public List<RawImage> rawImages = new List<RawImage>();
    public RenderTexture sceneCamRenderTexture;
    string path;                    // ĸ�� ���� ���� ���
    private int captureWidth;
    private int captureHeight;

    // Object Instantiate�� ���� List
    public GameObject[] obj;
    // ù RawImage��ġ
    public Transform firstRawImage;
    // �� ������Ʈ���� ���� �� ������Ʈ�� ���� ����Ʈ
    public List<GameObject> Scenes = new List<GameObject>();
    public List<Vector3> ScenesPos = new List<Vector3>();
    // �� �ؽ�Ʈ���� ���� �� ������Ʈ���� ���� ����Ʈ(Canvas�ȿ� �ִ�)
    public List<GameObject> Scenes_txt = new List<GameObject>();
    public List<Vector3> Scenes_txtPos = new List<Vector3>();

    public GameObject newScene;
    public GameObject newScene_Canvas;

    // ��Ƽ �������� ������Ʈ ���� Ŭ���� ����Ʈ
    //public List<PageInfo> objsInfo = new List<PageInfo>();
    //public List<string> objsInfo = new List<string>();
    // ��Ƽ å�� ������ ���� Ŭ���� ����Ʈ
    public List<PagesInfo> pages = new List<PagesInfo>();

    public BookInfo bookinfo = new BookInfo();

    // ���� ���� �ִ� �� ��ȣ
    public int currentSceneNum;

    // �ؽ�Ʈ �÷� hex Color List
    public List<string> hexColor;

    // �ؽ�Ʈ �÷� �ݿ��� �̹���
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


    #region ������ ��� ������ ��ư �Լ���
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
    int bgDir = 1;      // �� BG�� ��Ÿ�� ���� ������(��Ÿ�� ���� ���� -1�̴�)
    public void MoveSceneBG()
    {
        float x = sceneBG.transform.position.x + (sceneBG.GetComponent<RectTransform>().sizeDelta.x - 70) * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene", "x");
        if (bgDir == 1)       // ��Ÿ�� ���� �ʴ� ���� -> ��Ÿ���� ����
        {
            SceneBtn.rotation = new Quaternion(0, 0, 180 * -(bgDir), 0);
            // �̶� objDir�� ��Ÿ���ִ� ���¶��(objDir = 1)
            // objDir�� �����ش�
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


    // �ؽ�Ʈ �߰� �Լ�
    // �ؽ�Ʈ�� �߰��� �� ó���� �⺻ ������ �����Ѵ�
    // ��Ӵٿ��� ������ ������ ��Ʈ�� �����Ѵ�
    // �ؽ�Ʈ ����� ������ ������ ��Ʈ ũ�⸦ �����Ѵ�
    // ������ �������� ���� ���ڸ��� Ŭ������ �����س��´�.
    // �ش� �Լ��� ������ ���� ���� �Ҵ��Ѵ�
    // ���� �ִ� ���� ����Ѵ�
    int text;
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();

        SH_EditorManager.Instance.active_InputField = inputText;
        inputFields.Add(inputText);
        // �ʱⰪ ����
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

    #region �۾� ũ�� ����
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
    // �� �߰��ϱ� �Լ�
    // rawImage�� ����Ʈ�� ��´�
    // Save�� �����ų�, ���� �߰��ϴ� ���� �ش� ���� ĸ���Ѵ�.
    // ĸ���� �Ŀ� rawImage�� �ش� �̹����� ���
    // sceneCam�� �߰��� RawImage�� �ִ� RenderTexture�� �ٽ� �־��ش�
    // ���ο� rawImage�� �߰��Ѵ�
    // �� ī�޶� �Ʒ��� ������ --> ���� : ������Ʈ���� �� ������ �������Ʈ�� ����� �� �ȿ� �־��ְ� ���� �÷��ش�

    string fileName;            // ���� ���� �̸�
    public int i = 0;
    public int currentScene;
    public List<byte[]> rawImageList = new List<byte[]>();
    public Button ttsBtn;
    public Button recordBtn;

    public void AddScene()
    {
        #region ĸ���ϱ�
        // ������ ���ٸ�
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }

        // ĸ������ �̸� ���ϱ�(��ȭ�̸����� �ٲٱ�)
        fileName = path + "_" + currentScene + ".jpg";

        // ĸ���ϱ� 
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

        // ���� ���õǾ� �ִ� ������Ʈ�� ��ư�� ���ش�
        if (SH_EditorManager.Instance.activeObj != null)
        {
            List<GameObject> buttons = SH_EditorManager.Instance.activeObj.GetComponent<SH_SceneObj>().buttons;
            for (int k = 0; k < buttons.Count; k++)
            {
                buttons[k].SetActive(false);
            }
        }



        // �ش� y���� 0�̸� ���� ���� scene0�� �ִٴ� �Ҹ��� 
        // 20���� ���� ���� 1�̸� ���� ���� Scene1�� �ִٴ� �Ҹ���
        currentScene = (int)Scenes[0].transform.position.y / 20;

        if (rawImageList.Count == currentSceneNum) rawImageList.Add(bytes);
        else rawImageList[currentSceneNum] = bytes;

        if (bytes.Length > 0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(bytes);
            rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
        }


        // ���ο� Rawimage �߰�
        // �� �ؿ� �߰��ؾ��Ѵ�
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + (i + 1);
        rawImages.Add(raw.GetComponent<RawImage>());
        sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;

        // ������Ʈ�� �ø���(�� ������Ʈ�� �ö󰡸� ���� ������?)
        // �ȴ� : �������Ʈ List�� �� �÷����Ѵ�
        // ���� Scene�� ����ִ� 
        // ī�޶� ������ �ʱ�� ����(Scenecam, MainCamera ���!)
        for (int j = 0; j < Scenes.Count; j++)
        {
            Scenes[j].transform.position += new Vector3(0, 20 * ((i + 1) - currentScene), 0);
        }
        for (int k = 0; k < Scenes_txt.Count; k++)
        {
            Scenes_txt[k].transform.position += new Vector3(0, Screen.height * ((i + 1) - currentScene), 0);
        }


        // ���� ������Ʈ���� �� �� �÷����� ���ο� �� ������Ʈ���� �����
        GameObject n_Scene = Instantiate(newScene);
        // �� ������Ʈ���� �̸��� �ٲ���Ѵ�!
        n_Scene.name = "Scene" + (i + 1);       // �� �̸� : Scene0, Scene1, Scene2....
        GameObject n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene_txt" + (i + 1);      // �� �̸� : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // �� ������Ʈ���� ��ġ�� ��������
        n_Scene.transform.position = new Vector3(0, 0, 0);
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position;
        // �� ������Ʈ���� List�� �߰��غ���?
        Scenes.Add(n_Scene);
        Scenes_txt.Add(n_Scene_Canvas);

        SH_VoiceRecord.Instance.voiceClip.Add(null);
        // TTS ��ư�� ���� ��ư�� �ʱ�ȭ ���Ѻ���?
        SH_VoiceRecord.Instance.Reset();
        i++;
        currentSceneNum = i;   // �� �߰������Ƿ� �� ������ ���� ���� ������� i������ ���ش�

    }

    // Object ���� �Լ�
    // �ش� Object�� Scenes List�� ��´�
    GameObject assetPath;
    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        string clickText = clickBtn.name.Substring(0, clickBtn.name.Length - 3);
        // �̸��� �ش��ϴ� Object�� Instantiate �Ѵ�
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


    // ��ũ���� ������ 
    // �ٽ� ī�޶� RawImage�� �ٲ۴�
    // ������Ʈ�� ������ �����Ŀ� ���� ������ ������ �ٲ۴�
    // Ŭ���� ���� 0���� ������
    // �ٸ� ���鵵 ���� �������Ѵ�
    public void GoScene()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        for (int j = 0; j < raycastResults.Count; j++)
        {

            // ���� Ŭ���ߴٴ� ���̹Ƿ�
            // �ش� ������ ���ư����Ѵ�
            if (raycastResults[j].gameObject.name.Contains("RawImage") && Scenes[0] != null)
            {
                // ���� ���õǾ� �ִ� ������Ʈ�� ��ư�� ���ش�
                if (SH_EditorManager.Instance.activeObj == null) return;
                List<GameObject> buttons = SH_EditorManager.Instance.activeObj.GetComponent<SH_SceneObj>().buttons;
                for (int k = 0; k < buttons.Count; k++)
                {
                    buttons[k].SetActive(false);
                }

                // �ش� y���� 0�̸� ���� ���� scene0�� �ִٴ� �Ҹ��� 
                // 20���� ���� ���� 1�̸� ���� ���� Scene1�� �ִٴ� �Ҹ���
                int currentScene = (int)Scenes[0].transform.position.y / 20;
                // ���� �־��� ���� ĸ���ؼ� �ٲ��ش�
                // ĸ���ϱ� 
                // ĸ������ �̸� ���ϱ�
                fileName = path + "_CurrentScene_" + currentScene + ".jpg";

                // ĸ���ϱ� 
                RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
                sceneCam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
                Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
                sceneCam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
                screenShot.Apply();

                // �̹��� ����Ʈ�� PNG�� ��ȯ
                byte[] bytes = screenShot.EncodeToJPG();
                File.WriteAllBytes(fileName, bytes);

                if (rawImageList.Count == currentSceneNum) rawImageList.Add(bytes);
                else rawImageList[currentSceneNum] = bytes;

                // ĸ������ RawImage�� �ֱ�
                // ��θ� ���ؼ� ����Ʈ �޾ƿ���
                byte[] textureBytes = File.ReadAllBytes(fileName);
                // �� ������!
                if (textureBytes.Length > 0)
                {
                    Texture2D loadedTexture = new Texture2D(0, 0);
                    loadedTexture.LoadImage(textureBytes);
                    rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
                }

                // �ٽ� ĸ���̹������� RawImage�� �ٲ۴�
                int sceneNum = int.Parse(raycastResults[j].gameObject.name.Substring(9));
                rawImages[sceneNum].texture = sceneCamRenderTexture;
                sceneCam.targetTexture = rawImages[sceneNum].texture as RenderTexture;

                // � ���� �����Ŀ� ���� ������ ������ �����Ѵ�(�������� i���� ���� ���� ������� �����ϸ� �ȴ�)
                // (��ü �� ���� - Ŭ���� �� �ѹ�) * 10�� ���ش�(��δ�) �� ������Ʈ�� ���ָ� �ȴ�
                // txt�� (��ü �� ���� - Ŭ���� �� �ѹ�) * screen.Height�� ���ش�
                //(i - sceneNum) * 10
                // ���� �ִ� ���� Ŭ�������� ���߿� ��������� ���(��ȣ�� �� ũ��)
                if (currentScene > sceneNum)
                {

                    for (int k = 0; k < Scenes.Count; k++)
                    {
                        Scenes[k].transform.position -= new Vector3(0, (currentScene - sceneNum) * 20, 0);
                        Scenes_txt[k].transform.position -= new Vector3(0, (currentScene - sceneNum) * Screen.height, 0);
                    }
                }
                // Ŭ���� ������ ���� ����� ���� ���(��ȣ�� �� �۴�)
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
                // Ŭ���� �� �ѹ��� ���̽� ���¸� �ҷ��´�
                // �ش� �������� ��������ش�
                // ���� ���� ��ư�� Ȱ��ȭ ��Ų ���������

                // �ƹ��͵� �������� ���� ����
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
                currentSceneNum = sceneNum;            // ���� �� �ѹ��� ������ �� �ѹ��� �������ش�
                break;
            }
        }
    }



    [Header("�Ҹ� ���� ����")]
    public Sprite playing;
    public Sprite notPlaying;
    public AudioClip preClip;
    public AudioClip curClip;
    GameObject currentBtn;
    GameObject preBtn;
    public Image nonePlaying;
    string BGClipName;
    // ȿ���� ����Ʈ
    public List<AudioClip> BGClips;
    // ȿ���� ������ҽ�
    public AudioSource bgSoundSource;
    public AudioSource bgDefaultSource;

    // ������ ��ư�� ���� �Ҹ��� �ٲ۴�
    // ������ ��ư�� ���� �̹����� Playing���� �ٲ۴�.
    // ���� ������ ������� ��ư�� �ִٸ�
    // �ش��� ��ư�� �̹����� notPlaying���� �ٲ۴�
    // ���� ������� ��ư�� �ٽ��ѹ� Ŭ���� ���̶��
    // ������� ��ư�� �̹����� notPlaying���� �ٲٰ�
    // �Ҹ� ����� �����

    // 1. ��� ���� �Ҹ��� ��ư�� �����

    public void SelectSound2()
    {
        // Ŭ���� ��ư �̸�
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // Ŭ���� ��ư�� �̹���
        Image clickBtnImage = clickBtn.GetComponent<Image>();
        // Ŭ���ѹ�ư���� Btn �� �̸�
        string clickText = clickBtn.name.Substring(0, clickBtn.name.Length - 3);

        for (int i = 0; i < BGClips.Count; i++)
        {
            // ������� Ŭ���� �ִٸ�
            if (BGClips[i] != null)
            {
                // i��° BGŬ���� �̸�
                BGClipName = BGClips[i].name;
            }

            // ���� Ŭ���� ��ư�̸��� ����Ʈ�� �̸��� �����ϴٸ�
            if (clickText == BGClipName)
            {
                // �� �� PreClip�� ����ִٸ�(ó�� �������� �����)
                if (preClip == null && curClip == null)
                {
                    preClip = BGClips[i];
                    preBtn = clickBtn;

                    curClip = preClip;
                    currentBtn = preBtn;

                    // �� ä������ �̹����� �ٲ㺼��
                    currentBtn.GetComponent<Image>().sprite = playing;
                    // �� ä������ ����� �غ���?
                    bgSoundSource.clip = BGClips[i];
                    bgSoundSource.Play();
                }
                // ������� �ʴٸ�(�� ���� ������ ������ �ִ�.)
                else
                {
                    // ���� �̶� ���� Ŭ���� ���� ������ Ŭ���� ���ٸ�
                    if (curClip == BGClips[i])
                    {
                        // ����� �����
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

                    // �ƴ϶�� ����Ŭ���� �ٲ��ش�
                    else
                    {
                        //�� �� �Ҹ��� ����� �Ѵ�
                        bgSoundSource.Stop();
                        // ���� �ִ� ��ư�� ���� ��ư���� �ٲٰ�
                        preClip = curClip;
                        preBtn = currentBtn;

                        // ���� ��ư�� Ŭ���� �ٽ� ������Ʈ ���ش�
                        curClip = BGClips[i];
                        currentBtn = clickBtn;

                        // ���� ��ư�� ���Ź�ư�� �̹����� �ٲ㺼��?
                        preBtn.GetComponent<Image>().sprite = notPlaying;
                        currentBtn.GetComponent<Image>().sprite = playing;
                        // �� ä������ ����� �غ���?
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


    // ����� �����ϱ� ��ư Ŭ�� ��
    public AudioSource bgSelectSound;
    public void ClickApplySound()
    {
        // ���� ������Ʈ�� ����� �ҽ��� ����
        bgSelectSound.clip = bgSoundSource.clip;
        YJ_DataManager.instance.bgClip = bgSelectSound.clip;
        // UI ������ ������
        MoveSceneBG();
    }

    private AnimatorClipInfo[] clipInfo;
    public void Save()
    {
        pages.Clear();
        SaveInfo(true);

    }

    byte[] nullbytedata = new byte[0];

    // ���̽� ����
    // PageInfo -> PagesInfo -> BookInfo -> Json
    private void SaveInfo(bool send)
    {
        SH_VoiceRecord voice = this.gameObject.GetComponent<SH_VoiceRecord>();
        //objsInfo = new List<string>();

        // PageInfo Ŭ�������� ���� ������Ʈ�� �ؽ�Ʈ�� ������ �־��
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


            // wav > byte�� ��ȯ�ϱ�
            if (voice.voiceClip.Count > i && voice.voiceClip[i] != null)
            {
                float[] floatData = new float[voice.voiceClip[i].samples * voice.voiceClip[i].channels];
                voice.voiceClip[i].GetData(floatData, 0);

                // byte �迭 �����
                byte[] byteData = new byte[floatData.Length * 4];
                Buffer.BlockCopy(floatData, 0, byteData, 0, byteData.Length);

                pagesInfo.voice = byteData;
            }
            else
            {
                pagesInfo.voice = nullbytedata;
            }

            #region ������ ������ ĸ�� �� �ο� �̹��� �迭�� �ֱ�(������ �� ������ ���������� �����ؾ���)
            // �ο��̹��� ����
            // ������ ������ ĸ���Ѵ�
            // ĸ���ϱ� 
            // ������ �������� ���� �ø���
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

                // rawImageList�� �ִ´�
                // ĸ������ RawImage�� �ֱ�
                byte[] textureBytes = File.ReadAllBytes(fileName);

                if (rawImageList.Count == i) rawImageList.Add(bytes);
                else rawImageList[i] = bytes;

                if (textureBytes.Length > 0)
                {
                    Texture2D loadedTexture = new Texture2D(0, 0);
                    loadedTexture.LoadImage(textureBytes);
                    // ����� �� ������ �������� �����ؾ���..
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

            // �� �ϳ�
            // ������Ʈ ���(type, prefab, position, rotation, scale �ʿ���)
            // �� �ȿ� �ڽ��� �������� for���� ������!
            if (Scenes[i].transform.childCount > 0)
            {
                for (int j = 0; j < Scenes[i].transform.childCount; j++)
                {
                    // ���� �� �ѹ��� ���� y�� ��������(������ ���� 0����)
                    ObjInfo objInfo = new ObjInfo();
                    SH_SceneObj obj = Scenes[i].transform.GetChild(j).GetComponent<SH_SceneObj>();
                    objInfo.type = obj.objType.ToString();
                    objInfo.prefab = obj.name.Substring(0, obj.name.Length - 7);     //("(clone)" ���� �����ؾ���)
                    objInfo.position = new Vector3(obj.transform.position.x, obj.transform.position.y + (Scenes.Count - 1 - currentSceneNum) * 20, obj.transform.position.z);
                    objInfo.rotation = obj.transform.rotation;
                    objInfo.scale = obj.transform.localScale;
                    objInfo.anim = obj.GetComponent<SH_SceneObj>().currentAnim;
                    // ��Ƽ ������Ʈ Ŭ���� ����Ʈ�� ����ش�
                    objsInfo.Add(pagesInfo.SerializePageInfo(objInfo));

                }
            }

            if (Scenes_txt[i].transform.childCount > 0)
            {
                // �ؽ�Ʈ ���
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
                    // ��Ƽ ������Ʈ Ŭ���� ����Ʈ�� ����ش�
                    objsInfo.Add(pagesInfo.SerializePageInfo(txtInfo));
                }
            }

            // ������ �� ������Ʈ�� �� ������� data�� �Ҵ����ش�
            // objsInfo�� List�� �ʱ�ȭ���ش�
            pagesInfo.data = objsInfo;
            pages.Add(pagesInfo);
            //objsInfo.Clear();
        }
        // �ϳ��� å�� ������ �� ������Ʈ �������� ��� ����
        bookinfo.title = title;
        //bookinfo.createAt = DateTime.Now.ToString("yyyy / MM / dd");
        // ���� BookInfo �� Pages�� �� �������� ��ƺ���
        bookinfo.pages = pages;

        string pageJson = JsonUtility.ToJson(bookinfo, true);
        string path = Application.dataPath + "/" + "in" + ".Json";
        File.WriteAllText(path, pageJson);

        // �̸��������� ������ ������ json���� Ȯ��
        if (send)
            SaveJson();
    }


    // ���̽� ����
    private void SaveJson()
    {
        YJ_HttpRequester requester = new YJ_HttpRequester();
        requester.url = "http://43.201.10.63:8080/tale";
        requester.headers = new Dictionary<string, string>();
        requester.headers["accesstoken"] = YJ_DataManager.instance.myInfo.accessToken;
        requester.headers["Content-Type"] = "application/json";
        // å������� å ���� ��ư Ŭ���� �� �ٽ� �����ų ��
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
        //tring path = Application.dataPath + "/" + "����" + ".txt";
        string path = Application.dataPath + "/" + "out" + ".Json";
        File.WriteAllText(path, pageJson);
        //print(pageJson);
        requester.postData = pageJson;
        requester.onComplete = (handler) =>
        {
            print("��ȭå ���� �޾ƿ��� ��� : " + handler.downloadHandler.text);

        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    #region PreviewScene // ��ȭ �̸�����
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
            print("mp3���ϻ���!");
            //print(handler.downloadHandler.text);
            byte[] byteData = handler.downloadHandler.data;

            File.WriteAllBytes(/*Application.streamingAssetsPath + "/" + "ex"*/path + ".mp3", byteData);
            // �������Ͽ����� ����� ���� ���
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



    // �ش� ���� �ؽ�Ʈ�� ������� �ʴٸ�
    // UIManager�� �ִ� str�� �־��ְ�
    // �ٿ� ���� ������ ��������ش�
    public AudioSource ttsSound;
    [Header("TTS ���� �̹���")]
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


    // TTS ����� ������ �˾Ƽ� ttsSound�� ���߰� �̹����� �ٲٰ� isTTS�� false�� �ٲ۴�
    IEnumerator IESoundLength()
    {
        yield return new WaitForSeconds(ttsSound.clip.length);
        // �ݺ��� ���߱�
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
