using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SH_EditorChildren : MonoBehaviour
{
    public GameObject outline1;
    public GameObject outline2;
    public GameObject outline3;
    public List<GameObject> books;
    public GameObject boy;
    void Start()
    {
        
    }

    public void Book1True()
    {
        if(outline1.activeSelf == false)
        {
            outline1.SetActive(true);
        }
    }

    public void Book1False()
    {
        if(outline1.activeSelf == true)
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
        if(outline3.activeSelf == true)
        {
            outline3.SetActive(false);
        }
    }

    // ³­ ÄáÀº ¾È¸Ô¾î Ã¥ ¼±ÅÃ ½Ã
    public void SelectBook1()
    {
        for(int i =0; i<books.Count; i++)
        {
            books[i].SetActive(false);
        }
    }
}
