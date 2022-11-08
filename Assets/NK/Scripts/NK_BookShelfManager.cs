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
    public GameObject customBook;

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
            // ���ݸ�ŭ ��ȭå ����
            book.transform.position += new Vector3(spacing * i, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 Ŭ���ϸ�
        if (Input.GetMouseButtonDown(0))
        {
            ClickBook();
        }
    }

    public void ClickBook()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // LayerMask�� Book�̸�
            if (hit.transform.gameObject.layer == 7)
            {
                detailTitle.text = hit.transform.gameObject.GetComponentInChildren<Text>().text;
                detailUI.SetActive(true);
                booksParent.SetActive(false);
            }
            else
            {
                detailUI.SetActive(false);
                booksParent.SetActive(true);
            }
        }
    }

    public void UpdateBookCover()
    {
        bookCoverUI.SetActive(true);
        customBook.SetActive(true);
    }

    public void UpdateBookContent()
    {
        SceneManager.LoadScene("EditorScene");
    }
}
