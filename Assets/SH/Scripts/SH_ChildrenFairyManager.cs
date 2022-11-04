using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SH_ChildrenFairyManager : MonoBehaviour
{
    // 동화책 페이지를 담아 둘 리스트
    public List<GameObject> pages = new List<GameObject>();
    // 선택지에 따라 바뀔 텍스트 리스트
    public List<Text> boxTexts = new List<Text>();
    // 선택했을 때 나타날 오브젝트들의 리스트
    public List<GameObject> selectObject;
    [Header("선택지가 있는 페이지")]
    // 선택 페이지 리스트(이때는 넥스트 버튼이 선택하면 넘어가면 안됨)
    public List<int> SelectPage = new List<int>();
    public GameObject PassPopUp;
    public GameObject FailPopUp;
    public GameObject selectPopUp;
    public int currentPage;

    bool pass;


    void Start()
    {
        
    }

    void Update()
    {
        // 선택지가 정해졌다면 박스 채우기 
        // 한번만 채우개 하자
        if(boxText != "")
        {
            for(int i =0;i< boxTexts.Count;i++)
            {
                boxTexts[i].text = boxText;
            }
        }
    }


    public void NextPage()
    {
        // 현재 페이지가 마지막 페이지일때는 리턴시킨다
        if (currentPage == pages.Count -1) return;

        // 퀴즈 페이지나 선택 페이지면 반드시 선택을 해야지 넘어가게 한다
        for(int i =0; i<SelectPage.Count;i++)
        {
            // 선택 페이지일 때 
            if(currentPage == SelectPage[i])
            {
                if(pass != true)
                {
                    // 아직 선택을 하지 않았다면
                    selectPopUp.SetActive(true);
                    return;
                }             
            }
        }
        // 만약 패스 버튼이었다면 팝업을 꺼준다
        GameObject PassPopUp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        if (PassPopUp.name.Contains("Pass"))
        {
            PassPopUp.SetActive(false);
        }
        pages[currentPage].SetActive(false);
        pages[currentPage + 1].SetActive(true);

        // 나온 오브젝트를 꺼준다
        if (selectObj != null) selectObj.SetActive(false);

        currentPage += 1;
        pass = false;
    }

    public void PrePage()
    {
        // 현재 페이지가 처음 페이지일때는 리턴시킨다
        if (currentPage == 0) return;
        pages[currentPage].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        currentPage -= 1;
    }

    public void OnPassPopUp()
    {
        PassPopUp.SetActive(true);
        pass = true;

        GameObject selectBtn = EventSystem.current.currentSelectedGameObject;
        string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length - 3);
        selectBtn.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < selectObject.Count; i++)
        {
            if (selectBtnName == selectObject[i].name)
            {
                selectObj = selectObject[i];
                if (selectBtnName == "Rabbit") selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.024f, 0.24f);
                selectObj.SetActive(true);
            }
        }

    }

    GameObject selectObj;
    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);
        // 해당 FailBtn과 관련된 애니메이션 재생하자!
        // 그러러면 일단 내가 선택한 버튼이 뭔지 알아야한다 
        GameObject selectBtn = EventSystem.current.currentSelectedGameObject;
        string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length -3);
        // 버튼을 모두 끈다
        selectBtn.transform.parent.gameObject.SetActive(false);
        for(int i=0;i<selectObject.Count;i++)
        {
            if(selectBtnName == selectObject[i].name)
            {
                selectObj = selectObject[i];
                if(selectBtnName == "Bear") selectObj.transform.GetChild(1).localPosition = new Vector3(0, 2.22f, 2);
                else if(selectBtnName == "Tiger") selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.68f, 1.81f);
                selectObj.transform.GetChild(1).GetComponent<Rigidbody>().velocity = Vector3.zero;
                selectObj.SetActive(true);
            }
        }
    }

    public void PassTrue()
    {
        pass = true;
    }

    public void PopUpClose()
    {
        GameObject closeBtn = EventSystem.current.currentSelectedGameObject;
        closeBtn.transform.parent.gameObject.SetActive(false);


    }

    public void TryAgain()
    {
        GameObject closeBtn = EventSystem.current.currentSelectedGameObject;
        closeBtn.transform.parent.gameObject.SetActive(false);

        // 다시 현재 페이지를 켜준다
        pages[currentPage].SetActive(true);
        // 나온 오브젝트를 꺼준다
        selectObj.SetActive(false);
        selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // 당근 중력 꺼주기..
    }

    // 클릭한 버튼의 이름을 넣어주는 것
    public string boxText;
    public void FillEmptyBox()
    {
        GameObject SelectBtn = EventSystem.current.currentSelectedGameObject;
        boxText = SelectBtn.transform.GetChild(0).GetComponent<Text>().text;
    }
}
