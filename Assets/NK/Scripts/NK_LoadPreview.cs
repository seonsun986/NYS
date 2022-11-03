using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NK_LoadPreview : MonoBehaviour
{
    public GameObject textFactory;
    public Transform fairyTaleUI;
    public Transform fairyTaleObject;

    Animator animator;
    List<PageInfo> objs;

    Dictionary<int, List<PageInfo>> sceneObjects = new Dictionary<int, List<PageInfo>>();

    private void Start()
    {
        if(YJ_DataManager.instance.preScene == "PreviewScene")
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
        // Json 파일 받아오기
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // 파싱
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;

        // pageinfo(단일) 내에서 text, obj로 구분지어 클래스 내 json 정렬 > pagesinfo.data(리스트)
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

        InstantiateObject();
    }

    public int pageNum;

    public void InstantiateObject()
    {
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
        GameObject textObj = Instantiate(textFactory);
        Text textInfo = textObj.GetComponent<Text>();
        textInfo.text = txt.content;
        // 폰트 적용
        Font fontInfo;
        if (txt.font.Contains("Arial"))
            fontInfo = Resources.GetBuiltinResource<Font>(txt.font + ".ttf");
        else
            fontInfo = (Font)Resources.Load(txt.font);
        textInfo.font = fontInfo;
        // 색깔 적용
        UnityEngine.Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + txt.color, out colorInfo);
        textInfo.color = colorInfo;
        // 폰트 사이즈 변경
        textInfo.fontSize = txt.size;
        textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(textInfo.preferredWidth, textInfo.preferredHeight);
        textObj.transform.SetParent(fairyTaleUI);
        textObj.GetComponent<RectTransform>().anchoredPosition = txt.position;
    }

    private void CreateObject(ObjInfo obj)
    {
        GameObject objPrefab = (GameObject)Instantiate(Resources.Load(obj.prefab));
        objPrefab.transform.SetParent(fairyTaleObject);
        objPrefab.transform.localScale = obj.scale;
        // 애니메이션이 있다면
        if (obj.anim != "")
        {
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
}
