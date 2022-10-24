using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using Photon.Pun;



// InputField �ʿ��� ������
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

    // ���� �ִ� �ο� �ľ��ϱ�
    public new int liveCount = 0;

    public new Vector3[] spawnPos;

    public override void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        // XR_A��� ������ ����
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
//    // txt����
//    public string type;
//    public string font;
//    public int size;
//    public string content;
//    // obj����
//    public string prefab;
//    public Vector3 position;    // ����!
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


    public GameObject inputField;       // inputField ������
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // ���� ���õǾ��ִ� ��Ӵٿ�� �ؽ�Ʈ ������
    public Dropdown txtDropdown;
    public Text txtSize;

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


    #region ������ ��� ������ ��ư �Լ���
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

    int bgDir = 1;      // �� BG�� ��Ÿ�� ���� ������(��Ÿ�� ���� ���� -1�̴�)
    public void MoveSceneBG()
    {
        float x = sceneBG.transform.position.x + sceneBG.GetComponent<RectTransform>().sizeDelta.x * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene");
        if(bgDir== 1)       // ��Ÿ�� ���� �ʴ� ���� -> ��Ÿ���� ����
        {
            SceneBtn.rotation = new Quaternion(0, 0, 180 * -(bgDir), 0);
            // �̶� objDir�� ��Ÿ���ִ� ���¶��(objDir = 1)
            // objDir�� �����ش�
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


    // �ؽ�Ʈ �߰� �Լ�
    // �ؽ�Ʈ�� �߰��� �� ó���� �⺻ ������ �����Ѵ�
    // ��Ӵٿ��� ������ ������ ��Ʈ�� �����Ѵ�
    // �ؽ�Ʈ ����� ������ ������ ��Ʈ ũ�⸦ �����Ѵ�
    // ������ �������� ���� ���ڸ��� Ŭ������ �����س��´�.
    // �ش� �Լ��� ������ ���� ���� �Ҵ��Ѵ�
    public void AddText()
    {
        SH_InputField inputText = Instantiate(inputField).GetComponent<SH_InputField>();
        inputText.info = new TextInfo
        {
            inputs = inputText.GetComponent<InputField>().text,
            txtDropdown = txtDropdown.value,
            txtSize = int.Parse(txtSize.text),
        };
        // ���õǾ��ִ� dropdown�� textSize���� ���� ���� ũ�⸦ �ٲٱ� ����
        inputFields.Add(inputText);
        inputText.transform.SetParent(Scenes_txt[i].transform);
        inputText.transform.localPosition = new Vector3(0, 0, 0);
    }

    #region �۾� ũ�� ����
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

    // �� �߰��ϱ� �Լ�
    // rawImage�� ����Ʈ�� ��´�
    // Save�� �����ų�, ���� �߰��ϴ� ���� �ش� ���� ĸ���Ѵ�.
    // ĸ���� �Ŀ� rawImage�� �ش� �̹����� ���
    // sceneCam�� �߰��� RawImage�� �ִ� RenderTexture�� �ٽ� �־��ش�
    // ���ο� rawImage�� �߰��Ѵ�
    // �� ī�޶� �Ʒ��� ������ --> ���� : ������Ʈ���� �� ������ �������Ʈ�� ����� �� �ȿ� �־��ְ� ���� �÷��ش�

    string fileName;            // ���� ���� �̸�
    public int i = 0;
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


        // ĸ������ RawImage�� �ֱ�
        byte[] textureBytes = File.ReadAllBytes(fileName);
        if(textureBytes.Length>0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);
            rawImages[i].GetComponent<RawImage>().texture = loadedTexture;
        }
        
        // ���ο� Rawimage �߰�
        GameObject raw = Instantiate(rawImage);
        raw.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform);
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
            Scenes[j].transform.position += new Vector3(0, 20, 0);
        }
        for(int k =0;k<Scenes_txt.Count;k++)
        {
            Scenes_txt[k].transform.position += new Vector3(0, Screen.height, 0);
        }


        // ���� ������Ʈ���� �� �� �÷����� ���ο� �� ������Ʈ���� �����
        GameObject n_Scene = Instantiate(newScene);
        // �� ������Ʈ���� �̸��� �ٲ���Ѵ�!
        n_Scene.name = "Scene" + (i + 1);       // �� �̸� : Scene0, Scene1, Scene2....
        GameObject n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene" + (i + 1) + "_txt";      // �� �̸� : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // �� ������Ʈ���� ��ġ�� ��������
        n_Scene.transform.position = new Vector3(0, 0, 0);
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position;
        // �� ������Ʈ���� List�� �߰��غ���?
        Scenes.Add(n_Scene);
        Scenes_txt.Add(n_Scene_Canvas);

        i++;

    }

    // Object ���� �Լ�
    // �ش� Object�� Scenes List�� ��´�
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
                createObj.transform.SetParent(Scenes[i].transform);
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
            // �ش� y���� 0�̸� ���� ���� scene0�� �ִٴ� �Ҹ��� 
            // 20�̸� ���� ���� Scene1�� �ִٴ� �Ҹ���
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

            // ���� Ŭ���ߴٴ� ���̹Ƿ�
            // �ش� ������ ���ư����Ѵ�
            if (raycastResults[j].gameObject.name.Contains("RawImage"))
            {
                // �ٽ� ĸ���̹������� RawImage�� �ٲ۴�
                int sceneNum = int.Parse(raycastResults[j].gameObject.name.Substring(9));
                rawImages[sceneNum].texture = sceneCamRenderTexture;
                sceneCam.targetTexture = rawImages[sceneNum].texture as RenderTexture;

                // � ���� �����Ŀ� ���� ������ ������ �����Ѵ�(�������� i���� ���� ���� ������� �����ϸ� �ȴ�)
                // (��ü �� ���� - Ŭ���� �� �ѹ�) * 10�� ���ش�(��δ�) �� ������Ʈ�� ���ָ� �ȴ�
                // txt�� (��ü �� ���� - Ŭ���� �� �ѹ�) * screen.Height�� ���ش�
                //(i - sceneNum) * 10
                // ������� ������ ������ ���߿� ��������� ���
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


    // ���̽� ����
    // PageInfo -> PagesInfo -> BookInfo -> Json
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
                    txtInfo.font = txt2.info.txtDropdown.ToString();       // �Ƹ��� int ������ ���ðž�
                    txtInfo.size = txt2.info.txtSize;
                    txtInfo.content = txt2.transform.GetChild(3).GetComponent<Text>().text;
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
        bookinfo.createAt = DateTime.Now.ToString("yyyy - MM - dd");
        // ���� BookInfo �� Pages�� �� �������� ��ƺ���
        bookinfo.pages = pages;
        string jsonData = JsonUtility.ToJson(bookinfo, true);
        print(jsonData);

        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        File.WriteAllText(path, jsonData);
    }
}
