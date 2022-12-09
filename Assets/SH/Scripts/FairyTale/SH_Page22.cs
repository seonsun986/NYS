using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page22 : MonoBehaviour
{
    public GameObject nextBtn;
    public GameObject endBtn;
    public GameObject reBtn;
    float currentTime;
    public Animation mask;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject girl;
    public GameObject boy;
    public GameObject bubble;
    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(true);
        nextBtn.SetActive(false);
        mask.Play();
    }

    int i = 0;
    int j = 0;
    int k = 0;
    int q;
    void Update()
    {
        
        currentTime += Time.deltaTime;
        if(currentTime>3.5f && k<1)
        {
            bubble.SetActive(false);
            if(girl.activeSelf == true)
            {
                iTween.ScaleTo(girl, iTween.Hash("x", 9.2024f, "y", 9.2024f, "z", 9.2024, "time", 0.5f));
            }
            else
            {
                iTween.ScaleTo(boy, iTween.Hash("x", 9.2024f, "y", 9.2024f, "z", 9.2024, "time", 0.5f));
            }
        }
        if(currentTime>8.8f && j<1)
        {
            if(girl.activeSelf == true)
            {
                iTween.RotateTo(girl, iTween.Hash("y", 180, "time", 0.3f));
                girl.GetComponent<Animator>().SetTrigger("Hi");
            }
            else
            {
                iTween.RotateTo(boy, iTween.Hash("y", 180, "time", 0.3f));
                boy.GetComponent<Animator>().SetTrigger("Hi");

            }

            j++;
        }
        if(currentTime>gameObject.GetComponent<AudioSource>().clip.length && i<1)
        {
            i++;
            if(girl.activeSelf==true)
            {
                girl.GetComponent<Animator>().SetTrigger("Walk");
                iTween.RotateTo(girl, iTween.Hash("y", -90, "time", 0.5f));
                StartCoroutine(GirlGo());
            }
            else
            {
                boy.GetComponent<Animator>().SetTrigger("Walk");
                iTween.RotateTo(boy, iTween.Hash("y", -90, "time", 0.5f));
                StartCoroutine(BoyGo());
            }
        }

        if (currentTime > 16.5f && q < 1)
        {
            // 끝내기, 다시하기 버튼 생성
            quizBoy.SetActive(true);
            iTween.MoveTo(quizBoy, iTween.Hash("y", -9f, "time", 1f));
            panel.SetActive(false);
            panelWorld.SetActive(true);
            page22Boy.SetActive(false);
            //endBtn.SetActive(true);
            //reBtn.SetActive(true);
            //iTween.ScaleTo(endBtn, iTween.Hash("x", 1.8f, "y", 1.8f, "z", 1.8f, "time", 1.5f ));
            //iTween.ScaleTo(reBtn, iTween.Hash("x", 1.8f, "y", 1.8f, "z", 1.8f, "time", 1.5f ));
            q++;
        }

        if(currentTime > quizBoy.GetComponent<AudioSource>().clip.length + 16.5f && q<2)
        {
            quizBG.SetActive(true);
            iTween.ScaleTo(quizBG, iTween.Hash("x", 10, "time", 1.5f, "easetype", "easeOutBounce"));
            q++;
        }

        if(currentTime > 25.5 && q<3)
        {
            quiz1.SetActive(true);
            quizOBtn.SetActive(true);
            quizXBtn.SetActive(true);
            iTween.ScaleTo(quizOBtn, iTween.Hash("x", 1.4154f, "y", 1.4154f, "z", 1.4154f, "time", 1.5f));
            iTween.ScaleTo(quizXBtn, iTween.Hash("x", 1.4154f, "y", 1.4154f, "z", 1.4154f, "time", 1.5f));
            q++;
        }

    }

    public GameObject quizBoy;
    public GameObject panel;
    public GameObject panelWorld;
    public GameObject quizBG;
    public GameObject quiz1;
    public GameObject quiz2;
    public GameObject quiz3;
    public GameObject quizOBtn;
    public GameObject quizXBtn;
    public GameObject page22Boy;

    IEnumerator GirlGo()
    {
        yield return new WaitForSeconds(0.5f);
        iTween.MoveTo(girl, iTween.Hash("x", -20, "time", 4));
    }

    IEnumerator BoyGo()
    {
        yield return new WaitForSeconds(0.5f);
        iTween.MoveTo(boy, iTween.Hash("x", -20, "time", 4));
    }


}
