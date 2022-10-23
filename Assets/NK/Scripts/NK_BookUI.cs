using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NK_BookUI : MonoBehaviour
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
                GameObject textObj = Instantiate(textFactory);
                Text textInfo = textObj.GetComponent<Text>();
                textInfo.text = txt.content;
                textObj.transform.position = txt.position;
                //textInfo.font = txt.font;
                textInfo.fontSize = txt.size;
            }
            if(objs[i].type == "obj")
            {
                ObjInfo obj = (ObjInfo)objs[i];
                /*GameObject objPrefab = Instantiate(obj.prefab);
                objPrefab.transform.position = obj.position;
                objPrefab.transform.rotation = obj.rotation;
                objPrefab.transform.localScale = obj.scale;*/
            }
        }
    }
}
