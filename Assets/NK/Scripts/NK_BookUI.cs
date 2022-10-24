using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NK_BookUI : MonoBehaviourPun
{
    public enum Book
    {
        백설공주,
        신데렐라,
        오즈의마법사,
        용이야기,
    }

    public Book selectedBook = Book.백설공주;
    public GameObject fairyTaleManager;
    public List<PageInfo> objs;
    public GameObject textFactory;
    public Transform fairyTaleUI;
    public Transform fairyTaleObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SelectBook(Book book)
    {
        selectedBook = book;
        print(book);
    }

    public void ClickBook1()
    {
        SelectBook(Book.백설공주);
        ClickBook();
        gameObject.SetActive(false);
        fairyTaleManager.SetActive(true);
    }

    public void ClickBook2()
    {
        SelectBook(Book.신데렐라);
    }

    public void ClickBook3()
    {
        SelectBook(Book.오즈의마법사);
    }
    public void ClickBook4()
    {
        SelectBook(Book.용이야기);
        gameObject.SetActive(false);
        fairyTaleManager.SetActive(true);
    }

    public void ClickBook()
    {
        objs = new List<PageInfo>();

        // 메모장에 저장된 json 파일 불러오기
        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // 파싱
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;
        foreach(PagesInfo pagesInfo in pagesInfos)
        {
            foreach(string pageInfo in pagesInfo.data)
            {
                print(pageInfo);
                objs.Add(pagesInfo.DeserializePageInfo(pageInfo));
                InstantiateObject();
            }
        }
    }

    public void InstantiateObject()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].type == "text")
            {
                TxtInfo txt = (TxtInfo)objs[i];
                GameObject textObj = PhotonNetwork.Instantiate("NK/" + textFactory.name, Vector3.zero, Quaternion.identity);
                Text textInfo = textObj.GetComponent<Text>();
                textInfo.text = txt.content;
                if(txt.font != "0")
                    textInfo.font = new Font(txt.font);
                textInfo.fontSize = txt.size;
                textObj.transform.SetParent(fairyTaleUI);
                textObj.transform.localPosition = txt.position;
            }
            if(objs[i].type == "obj")
            {
                ObjInfo obj = (ObjInfo)objs[i];
                GameObject objPrefab = PhotonNetwork.Instantiate(obj.prefab, Vector3.zero, Quaternion.identity);
                objPrefab.transform.SetParent(fairyTaleObject);
                objPrefab.transform.position = obj.position;
                objPrefab.transform.rotation = obj.rotation;
                objPrefab.transform.localScale = obj.scale;
            }
        }
    }
}
