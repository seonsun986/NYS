using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NK_Emotion;

public class NK_BookShelfManager : MonoBehaviour
{
    public List<string> titles = new List<string>();
    public GameObject booksParent;
    public GameObject bookFactory;
    public float spacing = 4;
    // å ���� ���� ���⿡ �ʿ��� �Ӽ�
    public GameObject detailUI;
    public Text detailTitle;
    // å ǥ�� ������ �ʿ��� �Ӽ�
    public GameObject bookCoverUI;

    // Start is called before the first frame update
    void Start()
    {
        // ��ȭå ���� �߰��ϸ�
        titles.Add("�� �� �÷�!!!");
        titles.Add("�� ���� �÷�!!!");
        titles.Add("�� ���� �÷�!!!");
        for (int i = 0; i < titles.Count; i++)
        {
            // ���� �߰��� ������ŭ ��ȭå ����
            GameObject book = Instantiate(bookFactory, booksParent.transform);
            // JSON���� �ҷ��� �������� �ؽ�Ʈ ����
            book.GetComponentInChildren<Text>().text = titles[i];
            book.GetComponent<Button>().onClick.AddListener(ClickBook);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickBook()
    {
        detailTitle.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        detailUI.SetActive(true);
        booksParent.SetActive(false);
    }

    public void UpdateBookCover()
    {
        bookCoverUI.SetActive(true);
    }

    public void UpdateBookContent()
    {
        SceneManager.LoadScene("EditorScene");
    }

    public void ExitDetail()
    {
        detailUI.SetActive(false);
        booksParent.SetActive(true);
    }

    public void ExitPopup()
    {
        bookCoverUI.SetActive(false);
    }
}
