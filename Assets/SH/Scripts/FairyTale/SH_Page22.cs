using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page22 : MonoBehaviour
{
    public AudioClip audio22;
    public GameObject nextBtn;
    public GameObject endBtn;
    float currentTime;
    void Start()
    {
            nextBtn.SetActive(false);
    }


    void Update()
    {

        currentTime += Time.deltaTime;
        if(currentTime>audio22.length)
        {
            endBtn.SetActive(true);
            iTween.ScaleFrom(endBtn, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.3f));
        }
    }
}
