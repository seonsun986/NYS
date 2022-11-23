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
    public Animation pearl;
    public GameObject cactus;
    public GameObject potato;
    public Transform egg;
    void Update()
    {
        if(gameObject.activeSelf== true)
        {
            if (SH_ChildrenFairyManager.Instance.pages[12].activeSelf == true && SH_ChildrenFairyManager.Instance.eggB == true)
            {
                egg.Rotate(0, 0.1f, 0);
            }

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
                        iTween.ScaleFrom(spinach, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
                    }
                }

                if (SH_ChildrenFairyManager.Instance.pages[12].activeSelf == true && SH_ChildrenFairyManager.Instance.onionB == true)
                {
                    pearl.Play();
                }

                if (SH_ChildrenFairyManager.Instance.pages[12].activeSelf == true && SH_ChildrenFairyManager.Instance.potatoB == true)
                {
                    iTween.ScaleTo(cactus, iTween.Hash("x", 1.6718f, "y", 1.6718f, "z", 1.6718f, "time", 0.3f));
                    if(currentTime>7.5f)
                    {
                        iTween.ScaleTo(potato, iTween.Hash("x", 0.524f, "y", 0.524f, "z", 0.524f, "time", 0.3f));

                    }
                }

               
            }
        }
    }
}
