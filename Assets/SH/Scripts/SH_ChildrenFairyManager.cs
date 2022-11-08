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
    public GameObject bookWorld;            // 책 골랐을 때 나타나는 월드 오브젝트

    void Start()
    {
        
    }

    void Update()
    {     

        if (bookWorldOpen == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime > bookOpenTime)
            {
                bookWorld.SetActive(true);
                currentTime = 0;
                bookWorldOpen = false;
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
                else
                {
                    if(PassPopUp.activeSelf == true)
                    {
                        PassPopUp.SetActive(false);
                    }
                }

            }
        }

        // 만약 패스 버튼이었다면 팝업을 꺼준다
        GameObject PassPopUpj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        if (PassPopUpj.name.Contains("Pass"))
        {
            PassPopUpj.SetActive(false);
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
    // 책을 골랐을 경우 책이 열리고 나서 고래가 떴으면 좋겠다!
    float currentTime;
    public float bookOpenTime;
    bool bookWorldOpen;
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
                if (selectBtnName == "Bear") selectObj.transform.GetChild(1).localPosition = new Vector3(0, 2.22f, 2);
                else if (selectBtnName == "Tiger") selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.68f, 1.81f);
                else if(selectBtnName == "Book")
                {
                    bookWorldOpen = true;
                }
                if(selectObj.transform.childCount > 1)
                {
                    if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
                    {
                        selectObj.transform.GetChild(1).GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
                
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
        if (selectObj.transform.childCount > 1)
        {
            if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
            {
                selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // 당근 중력 꺼주기..
            }
        }       

        if(bookWorld.activeSelf == true)
        {
            bookWorld.SetActive(false);
        }
    }

    public GameObject selectPages;
    public void FillEmptyBox()
    {
        string GoName = EventSystem.current.currentSelectedGameObject.name;
        string selectBtnName = GoName.Substring(0, GoName.Length - 3);
        selectPages = GameObject.Find(selectBtnName + "Pages");
        
        // 선택한 페이지에 따라서 Page들을 Pages에 추가해준다
        for(int i =0;i<selectPages.transform.childCount;i++)
        {
            pages.Insert(i + 10, selectPages.transform.GetChild(i).gameObject);
        }
        // NextPage 함수 그냥 여기서 실행함
        pages[currentPage].SetActive(false);
        pages[currentPage + 1].SetActive(true);
        currentPage += 1;
    }

    public void TryNo()
    {
        pages[currentPage].SetActive(false);
        pages[currentPage + 2].SetActive(true);
        currentPage += 2;
    }
}
