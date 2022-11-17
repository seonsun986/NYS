using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Book");
    }

    RaycastHit hitInfo;
    int layerMask;
    private void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hitInfo, layerMask))
        //{
        //    if(hitInfo.transform.name.Contains("Book"))
        //    {
        //        if (outline1.activeSelf == false)
        //        {
        //            outline1.SetActive(true);
        //        }
        //    }

        //    else if(hitInfo.transform.name.Contains("Hey"))
        //    {
        //        if (outline2.activeSelf == false)
        //        {
        //            outline2.SetActive(true);
        //        }
        //    }

        //    else if(hitInfo.transform.name.Contains("Missing"))
        //    {
        //        if (outline3.activeSelf == false)
        //        {
        //            outline3.SetActive(true);
        //        }
        //    }

        //    else
        //    {
        //        if (outline1.activeSelf == true)
        //        {
        //            outline1.SetActive(false);
        //        }

        //        if (outline2.activeSelf == true)
        //        {
        //            outline2.SetActive(false);
        //        }

        //        if (outline3.activeSelf == true)
        //        {
        //            outline3.SetActive(false);
        //        }
        //    }
        //}

        //else
        //{
        //    if (outline1.activeSelf == true)
        //    {
        //        outline1.SetActive(false);
        //    }

        //    if (outline2.activeSelf == true)
        //    {
        //        outline2.SetActive(false);
        //    }

        //    if (outline3.activeSelf == true)
        //    {
        //        outline3.SetActive(false);
        //    }
        //}
    }

    public void Book1True()
    {
        if (outline1.activeSelf == false)
        {
            outline1.SetActive(true);
        }
    }

    public void Book1False()
    {
        if (outline1.activeSelf == true)
        {
            outline1.SetActive(false);
        }
    }

    public void Book2True()
    {
        if (outline2.activeSelf == false)
        {
            outline2.SetActive(true);
        }
    }

    public void Book2False()
    {
        if (outline2.activeSelf == true)
        {
            outline2.SetActive(false);
        }
    }

    public void Book3True()
    {
        if (outline3.activeSelf == false)
        {
            outline3.SetActive(true);
        }
    }
    public void Book3False()
    {
        if (outline3.activeSelf == true)
        {
            outline3.SetActive(false);
        }
    }

    // �� ���� �ȸԾ� å ���� ��
    public void SelectBook1()
    {
        // ý ����
        for (int i = 0; i < books.Count; i++)
        {
            books[i].SetActive(false);
            bookBG[i].SetActive(false);
            // ���ھ� ������
            iTween.MoveTo(boy, iTween.Hash("x", 0, "y", -8, "z", -1.6f, "easeType", "easeOutQuad", "time", 0.5f));
            StartCoroutine(boySound());
            // O, X ��ư Ű��
            OBtn.SetActive(true);
            XBtn.SetActive(true);
        }
    }


    IEnumerator boySound()
    {
        yield return new WaitForSeconds(0.6f);
        boy.GetComponent<AudioSource>().Play();
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
            // ���ھ� �������
            iTween.MoveTo(boy, iTween.Hash("x", 0, "y", -17.21f, "z", -1.6f, "easeType", "easeOutQuad", "time", 0.5f));
            // O, X ��ư ����
            OBtn.SetActive(false);
            XBtn.SetActive(false);
        }
    }

    public Transform BoyRigging;
    public Vector3 oBtnPos;
    public Vector3 xBtnPos;
    public Vector3 boyOriginPos;
    public float speed = 0.5f;
    float currentTime;
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

}
