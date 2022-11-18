  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_FootPrint : MonoBehaviour
{
    GameObject foot_R_OBJ;
    GameObject foot_L_OBJ;

    Material foot_R_MT;
    Material foot_L_MT;

    void Start()
    {
        foot_R_OBJ = transform.GetChild(0).gameObject;
        foot_L_OBJ = transform.GetChild(1).gameObject;

        foot_R_MT = foot_R_OBJ.GetComponent<MeshRenderer>().material;
        foot_L_MT = foot_L_OBJ.GetComponent<MeshRenderer>().material;

        foot_R_OBJ.SetActive(false);
        foot_L_OBJ.SetActive(false);
    }

    float currentTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 0)
        {
            foot_R_OBJ.SetActive(true);
        }
        if (currentTime > 0.2)
        {
            foot_L_OBJ.SetActive(true); 
        }
        if (currentTime > 0.5)
        {
            Color color1 = foot_R_MT.color;
            color1.a -= 0.05f;
            if (color1.a < 0) color1.a = 0;
            foot_R_MT.color = color1;
        }
        if (currentTime > 0.7)
        {
            Color color2 = foot_L_MT.color;
            color2.a -= 0.05f;
            if (color2.a < 0)
            {
                color2.a = 0;
                Destroy(this.gameObject);
            }
            foot_L_MT.color = color2;
        }
    }
}
