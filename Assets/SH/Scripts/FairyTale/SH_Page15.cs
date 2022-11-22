using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page15 : MonoBehaviour
{
    public float changeTime;
    float currentTime;
    public GameObject text1;
    public GameObject text2;
    public Transform egg;
    private void Update()
    {
        if(gameObject.activeSelf == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime > changeTime)
            {
                text1.SetActive(false);
                text2.SetActive(true);
            }
        }

        if(egg!=null)
        {
            egg.Rotate(0, 0.1f, 0);
        }
    }
}

