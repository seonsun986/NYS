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
    }

    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);
        // 해당 FailBtn과 관련된 애니메이션 재생하자!
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

    // 클릭한 버튼의 이름을 넣어주는 것
    public string boxText;
    public void FillEmptyBox()
    {
        GameObject SelectBtn = EventSystem.current.currentSelectedGameObject;
        boxText = SelectBtn.transform.GetChild(0).GetComponent<Text>().text;
    }
}
