using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SH_EditorChildren : MonoBehaviour
{
    public GameObject outline1;
    public GameObject outline2;
    public GameObject outline3;
    public List<GameObject> books;
    public List<GameObject> bookBG;
    public GameObject boy;
    public GameObject OBtn;
    public GameObject XBtn;
    public GameObject fairys;       // 처음에 효과주기 위한 동화들 목록
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Book");
        iTween.ScaleFrom(bg, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.4f));
        iTween.ScaleFrom(selectBook, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.4f));
        //iTween.ScaleFrom(fairys, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.4f));

    }

    RaycastHit hitInfo;
    int layerMask;
    private void Update()
    {
      
    }

    public GameObject bg;
    public GameObject selectBook;
    public Sprite peas;
    public Sprite originBGImg;
    public Image backGroundImg;
    // 난 콩은 안먹어 책 선택 시
    public void SelectBook1()
    {
        StartCoroutine(book1Effect());
    }

    IEnumerator book1Effect()
    {
        OnClickMTChangeBook1();
        yield return new WaitForSeconds(0.3f);
        SmallButton();
        yield return new WaitForSeconds(0.5f);
        // 첵 끄기
        for (int i = 0; i < books.Count; i++)
        {
            books[i].SetActive(false);
            //bookBG[i].SetActive(false);
        }
        // 배경 바꾸기
        backGroundImg.sprite = peas;
        //iTween.ScaleTo(bg, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.4f));
        bg.transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(selectBook, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.4f));
        // 남자애 나오기
        iTween.MoveTo(boy, iTween.Hash("x", 0, "y", -8, "z", -1.6f, "easeType", "easeOutQuad", "time", 0.5f));
        //StartCoroutine(boySound());
        boy.GetComponent<AudioSource>().enabled = true;
        StartCoroutine(OXShow());
    }
    public void OnClickMTChangeBook1()
    {
        iTween.ScaleTo(books[0], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "easeType", "easeOutSine", "time", 0.2f));
    }

    void SmallButton()
    {
        iTween.ScaleTo(books[0], iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", "easeOutSine", "time", 0.2f));
    }


    IEnumerator boySound()
    {
        yield return new WaitForSeconds(0.3f);
        boy.GetComponent<AudioSource>().enabled = true;
        StartCoroutine(OXShow());
    }

    IEnumerator OXShow()
    {
        yield return new WaitForSeconds(6.7f);
        OBtn.SetActive(true);
        XBtn.SetActive(true);
        iTween.ScaleTo(OBtn, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.4f, "easeType", "easeOutBack"));
        iTween.ScaleTo(XBtn, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.4f, "easeType", "easeOutBack"));
    }


    public void OkBtn()
    {
        SceneManager.LoadScene("Fairy_IHate");
    }

    public void NoBtn()
    {
        for (int i = 0; i < books.Count; i++)
        {
            books[i].SetActive(true);          
        }
        // 남자애 사라지기
        iTween.MoveTo(boy, iTween.Hash("x", 0, "y", -17.21f, "z", -1.6f, "easeType", "easeOutQuad", "time", 0.5f));
        // O, X 버튼 끄기
        iTween.ScaleTo(OBtn, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.3f));
        iTween.ScaleTo(XBtn, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.3f));
        iTween.ScaleTo(bg, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.4f));
        iTween.ScaleTo(selectBook, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.4f));
        boy.GetComponent<AudioSource>().enabled = false;
        //OBtn.SetActive(false);
        //XBtn.SetActive(false);
        backBtn.SetActive(true);
        preBtn.SetActive(true);
        nextBtn.SetActive(true);
        // 배경 바꾸기
        backGroundImg.sprite = originBGImg;
    }

    public Transform BoyRigging;
    public Vector3 oBtnPos;
    public Vector3 xBtnPos;
    public Vector3 boyOriginPos;
    public float speed = 0.5f;
    float currentTime;
