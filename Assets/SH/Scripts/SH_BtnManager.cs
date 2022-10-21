using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;



// InputField �ʿ��� ������
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
    string path;                    // ĸ�� ���� ���� ���
    private int captureWidth;
    private int captureHeight;
        
    // Object �߰��� ���� List
    public GameObject[] obj;

    // RawImage�� ���� �� ī�޶� ��ġ ����Ʈ
    public List<Vector3> sceneCamPos = new List<Vector3>();
    public List<Vector3> mainCamPos = new List<Vector3>();
    // ù RawImage��ġ
    public Transform firstRawImage;
    void Start()
    {
        sceneAnim = sceneBG.GetComponent<Animation>();
        objectAnim = objectBG.GetComponent<Animation>();
        path = Application.dataPath + "/Capture/";
        captureWidth = Screen.width;
        captureHeight = Screen.height;
        // ó�� �������� �߰����ش�(SceneCamPos, MaincamPos)
        sceneCamPos.Add(new Vector3(0, 0.46f, -8.2f));
        mainCamPos.Add(Camera.main.transform.position);
    }

    void Update()
    {
        
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

    int bgDir = 1;
    public void MoveSceneBG()
    {
        float x = sceneBG.transform.position.x + sceneBG.GetComponent<RectTransform>().sizeDelta.x * bgDir;
        MoveObj(sceneBG.gameObject, x, "OnCompleteScene");
        SceneBtn.rotation = new Quaternion(0, 180 * -(bgDir),0, 0);
        bgDir *= -1;

        MoveObj(objectBG.gameObject, Screen.width);
        objDir = -1;
        return;
        // sceneBG�� ������ �ʴ´ٸ�       
        if(sceneBG_view == false)
        {
            sceneAnim.Play("SceneBGShowAnim");            
            SceneBtn.rotation = new Quaternion(0, 0, -180,0);       // ��ư �����ֱ�
            sceneBG_view = true;
            // �̶� objectBG�� ���δٸ�
            if(objectBG_view == true)
            {
                objectAnim.Play("ObjectBGShowOffAnim");
                ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);
                objectBG_view = false;
            }
        }
        // �׷��� �ʴٸ�
        else
        {
            sceneAnim.Play("SceneBGShowOffAnim");
            SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
            sceneBG_view = false;
        }
    }

    int objDir = -1;
    public void MoveObjectBG()
    {
        float x = objectBG.transform.position.x + objectBG.GetComponent<RectTransform>().sizeDelta.x * objDir;
        MoveObj(objectBG.gameObject, x, "OnCompleteObject");
        if(objDir == -1)
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            ObjectBtn.rotation = new Quaternion(0, 0, 180 * -objDir, 0);
        }
        objDir *= -1;
       

        MoveObj(sceneBG.gameObject, 0);
        bgDir = 1;
        return;

        // objectBG�� ������ �ʴ´ٸ�
        if (objectBG_view == false)
        {
            objectAnim.Play("ObjectBGShowAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, 0, 0);       // ��ư �����ֱ�
            objectBG_view = true;
            // �̶� SceneBG�� ���δٸ�
            if(sceneBG_view == true)
            {
                sceneAnim.Play("SceneBGShowOffAnim");
                SceneBtn.rotation = new Quaternion(0, 0, 0, 0);
                sceneBG_view = false;
            }
        }

        //�׷��� �ʴٸ�
        else
        {
            objectAnim.Play("ObjectBGShowOffAnim");
            ObjectBtn.rotation = new Quaternion(0, 0, -180, 0);    
            objectBG_view = false;
        }
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
        inputText.transform.SetParent(GameObject.Find("Canvas").transform);
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
    // ���ο� rawImage�� �߰��Ѵ�
    // �� ī�޶� �Ʒ��� ������

    string fileName;            // ���� ���� �̸�
    int i = 0;
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
        raw.transform.position = firstRawImage.position + transform.up * (-150* (i+1));
        rawImages.Add(raw.GetComponent<RawImage>());
        
        // ī�޶� ������(Scenecam, MainCamera ���!)
        // Vector3(0,0.460000008,-8.18999958) ��ġ ���� y������ -10��ŭ!
        sceneCamPos.Add(sceneCamPos[i] + new Vector3(0, -10, 0));
        mainCamPos.Add(mainCamPos[i] + new Vector3(0, -10, 0));
        sceneCam.transform.position = sceneCamPos[i + 1];
        Camera.main.transform.position = mainCamPos[i + 1];
                
        i++;

    }

    // Object ���� �Լ�
    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        string clickText = clickBtn.name.Substring(0,clickBtn.name.Length - 3);
        // �̸��� �ش��ϴ� Object�� Instantiate �Ѵ�
        for(int i =0;i<obj.Length;i++)
        {
            if(obj[i].name.Contains(clickText))
            {
                GameObject createObj = Instantiate(obj[i]);
                createObj.transform.position = new Vector3(0, 0, 0);
                break;
            }
        }
    }

}
