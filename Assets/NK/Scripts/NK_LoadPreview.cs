using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (YJ_DataManager.instance.preScene == "PreviewScene")
        {
            LoadObjects("PreviewBook");
            YJ_DataManager.instance.preScene = null;
        }
    }

    public void LoadPreview()
    {
        SceneManager.LoadScene("PreviewScene");
    }

    public void LoadEditor()
    {
        YJ_DataManager.instance.preScene = "PreviewScene";
        SceneManager.LoadScene("EditorScene");
    }

    public void LoadObjects(string fileName)
    {
        // Json ���� �޾ƿ���
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // �Ľ�
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;

        // pageinfo(����) ������ text, obj�� �������� Ŭ���� �� json ���� > pagesinfo.data(����Ʈ)
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

    public int pageNum;

    public void InstantiateObject()
    {
        // pageNum�� ���� �� ������Ʈ ����Ʈ�� ����
        List<PageInfo> objs = sceneObjects[pageNum];
        SH_BtnManager.Instance.currentSceneNum = pageNum;
        AddImage(pageNum);
        AddScene(pageNum);
        for (int i = 0; i < objs.Count; i++)
        {
            // ���������� �ؽ�Ʈ ����
            if (objs[i].type == "text")
            {
                TxtInfo txt = (TxtInfo)objs[i];
                CreateText(txt);
            }
            // ���������� ������Ʈ ����
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
        Text textInfo = textObj.transform.GetChild(2).GetComponent<Text>();
        textInfo.text = txt.content;
        // ��Ʈ ����
        Font fontInfo;
        if (txt.font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(txt.font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(txt.font);
        textInfo.font = fontInfo;
        // ���� ����
        UnityEngine.Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + txt.color, out colorInfo);
        textInfo.color = colorInfo;
        // ��Ʈ ������ ����
        textInfo.fontSize = txt.size;
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
        // �ִϸ��̼��� �ִٸ�
        if (obj.anim != "")
        {
            // �ִϸ����͸� ������
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
        // �ִϸ��̼� �÷���
        yield return null;
        animator.Play(anim);
    }

    private void AddScene(int i)
    {
        if (i == 0)
        {
            n_Scene_Canvas = SH_BtnManager.Instance.Scenes_txt[0];
            n_Scene = SH_BtnManager.Instance.Scenes[0];
            // �� ������Ʈ���� ��ġ�� ��������
            n_Scene.transform.position = new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
            n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
            pageNum++;
            return;
        }
        // ���� ������Ʈ���� �� �� �÷����� ���ο� �� ������Ʈ���� �����
        n_Scene = Instantiate(newScene);
        // �� ������Ʈ���� �̸��� �ٲ���Ѵ�!
        n_Scene.name = "Scene" + (i);       // �� �̸� : Scene0, Scene1, Scene2....
        n_Scene_Canvas = Instantiate(newScene_Canvas);
        n_Scene_Canvas.name = "Scene_txt" + (i);      // �� �̸� : Scene0_txt, Scene1_txt....
        n_Scene_Canvas.transform.SetParent(GameObject.Find("Canvas").transform);
        // �� ������Ʈ���� ��ġ�� ��������
        n_Scene.transform.position = new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
        n_Scene_Canvas.transform.position = GameObject.Find("Canvas").transform.position + new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1));
        // �� ������Ʈ���� List�� �߰��غ���?
        SH_BtnManager.Instance.Scenes.Add(n_Scene);
        SH_BtnManager.Instance.Scenes_txt.Add(n_Scene_Canvas);
        pageNum++;
    }

    private void AddImage(int i)
    {
        string path = Application.dataPath + "/Capture/_CurrentScene_" + pageNum + ".png";
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(0, 0);
        if (i == 0)
        {
            texture2D.LoadImage(byteTexture);
            SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture = texture2D;
            return;
        }
        // ���ο� Rawimage �߰�
        // �� �ؿ� �߰��ؾ��Ѵ�
        GameObject raw = Instantiate(SH_BtnManager.Instance.rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = SH_BtnManager.Instance.firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + i;
        texture2D.LoadImage(byteTexture);
        raw.GetComponent<RawImage>().texture = texture2D;
        SH_BtnManager.Instance.rawImages.Add(raw.GetComponent<RawImage>());
        if (i == sceneObjects.Count - 1)
        {
            raw.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
            SH_BtnManager.Instance.sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        }
    }
}