#if !UNITY_ANDROID
    public void OBtnRigging()
    {
        StopAllCoroutines();
        StartCoroutine(OLerp());
    }

    IEnumerator OLerp()
    {
        while (true)
        {
            if (BoyRigging.transform.position.x <=  -19.5f)
            {
                currentTime = 0;
                break;
            }
            BoyRigging.position = Vector3.Lerp(BoyRigging.transform.position, oBtnPos, Time.deltaTime * speed) ;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void XBtnRigging()
    {
        StopAllCoroutines();
        StartCoroutine(XLerp());
    }

    IEnumerator XLerp()
    {
        while (true)
        {
            if (BoyRigging.transform.position.x >= 19.5f)
            {
                currentTime = 0;
                break;
            }
            BoyRigging.position = Vector3.Lerp(BoyRigging.transform.position, xBtnPos, Time.deltaTime * speed);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }


    public void BoyOringRig()
    {
        StartCoroutine(RiggingStop());
    }

    IEnumerator RiggingStop()
    {
        while (true)
        {
            if (Time.deltaTime * speed >= 1)
            {
                currentTime = 0;
                break;
            }
            BoyRigging.position = Vector3.Lerp(BoyRigging.transform.position, boyOriginPos, Time.time * speed);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
#endif

    public GameObject backBtn;
    public GameObject preBtn;
    public GameObject nextBtn;
    public void SelectBtn()
    {
        backBtn.SetActive(false);
        preBtn.SetActive(false);
        nextBtn.SetActive(false);
    }

#if !UNITY_ANDROID
    public void Book1ScaleUp()
    {
        iTween.ScaleTo(books[0], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", 0.3f));
    }

    public void Book1ScaleDown()
    {
        iTween.ScaleTo(books[0], iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.3f));

    }

    public void Book2ScaleUp()
    {
        iTween.ScaleTo(books[1], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", 0.3f));
    }

    public void Book2ScaleDown()
    {
        iTween.ScaleTo(books[1], iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.3f));

    }


    public void Book3ScaleUp()
    {
        iTween.ScaleTo(books[2], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", 0.3f));
    }

    public void Book3ScaleDown()
    {
        iTween.ScaleTo(books[2], iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.3f));
    }

    public void Book4ScaleUp()
    {
        iTween.ScaleTo(books[3], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", 0.3f));

    }

    public void Book4ScaleDown()
    {
        iTween.ScaleTo(books[3], iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.3f));
    }

    public void Book5ScaleUp()
    {
        iTween.ScaleTo(books[4], iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", 0.3f));

    }

    public void Book5ScaleDown()
    {
        iTween.ScaleTo(books[4], iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.3f));
    }

#endif

    public GameObject contentPos;
    public void NextBtn(float change)
    {
        //print(contentPos.GetComponent<RectTransform>().sizeDelta.x);
        //print(contentPos.GetComponent<RectTransform>().anchoredPosition.x + change);
        //iTween.MoveTo(contentPos, iTween.Hash("x", contentPos.GetComponent<RectTransform>().anchoredPosition.x + change, "time", 0.5f));

        float x = contentPos.GetComponent<RectTransform>().anchoredPosition.x;
        if (x + change < -900)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
          "from", x, "to", -840, "time", 0.3f,
          "onupdatetarget", gameObject, "onupdate", "그냥러프써"));
        }
        else
        {
            iTween.ValueTo(gameObject, iTween.Hash(
            "from", x, "to", x + change, "time", 0.3f,
            "onupdatetarget", gameObject, "onupdate", "그냥러프써"));
        }

    }

    void 그냥러프써(float v)
    {
        RectTransform rt = contentPos.GetComponent<RectTransform>();
        Vector2 v2 = rt.anchoredPosition;
        v2.x = v;
        rt.anchoredPosition = v2;

    }

    public void PreBtn(float change)
    {
        iTween.MoveTo(contentPos, iTween.Hash("x", contentPos.GetComponent<RectTransform>().anchoredPosition.x + change, "time", 0.3f));

    }

    public void GoMyRoom()
    {
        SceneManager.LoadScene("MyRoomScene");
    }



}
