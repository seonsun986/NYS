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
        // Json 파일 받아오기
        string fileName = "in";
        string path = Application.dataPath + "/" + fileName + ".Json";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        // 파싱
        BookInfo bookInfo = JsonUtility.FromJson<BookInfo>(jsonData);
        List<PagesInfo> pagesInfos = bookInfo.pages;
        SetBook(pagesInfos);
        // 첫 페이지의 오브젝트를 띄움
        InstantiateObject();
    }

    public override void Update()
    {
        
    }
}
