using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using Photon.Pun;
using UnityEditor;



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
    public string color;
}

[System.Serializable]
public class ObjInfo : PageInfo
{
    public string prefab;
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
        Instance = this;
    }

    public Image sceneBG;
    public Image objectBG;
    public RectTransform SceneBtn;
    public RectTransform ObjectBtn;


    public GameObject inputField;       // inputField ������
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // ���� ���õǾ��ִ� ��Ӵٿ�� �ؽ�Ʈ ������, �ؽ�Ʈ �÷�
    public Dropdown txtDropdown;
    public string txtSize;
    public InputField InputtxtSize;
    public Color txtColor;
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
    public List<string> objsInfo = new List<string>();
    // ��Ƽ å�� ������ ���� Ŭ���� ����Ʈ
    public List<PagesInfo> pages = new List<PagesInfo>();

    // ���� ���� �ִ� �� ��ȣ
    public int currentSceneNum;

    // �ؽ�Ʈ �÷� hex Color List
    public List<string> hexColor;

    // �ؽ�Ʈ �÷� �ݿ��� �̹���
    public Image txtcolorImage;
    void Start()
    {
        path = Application.dataPath + "/Capture/";
        captureWidth = Screen.width;
        captureHeight = Screen.height;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoScene();
        }
        currentScene = (int)Scenes[0].transform.position.y / 20;

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
    void MoveObj(GameObject go, float destination, string completeFun = "", string axis= "")
    {
        Hashtable hash = iTween.Hash(axis, destination,
            "time", 0.5f);
            
        if(completeFun.Length > 0)
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
        float x = sceneBG.transform.position.x + (sceneBG.GetComponent<RectTransform>().sizeDelta.x - 55) * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene", "x");
        if(bgDir== 1)       // ��Ÿ�� ���� �ʴ� ���� -> ��Ÿ���� ����
        {
            SceneBtn.rotation = new Quaternion(0, 0, 180 * -(bgDir), 0);
            // �̶� objDir�� ��Ÿ���ִ� ���¶��(objDir = 1)
            // objDir�� �����ش�
            ObjectBtn.rotation = new Quaternion(0, 0, 180, 0);
            soundBtn.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
        }
        bgDir *= -1;

        MoveObj(objectBG.gameObject, /*Screen.width*/1865, "OnCompleteScene", "x");
        MoveObj(soundBG.gameObject, -210, "OnCompleteObject", "y");
        objDir = -1;
        soundDir = 1;
    }

    int objDir = -1;
    public void MoveObjectBG()
    {
        float x = objectBG.transform.position.x + (objectBG.GetComponent<RectTransform>().sizeDelta.x - 65) * objDir;
        MoveObj(objectBG.gameObject, x, "OnCompleteObject", "x");
        if(objDir == -1)
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
            soundBtn.rotation = Quaternion.Euler(0, 0, 90);

        }
        else
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 180 * -objDir, 0);
        }
        objDir *= -1;
       

        MoveObj(sceneBG.gameObject, 50, "OnCompleteScene","x");
        MoveObj(soundBG.gameObject, -210, "OnCompleteObject", "y");
        bgDir = 1;
        soundDir = 1;
    }

    int soundDir = 1;
    public GameObject soundBG;
    public RectTransform soundBtn;
    public void SoundBG()
    {
        float y = soundBG.transform.position.y + 300 * soundDir;
        MoveObj(soundBG.gameObject, y, "OnCompleteObject", "y");
        if(soundDir == 1)
        {
            soundBtn.rotation = Quaternion.Euler(0, 0, -90);
            ObjectBtn.rotation = new Quaternion(0, 0, 180, 0);
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);

        }
        else
        {
            soundBtn.rotation = Quaternion.Euler(0, 0, 90);
        }
        soundDir *= -1;

        MoveObj(sceneBG.gameObject, 50, "OnCompleteScene", "x");
        MoveObj(objectBG.gameObject, /*Screen.width*/1865, "OnCompleteScene", "x");
        bgDir = 1;
        objDir = -1;
    }
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
        if(text==0)
        {
            SH_EditorManager.Instance.origin_InputField = inputText;
            SH_EditorManager.Instance.active_InputField = inputText;
        }
        else
        {
            SH_EditorManager.Instance.active_InputField = inputText;
        }
        
        inputText.gameObject.name = "Text" + text;
        // �ʱⰪ ����
        txtDropdown.value = 0;
        txtSize = "20";
        InputtxtSize.text = txtSize;
        txtColor = new Color(0, 0, 0, 1);

        inputText.info = new TextInfo
        {
            inputs = inputText.GetComponent<InputField>().text,
            txtDropdown = txtDropdown.value,
            txtSize = int.Parse(txtSize),
            txtColor = txtColor,
        };
        // ���õǾ��ִ� dropdown�� textSize���� ���� ���� ũ�⸦ �ٲٱ� ����
        inputFields.Add(inputText);
        inputText.transform.SetParent(Scenes_txt[currentSceneNum].transform);
        inputText.transform.localPosition = new Vector3(0, 0, 0);
        text++;
    }

    #region �۾� ũ�� ����
    public void PlusSize()
    {
        int size = int.Parse(txtSize);
        size++;
        txtSize = size.ToString();
        InputtxtSize.text = txtSize;
    }

    public void MinusSize()
    {
        int size = int.Parse(txtSize);
        size--;
        txtSize = size.ToString();
        InputtxtSize.text = txtSize;
    }

    
    #endregion

    public void ChangeTextColor()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));               
        Color color;
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out color);
        txtColor = color;
        txtcolorImage.color = color;

    }

    public GameObject palette;
    public void PaletteOnOff()
    {
        if(palette.activeSelf == true)
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
    public void AddScene()
    {
        #region ĸ���ϱ�
        // ������ ���ٸ�
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        
        // ĸ������ �̸� ���ϱ�
        fileName = path + "_" + i + ".png";
        
        // ĸ���ϱ� 
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

        // ���� ���õǾ� �ִ� ������Ʈ�� ��ư�� ���ش�
        if(SH_EditorManager.Instance.activeObj != null)
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

        // ĸ������ RawImage�� �ֱ�
        byte[] textureBytes = File.ReadAllBytes(fileName);
        if(textureBytes.Length>0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);
            rawImages[currentScene].GetComponent<RawImage>().texture = loadedTexture;
        }
        
        // ���ο� Rawimage �߰�
        // �� �ؿ� �߰��ؾ��Ѵ�
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform.GetChild(0).transform);
        raw.transform.position = firstRawImage.position + transform.up * (-180* (i+1));
        raw.name = "RawImage_" + (i + 1);
        rawImages.Add(raw.GetComponent<RawImage>());
        sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;

        // ������Ʈ�� �ø���(�� ������Ʈ�� �ö󰡸� ���� ������?)
        // �ȴ� : �������Ʈ List�� �� �÷����Ѵ�
        // ���� Scene�� ����ִ� 
        // ī�޶� ������ �ʱ�� ����(Scenecam, MainCamera ���!)
        for(int j =0;j<Scenes.Count;j++)
        {
            Scenes[j].transform.position += new Vector3(0, 20 * ((i+1) - currentScene), 0);
        }
        for(int k =0;k<Scenes_txt.Count;k++)
        {
            Scenes_txt[k].transform.position += new Vector3(0, Screen.height * ((i + 1) - currentScene), 0);
        }


        // ���� ������Ʈ���� �� �� �÷����� ���ο� �� ������Ʈ���� �����
        GameObject n_Scene = Instantiate(newScene);
        // �� ������Ʈ���� �̸��� �ٲ���Ѵ�!
        n_Scene.name = "Scene" + (i + 1);       // �� �̸� : Scene0, Scene1, Scene2....
        GameObject n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene_txt" + (i + 1) ;      // �� �̸� : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // �� ������Ʈ���� ��ġ�� ��������
        n_Scene.transform.position = new Vector3(0, 0, 0);
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position;
        // �� ������Ʈ���� List�� �߰��غ���?
        Scenes.Add(n_Scene);
        Scenes_txt.Add(n_Scene_Canvas);

        i++;
        currentSceneNum = i;   // �� �߰������Ƿ� �� ������ ���� ���� ������� i������ ���ش�

    }

    // Object ���� �Լ�
    // �ش� Object�� Scenes List�� ��´�
    GameObject assetPath;
    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        string clickText = clickBtn.name.Substring(0,clickBtn.name.Length - 3);
        // �̸��� �ش��ϴ� Object�� Instantiate �Ѵ�
        for(int j =0;j<obj.Length;j++)
        {
            if(obj[j].name.Contains(clickText))
            {
                GameObject createObj = Instantiate(obj[j]);
                SH_EditorManager.Instance.activeObj = createObj;
                createObj.transform.SetParent(Scenes[currentSceneNum].transform);
                createObj.transform.position = new Vector3(0, 0, 0);
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
            if (raycastResults[j].gameObject.name.Contains("RawImage") && Scenes[0]!=null)
            {
                // ���� ���õǾ� �ִ� ������Ʈ�� ��ư�� ���ش�

                List<GameObject> buttons = SH_EditorManager.Instance.activeObj.GetComponent<SH_SceneObj>().buttons;
                for(int k=0;k<buttons.Count;k++)
                {
                    buttons[k].SetActive(false);
                }

                // �ش� y���� 0�̸� ���� ���� scene0�� �ִٴ� �Ҹ��� 
                // 20���� ���� ���� 1�̸� ���� ���� Scene1�� �ִٴ� �Ҹ���
                int currentScene = (int)Scenes[0].transform.position.y / 20;
                // ���� �־��� ���� ĸ���ؼ� �ٲ��ش�
                // ĸ���ϱ� 
                // ĸ������ �̸� ���ϱ�
                fileName = path + "_CurrentScene_" + currentScene + ".png";

                // ĸ���ϱ� 
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

                // ĸ������ RawImage�� �ֱ�
                byte[] textureBytes = File.ReadAllBytes(fileName);
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
                if(currentScene > sceneNum)
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
                currentSceneNum = sceneNum;            // ���� �� �ѹ��� ������ �� �ѹ��� �������ش�
                break;
            }
        }

    }


    // ���̽� ����
    // PageInfo -> PagesInfo -> BookInfo -> Json
    private AnimatorClipInfo[] clipInfo;
    public void Save()
    {
        BookInfo bookinfo = new BookInfo();
        // PageInfo Ŭ�������� ���� ������Ʈ�� �ؽ�Ʈ�� ������ �־��
        for(int i =0;i<Scenes.Count;i++)
        {
            PagesInfo pagesInfo = new PagesInfo();
            objsInfo = new List<string>();
            pagesInfo.page = i;

            // �� �ϳ�
            // ������Ʈ ���(type, prefab, position, rotation, scale �ʿ���)
            // �� �ȿ� �ڽ��� �������� for���� ������!
            if(Scenes[i].transform.childCount>0)
            {
                for (int j = 0; j < Scenes[i].transform.childCount; j++)
                {
                    ObjInfo objInfo = new ObjInfo();
                    SH_SceneObj obj = Scenes[i].transform.GetChild(j).GetComponent<SH_SceneObj>();
                    objInfo.type = obj.objType.ToString();
                    objInfo.prefab = obj.name.Substring(0, obj.name.Length - 7);     //("(clone)" ���� �����ؾ���)
                    objInfo.position = obj.transform.position;
                    objInfo.rotation = obj.transform.rotation;
                    objInfo.scale = obj.transform.localScale;
                    objInfo.anim = obj.GetComponent<SH_SceneObj>().currentAnim;
                    // ��Ƽ ������Ʈ Ŭ���� ����Ʈ�� ����ش�
                    objsInfo.Add(pagesInfo.SerializePageInfo(objInfo));
                }
            }
           
            if(Scenes_txt[i].transform.childCount>0)
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
        bookinfo.id = "�ɼ��� �ְ�";
        bookinfo.title = "������ : �ɼ���";
        bookinfo.createAt = DateTime.Now.ToString("yyyy / MM / dd");
        // ���� BookInfo �� Pages�� �� �������� ��ƺ���
        bookinfo.pages = pages;
        string jsonData = JsonUtility.ToJson(bookinfo, true);
        print(jsonData);

        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        File.WriteAllText(path, jsonData);
    }
}
