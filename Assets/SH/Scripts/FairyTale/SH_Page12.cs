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
            }
        }
    }
}
