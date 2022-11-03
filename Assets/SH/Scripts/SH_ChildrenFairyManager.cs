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
    }

    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);
        // �ش� FailBtn�� ���õ� �ִϸ��̼� �������!
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

    // Ŭ���� ��ư�� �̸��� �־��ִ� ��
    public string boxText;
    public void FillEmptyBox()
    {
        GameObject SelectBtn = EventSystem.current.currentSelectedGameObject;
        boxText = SelectBtn.transform.GetChild(0).GetComponent<Text>().text;
    }
}
