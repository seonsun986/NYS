using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NK_BookUI : MonoBehaviour
{
    public enum Book
    {
        �鼳����,
        �ŵ�����,
        �����Ǹ�����,
        ���̾߱�,
    }

    public Book selectedBook = Book.�鼳����;
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
        SelectBook(Book.�鼳����);
        ClickBook();
    }

    public void ClickBook2()
    {
        SelectBook(Book.�ŵ�����);
    }

    public void ClickBook3()
    {
        SelectBook(Book.�����Ǹ�����);
    }
    public void ClickBook4()
    {
        SelectBook(Book.���̾߱�);
        gameObject.SetActive(false);
        fairyTaleManager.SetActive(true);
    }

    public void ClickBook()
    {
        objs = new List<PageInfo>();

        // �޸��忡 ����� json ���� �ҷ�����
        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // �Ľ�
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
