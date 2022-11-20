using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page11 : MonoBehaviour
{
    void Start()
    {
        
    }
    public float changeTime = 5.5f;
    float currentTime;
    public GameObject boyText;
    public GameObject broSisText;
    void Update()
    {
        if(gameObject.activeSelf == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime > changeTime)
            {
                boyText.SetActive(false);
                broSisText.SetActive(true);
            }
        }
    }
}
