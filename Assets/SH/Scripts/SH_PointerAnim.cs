using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PointerAnim : MonoBehaviour
{
    void Start()
    {
        
    }
    RaycastHit hitInfo;
    public Animator bear;
    public Animator tiger;
    public Animator rabbit;
    void Update()
    {
        // ¸¶¿ì½º Ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.name == "BearBtn")
            {
                if(bear.enabled == false)
                {
                    bear.enabled = true;
                    rabbit.Rebind();
                    tiger.Rebind();
                }

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }
            }
            else if(hitInfo.transform.name == "TigerBtn")
            {
                if (tiger.enabled == false)
                {
                    tiger.enabled = true;
                    bear.Rebind();
                    bear.enabled = false;
                    rabbit.Rebind();
                    rabbit.enabled = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }
            }
            else if (hitInfo.transform.name == "RabbitBtn")
            {
                if (rabbit.enabled == false)
                {
                    rabbit.enabled = true;
                    bear.Rebind();
                    bear.enabled = false;
                    tiger.Rebind();
                    tiger.enabled = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnPassPopUp();
                }
            }
            else
            {
                rabbit.Rebind();
                rabbit.enabled = false;
                tiger.Rebind();
                tiger.enabled = false;
                bear.Rebind();
                bear.enabled = false;
            }
        }
        else
        {
            rabbit.Rebind();
            rabbit.enabled = false;
            tiger.Rebind();
            tiger.enabled = false;
            bear.Rebind();
            bear.enabled = false;
        }

    }
}
