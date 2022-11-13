using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_FariyStart : MonoBehaviour
{
    public Animator girl;
    public Animator brother;
    public GameObject girlText;
    public GameObject broText;
    
    void Start()
    {
        
    }

    RaycastHit hitInfo;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.name == "Girl")
            {

            }
        }
    }
}
