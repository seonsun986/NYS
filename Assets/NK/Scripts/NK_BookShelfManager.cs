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
    // 책 세부 내용 보기에 필요한 속성
    public GameObject detailUI;
    public Text detailTitle;
    public RawImage rawImage;
    // 책 표지 수정에 필요한 속성
    public GameObject bookCoverUI;
    // 삭제될 스티커
    public GameObject delSticker;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 동화책 제목 추가하면
        titles.Add("난 콩 시러!!!");
        titles.Add("난 버섯 시러!!!");
        titles.Add("난 오이 시러!!!");
        for (int i = 0; i < titles.Count; i++)
        {
            // 제목 추가된 개수만큼 동화책 생성
            GameObject book = Instantiate(bookFactory, booksParent.transform);
            // JSON에서 불러온 제목으로 텍스트 지정
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

        // rawImage 초기화
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
        // Json으로 책 표지 저장
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
