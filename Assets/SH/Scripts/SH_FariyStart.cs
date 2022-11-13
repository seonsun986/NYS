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
    public GameObject nextBtn;
    public GameObject preBtn;
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
                if(girlText.activeSelf== false)
                {
                    girl.Play("Yes");
                    brother.Rebind();
                    brother.Play("Idle");
                    girlText.SetActive(true);
                    broText.SetActive(false);
                }

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.Girl();
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.SelectSis();
                    gameObject.SetActive(false);
                    SH_ChildrenFairyManager.Instance.pages[0].SetActive(true);
                    preBtn.SetActive(true);
                    nextBtn.SetActive(true);
                }
            }

            else if(hitInfo.transform.name == "Brother")
            {
                if(broText.activeSelf == false)
                {
                    girl.Rebind();
                    girl.Play("Idle");
                    brother.Play("Yes");
                    broText.SetActive(true);
                    girlText.SetActive(false);
                }

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.Boy();
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.SelectBro();
                    gameObject.SetActive(false);
                    SH_ChildrenFairyManager.Instance.pages[0].SetActive(true);
                    preBtn.SetActive(true);
                    nextBtn.SetActive(true);
                }
            }
            
            else
            {
                girl.Rebind();
                brother.Rebind();
                girl.Play("Idle");
                brother.Play("Idle");
                girlText.SetActive(false);
                broText.SetActive(false);
            }
        }

        else
        {
            girl.Rebind();
            brother.Rebind();
            girl.Play("Idle");
            brother.Play("Idle");
            girlText.SetActive(false);
            broText.SetActive(false);
        }
    }
}
