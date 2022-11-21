using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page12 : MonoBehaviour
{
    void Start()
    {
        
    }

    public float changeTime1 = 3.4f;
    public float changeTime2 = 5.6f;
    float currentTime;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;

    public Animator bro;
    public Animator girl;
    public GameObject pineTree;
    public GameObject spinach;
    void Update()
    {
        if(gameObject.activeSelf== true)
        {
            currentTime += Time.deltaTime;
            if(currentTime>changeTime1)
            {
                text1.SetActive(false);
                text2.SetActive(true);
            }

            if(currentTime>changeTime2)
            {
                text2.SetActive(false);
                text3.SetActive(true);

                // 페이지 18에서는 동생의 애니메이션이 달라진다
                if(SH_ChildrenFairyManager.Instance.pages[18].activeSelf == true)
                {
                    // 남동생이 켜져있다면
                    if(bro.gameObject.activeSelf == true)
                    {
                        bro.SetTrigger("Lose");
                    }
                    else
                    {
                        girl.SetTrigger("Lose");
                    }
                }

                if(SH_ChildrenFairyManager.Instance.pages[12].activeSelf == true && SH_ChildrenFairyManager.Instance.spinachB == true)
                {
                    iTween.ScaleTo(pineTree, iTween.Hash("x", 2.538f, "y", 2.538f, "z", 2.538f, "time", 0.3f));
                    if(currentTime>5.9f)
                    {
                        spinach.SetActive(true);
                    }
                }
            }
        }
    }
}
