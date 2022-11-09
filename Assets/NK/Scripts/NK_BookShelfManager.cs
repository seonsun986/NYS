using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NK_Emotion;

public class NK_BookShelfManager : MonoBehaviour
{
    public static NK_BookShelfManager instance;
    public List<string> titles = new List<string>();
    public List<Texture2D> images = new List<Texture2D>();
    public GameObject booksParent;
    public GameObject bookFactory;
    public float spacing = 4;
    // å ���� ���� ���⿡ �ʿ��� �Ӽ�
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // å ǥ�� ������ �ʿ��� �Ӽ�
    public GameObject bookCoverUI;
    // ������ ��ƼĿ
    public GameObject delSticker;

    private void Awake()
    {
        instance = this;
    }

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

        // rawImage �ʱ�ȭ
        rawImage.texture = images[0];
    }

    int index = 0;
    public void ClickBefore()
    {
        if (0 < index)
        {
            rawImage.texture = images[index];
            index--;
        }
    }

    public void ClickNext()
    {
        if(index < images.Count - 1)
        {
            rawImage.texture = images[index];
            index++;
        }
    }

    public void UpdateBookCover()
    {
        bookCoverUI.SetActive(true);
        NK_BookCover.instance.inputField.text = detailTitle.text;
    }

    public void UpdateBookContent()
    {
        SceneManager.LoadScene("EditorScene");
    }

    public void DeleteObj()
    {
        if(delSticker != null)
            Destroy(delSticker);
    }

    public void SaveBookCover()
    {
        // Json���� å ǥ�� ����
        // NK_BookCover.instance.bookCover.gameObject
        ExitPopup();
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
