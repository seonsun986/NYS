using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public GameObject bookUI;
    public GameObject fairyTaleManager;
    public List<PageInfo> objs;
    public GameObject textFactory;
    public Transform fairyTaleUI;
    public Transform fairyTaleObject;

    Dictionary<int, List<PageInfo>> sceneObjects = new Dictionary<int, List<PageInfo>>();

    void SelectBook(Book book)
    {
        selectedBook = book;
        print(book);
    }

    public void ClickBook1()
    {
        SelectBook(Book.백설공주);
        photonView.RPC("RPCSetActive", RpcTarget.All);
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
        bookUI.SetActive(false);
        fairyTaleManager.SetActive(true);
    }

    [PunRPC]
    private void RPCSetActive()
    {
        bookUI.SetActive(false);
        fairyTaleManager.SetActive(true);
    }

    public void ClickBook()
    {
        // Json 파일 받아오기
        string fileName = "Book1";
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // 파싱
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;
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
        if (sceneObjects.Count > 0)
            InstantiateObject();
    }

    public int pageNum;

    public void InstantiateObject()
    {
        List<PageInfo> objs = sceneObjects[pageNum];

        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].type == "text")
            {
                TxtInfo txt = (TxtInfo)objs[i];
                GameObject textObj = PhotonNetwork.Instantiate("NK/" + textFactory.name, Vector3.zero, Quaternion.identity);
                photonView.RPC("RPCCreateText", RpcTarget.All, textObj.GetPhotonView().ViewID, txt.content, txt.size, txt.position);
            }
            if (objs[i].type == "obj")
            {
                ObjInfo obj = (ObjInfo)objs[i];
                GameObject objPrefab = PhotonNetwork.Instantiate(obj.prefab, obj.position - new Vector3(0, 20, 0) * (sceneObjects.Count - (pageNum + 1)), obj.rotation);
                photonView.RPC("RPCCreateObject", RpcTarget.All, objPrefab.GetPhotonView().ViewID, obj.scale);
            }
        }
    }

    IEnumerator ScaleUp(Transform tr, Vector3 scale)
    {
        yield return null;
        tr.localScale = scale;
    }

    void DestroyObject()
    {
        Transform[] texts = fairyTaleUI.GetComponentsInChildren<Transform>();
        Transform[] objects = fairyTaleObject.GetComponentsInChildren<Transform>();

        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] != fairyTaleUI.transform)
                    Destroy(texts[i].gameObject);
            }
        }

        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != fairyTaleObject.transform)
                    Destroy(objects[i].gameObject);
            }
        }
    }

    [PunRPC]
    private void RPCCreateText(int viewId, string content, int size, Vector3 position)
    {
        PhotonView view = PhotonView.Find(viewId);
        GameObject textObj = view.gameObject;
        Text textInfo = textObj.GetComponent<Text>();
        textInfo.text = content;
        /*if(txt.font != "0")
            textInfo.font = new Font(txt.font);*/
        textInfo.fontSize = size;
        textObj.transform.SetParent(fairyTaleUI);
        textObj.transform.localPosition = position;
    }

    [PunRPC]
    private void RPCCreateObject(int viewId, Vector3 scale)
    {
        PhotonView view = PhotonView.Find(viewId);
        GameObject objPrefab = view.gameObject;
        objPrefab.transform.SetParent(fairyTaleObject);
        objPrefab.transform.localScale = scale;
        //StartCoroutine(ScaleUp(objPrefab.transform, scale));
    }

    public void ClickNext()
    {
        if (sceneObjects.Count > pageNum + 1)
        {
            pageNum++;
            DestroyObject();
            InstantiateObject();
        }
    }
    
    public void ClickBefore()
    {
        if (0 <= pageNum - 1)
        {
            pageNum--;
            DestroyObject();
            InstantiateObject();
        }
    }
}
