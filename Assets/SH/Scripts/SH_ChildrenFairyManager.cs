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
    // �������� ���� �ٲ� �ؽ�Ʈ ����Ʈ
    public List<Text> boxTexts = new List<Text>();
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


    void Start()
    {
        
    }

    void Update()
    {
        // �������� �������ٸ� �ڽ� ä��� 
        // �ѹ��� ä�찳 ����
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
            }
        }
        // ���� �н� ��ư�̾��ٸ� �˾��� ���ش�
        GameObject PassPopUp = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        if (PassPopUp.name.Contains("Pass"))
        {
            PassPopUp.SetActive(false);
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

        // �ٽ� ���� �������� ���ش�
        pages[currentPage].SetActive(true);
        // ���� ������Ʈ�� ���ش�
        selectObj.SetActive(false);
        selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // ��� �߷� ���ֱ�..
    }

    // Ŭ���� ��ư�� �̸��� �־��ִ� ��
    public string boxText;
    public void FillEmptyBox()
    {
        GameObject SelectBtn = EventSystem.current.currentSelectedGameObject;
        boxText = SelectBtn.transform.GetChild(0).GetComponent<Text>().text;
    }
}
