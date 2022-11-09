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
    // 책 세부 내용 보기에 필요한 속성
    public GameObject detailUI;
    public Text detailTitle;
    // 책 표지 수정에 필요한 속성
    public GameObject bookCoverUI;

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
