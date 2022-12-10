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
            iTween.ScaleTo(quizBG, iTween.Hash("x", 0.2851699f, "time", 1.5f, "easetype", "easeOutBounce"));
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

    // 문제 1 : 내 동생은 당근을 좋아했을까요? X
    // 문제 2 : 콩이랑 공은 둥글게 생겼을까요? O
    // 문제 3 : 이제 내 동생은 콩을 먹을 수 있을까요? O
    public AudioClip correctClip;
    public AudioClip incorrectClip;
    public AudioClip quizEnd;
    public GameObject quizGirl;
    public GameObject quizBrother;
    public GameObject quizGoodText;
    public ParticleSystem quizFirework;
    public void QuizOBtn()
    {
        if(q<4)
        {
            quiz1.GetComponent<AudioSource>().Stop();
            quizOBtn.GetComponent<AudioSource>().clip = incorrectClip;
            quizOBtn.GetComponent<AudioSource>().Play();
        }

        else if(q<5)
        {
            quizOBtn.GetComponent<AudioSource>().clip = correctClip;
            quizOBtn.GetComponent<AudioSource>().Play();
            quiz2.SetActive(false);
            quizGirl.SetActive(true);
            quizBrother.SetActive(true);
            quizGoodText.SetActive(true);
            quizFirework.Play();
            iTween.ScaleTo(quizGirl, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizBrother, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizGoodText, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.7f));

            ButtonSizeDown();
            StartCoroutine(NextQuiz());
            q++;
        }

        else
        {
            quizOBtn.GetComponent<AudioSource>().clip = correctClip;
            quizOBtn.GetComponent<AudioSource>().Play();
            quiz3.SetActive(false);
            quizGirl.SetActive(true);
            quizBrother.SetActive(true);
            quizGoodText.SetActive(true);
            quizFirework.Play();

            iTween.ScaleTo(quizGirl, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizBrother, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizGoodText, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.7f));
            ButtonSizeDown();
            StartCoroutine(NextQuiz());
            q++;
        }
    }

    public void QuizXBtn()
    {
        if(q<4)
        {
            quizOBtn.GetComponent<AudioSource>().clip = correctClip;
            quizOBtn.GetComponent<AudioSource>().Play();
            quiz1.SetActive(false);
            quizGirl.SetActive(true);
            quizBrother.SetActive(true);
            quizGoodText.SetActive(true);
            quizFirework.Play();

            iTween.ScaleTo(quizGirl, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizBrother, iTween.Hash("x", 4.185395f, "y", 4.185395f, "z", 4.185395f, "time", 0.7f));
            iTween.ScaleTo(quizGoodText, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.7f));
            ButtonSizeDown();
            StartCoroutine(NextQuiz());
            q++;
      
        }
        else if(q<5)
        {
            quiz2.GetComponent<AudioSource>().Stop();
            quizOBtn.GetComponent<AudioSource>().clip = incorrectClip;
            quizOBtn.GetComponent<AudioSource>().Play();
        }
        else
        {
            quiz3.GetComponent<AudioSource>().Stop();

            quizOBtn.GetComponent<AudioSource>().clip = incorrectClip;
            quizOBtn.GetComponent<AudioSource>().Play();
        }
    }

    // O,X 버튼 키우기
    public void ButtonSizeUp()
    {
        iTween.ScaleTo(quizOBtn, iTween.Hash("x", 1.4154f, "y", 1.4154f, "z", 1.4154f));
        iTween.ScaleTo(quizXBtn, iTween.Hash("x", 1.4154f, "y", 1.4154f, "z", 1.4154f));
    }

    public void ButtonSizeDown()
    {
        iTween.ScaleTo(quizOBtn, iTween.Hash("x", 0, "y", 0, "z", 0));
        iTween.ScaleTo(quizXBtn, iTween.Hash("x", 0, "y", 0, "z", 0));
    }

    IEnumerator NextQuiz()
    {
        yield return new WaitForSeconds(4f);

        iTween.ScaleTo(quizGirl, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.7f));
        iTween.ScaleTo(quizBrother, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.7f));
        iTween.ScaleTo(quizGoodText, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.7f));
        yield return new WaitForSeconds(1f);
        quizGirl.SetActive(false);
        quizBrother.SetActive(false);
        quizGoodText.SetActive(false);

        // 퀴즈 1 -> 퀴즈 2 
        if (q<5)
        {
            quiz2.SetActive(true);
            ButtonSizeUp();
        }

        else if(q<6)
        {
            quiz3.SetActive(true);
            ButtonSizeUp();
        }

        else
        {
            quizBoy.GetComponent<Animator>().Play("Victory");
            quizBoy.GetComponent<AudioSource>().clip = quizEnd;
            quizBoy.GetComponent<AudioSource>().Play();
            iTween.ScaleTo(quizBG, iTween.Hash("x", 0, "time", 1, "easetype", "easeOutQuad"));
            yield return new WaitForSeconds(4.2f);
            iTween.RotateTo(quizBoy, iTween.Hash("y", 180, "time", 2));
            quizBoy.GetComponent<Animator>().SetTrigger("Hi");
            yield return new WaitForSeconds(6f);
            iTween.MoveTo(quizBoy, iTween.Hash("y", -18f,"time",1.5f));
            yield return new WaitForSeconds(1f);
            endBtn.SetActive(true);
            reBtn.SetActive(true);
            iTween.ScaleTo(endBtn, iTween.Hash("x", 1.8f, "y", 1.8f, "z", 1.8f, "time", 1.5f));
            iTween.ScaleTo(reBtn, iTween.Hash("x", 1.8f, "y", 1.8f, "z", 1.8f, "time", 1.5f));
            quizBoy.SetActive(false);
        }
    }

}

