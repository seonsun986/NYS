using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page22 : MonoBehaviour
{
    public GameObject nextBtn;
    public GameObject endBtn;
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

        if(currentTime >16.5f && q<1)
        {
            endBtn.SetActive(true);
            iTween.ScaleTo(endBtn, iTween.Hash("x", 2, "y", 2, "z", 2, "time", 0.7f ));
            q++;
        }
    }

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
