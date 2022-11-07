using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static NK_Emotion;

public class NK_BookShelfManager : MonoBehaviour
{
    public List<string> titles = new List<string>();
    public GameObject booksParent;
    public GameObject bookFactory;
    public GameObject DetailUI;
    public float spacing = 4;

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
            // 간격만큼 동화책 정렬
            book.transform.position += new Vector3(spacing * i, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭하면
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
            // LayerMask가 Book이면
            if (hit.transform.gameObject.layer == 7)
            {
                DetailUI.SetActive(true);
                booksParent.SetActive(false);
            }
            else
            {
                DetailUI.SetActive(false);
                booksParent.SetActive(true);
            }
        }
    }
}
