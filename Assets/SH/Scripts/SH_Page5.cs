using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page5 : MonoBehaviour
{
    void Start()
    {
        
    }

    public GameObject box;
    public GameObject ball;
    public GameObject book;

    RaycastHit hitInfo;
        int i = 0;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo))
        {

            if(hitInfo.transform.name == "Box")
            {
                if (i < 1)
                {
                    // 박스 크기 키우기
                    ScaleUp(box, 0.00015f, "x");
                    ScaleUp(box, 0.00015f, "y");
                    ScaleUp(box, 0.00015f, "z");
                    i++;
                }

            }
        }
    }


    public void ScaleUp(GameObject go, float scaleSize, string axis = "")
    {
        Hashtable hash = iTween.Hash(axis, scaleSize,
            "time", 0.3f); 

        iTween.ScaleTo(go, hash);
    }

}
