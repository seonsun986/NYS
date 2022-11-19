using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NK_Preview : NK_BookUI
{
    // Start is called before the first frame update
    void Start()
    {
        // Json ���� �޾ƿ���
        string fileName = "in";
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // �Ľ�
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;
        SetBook(pagesInfos);
        // ù �������� ������Ʈ�� ���
        InstantiateObject();
    }

    public override void Update()
    {
        
    }
}
