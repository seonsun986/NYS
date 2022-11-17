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
        // ���� ���� ��������̶��
        if (YJ_DataManager.instance.preScene == "PreviewScene")
        {
            LoadObjects();
            YJ_DataManager.instance.preScene = null;
        }

        // ���� ���� å����̶��
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
        // ���� �� �̸��� ��������̶�� ����
        YJ_DataManager.instance.preScene = "PreviewScene";
        SceneManager.LoadScene("EditorScene");
    }

    #region LoadObjects // �������
    public void LoadObjects()
    {
        // Json ���� �޾ƿ���
        string path = Application.dataPath + "/" + "in" + ".Json";
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
    #endregion

    #region LoadObjects // å������� å ���� �ҷ�����
    public void LoadObjects(string id)
    {
        // �ҷ�����
        print("������ ��ȭå ���� ��.");
        Info title = new Info();
        YJ_HttpRequester requester2 = new YJ_HttpRequester();
        requester2.url = "http://43.201.10.63:8080/tale/" + id;
        requester2.requestType = RequestType.GET;
        requester2.onComplete = (handler) =>
        {
            Debug.Log("�� ��ȭ �¾�? \n" + handler.downloadHandler.text);
            JObject taleJObj = JObject.Parse(handler.downloadHandler.text);
            title = JsonUtility.FromJson<Info>(handler.downloadHandler.text);
            BookInfo bookInfo = title.data;
            List<PagesInfo> pagesInfos = bookInfo.pages;

            // pageinfo(����) ������ text, obj�� �������� Ŭ���� �� json ���� > pagesinfo.data(����Ʈ)
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
            print("�� : " + sceneObjects.Count);
            for (int i = 0; i < sceneObjects.Count; i++)
                InstantiateObject();
        };
        YJ_HttpManager.instance.SendRequest(requester2);
    }
    #endregion

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
        InputField textInfo = textObj.GetComponent<InputField>();
        textInfo.text = txt.content;
        // ��Ʈ ����
        Font fontInfo;
        if (txt.font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(txt.font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(txt.font);
        textInfo.textComponent.font = fontInfo;
        // ���� ����
        UnityEngine.Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + txt.color, out colorInfo);
        textInfo.textComponent.color = colorInfo;
        // ��Ʈ ������ ����
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
        // Scene0 ������Ʈ�� �̹� �����Ƿ�
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


    List<Texture2D> images = new List<Texture2D>();
    public void GetRawImage(string url, int index)
    {
        // å ���� �̹��� �޾ƿ���
        NK_HttpDetailImage requester = new NK_HttpDetailImage();
        requester.url = url;
        requester.requestType = RequestType.IMAGE;
        requester.index = index;
        requester.onCompleteDownloadImage = (handler, idx) =>
        {
            // å ���� �̹��� �ؽ��ķ� �޾ƿ���
            Texture2D texture = DownloadHandlerTexture.GetContent(handler);
            images[idx] = texture;
        };
        YJ_HttpManager.instance.SendRequest(requester);
    }

    private void AddImage(int i)
    {
        // RawImage �ҷ�����
        // ������ ���ÿ� ����� ������ �ҷ��´�
        // ���� ����Ǹ� ������ ����� RawImage �ҷ��� ��
        //string path = Application.dataPath + "/Capture/_CurrentScene_" + pageNum + ".png";
        //byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(0, 0);
        // RawImage0 ������Ʈ�� �̹� �����Ƿ�
        if (i == 0)
        {
            texture2D = images[0];
            SH_BtnManager.Instance.firstRawImage.gameObject.GetComponent<RawImage>().texture = texture2D;
            return;
        }
        // ���ο� Rawimage �߰�
        // �� �ؿ� �߰��ؾ��Ѵ�
        GameObject raw = Instantiate(SH_BtnManager.Instance.rawImage);
        raw.transform.SetParent(GameObject.Find("ContentRaw").transform);
        raw.transform.position = SH_BtnManager.Instance.firstRawImage.position + transform.up * (-180 * (i + 1));
        raw.name = "RawImage_" + i;
        texture2D = images[i];
        raw.GetComponent<RawImage>().texture = texture2D;
        SH_BtnManager.Instance.rawImages.Add(raw.GetComponent<RawImage>());
        // ������ �������� RawImage�� RenderTexture��
        if (i == sceneObjects.Count - 1)
        {
            raw.GetComponent<RawImage>().texture = SH_BtnManager.Instance.sceneCamRenderTexture;
            SH_BtnManager.Instance.sceneCam.targetTexture = raw.GetComponent<RawImage>().texture as RenderTexture;
        }
    }
}
