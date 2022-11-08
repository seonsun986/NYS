using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SH_ChildrenFairyManager : MonoBehaviour
{
    // ��ȭå �������� ��� �� ����Ʈ
    public List<GameObject> pages = new List<GameObject>();
    // �������� �� ��Ÿ�� ������Ʈ���� ����Ʈ
    public List<GameObject> selectObject;
    [Header("�������� �ִ� ������")]
    // ���� ������ ����Ʈ(�̶��� �ؽ�Ʈ ��ư�� �����ϸ� �Ѿ�� �ȵ�)
    public List<int> SelectPage = new List<int>();
    public GameObject PassPopUp;
    public GameObject FailPopUp;
    public GameObject selectPopUp;
    public int currentPage;

    bool pass;
    public GameObject bookWorld;            // å ����� �� ��Ÿ���� ���� ������Ʈ

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
        // ���� �������� ������ �������϶��� ���Ͻ�Ų��
        if (currentPage == pages.Count -1) return;

        // ���� �������� ���� �������� �ݵ�� ������ �ؾ��� �Ѿ�� �Ѵ�
        for(int i =0; i<SelectPage.Count;i++)
        {
            // ���� �������� �� 
            if(currentPage == SelectPage[i])
            {
                if(pass != true)
                {
                    // ���� ������ ���� �ʾҴٸ�
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

        // ���� �н� ��ư�̾��ٸ� �˾��� ���ش�
        GameObject PassPopUpj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        if (PassPopUpj.name.Contains("Pass"))
        {
            PassPopUpj.SetActive(false);
        }

        
        pages[currentPage].SetActive(false);
        pages[currentPage + 1].SetActive(true);

        // ���� ������Ʈ�� ���ش�
        if (selectObj != null) selectObj.SetActive(false);

        currentPage += 1;
        pass = false;
    }

    public void PrePage()
    {
        // ���� �������� ó�� �������϶��� ���Ͻ�Ų��
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
    // å�� ����� ��� å�� ������ ���� ���� ������ ���ڴ�!
    float currentTime;
    public float bookOpenTime;
    bool bookWorldOpen;
    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);
        // �ش� FailBtn�� ���õ� �ִϸ��̼� �������!
        // �׷����� �ϴ� ���� ������ ��ư�� ���� �˾ƾ��Ѵ� 
        GameObject selectBtn = EventSystem.current.currentSelectedGameObject;
        string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length -3);
        // ��ư�� ��� ����
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

        // �ٽ� ���� �������� ���ش�
        pages[currentPage].SetActive(true);
        // ���� ������Ʈ�� ���ش�
        selectObj.SetActive(false);
        if (selectObj.transform.childCount > 1)
        {
            if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
            {
                selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // ��� �߷� ���ֱ�..
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
        
        // ������ �������� ���� Page���� Pages�� �߰����ش�
        for(int i =0;i<selectPages.transform.childCount;i++)
        {
            pages.Insert(i + 10, selectPages.transform.GetChild(i).gameObject);
        }
        // NextPage �Լ� �׳� ���⼭ ������
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
