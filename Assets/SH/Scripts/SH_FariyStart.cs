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
    //public GameObject preBtn;
    void Start()
    {
        
    }

    RaycastHit hitInfo;
    public float currentTime;
    public AudioClip startClip;
    public YJ_ButtonClickSound clickSound;
    public int i = 0;
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime> startClip.length -0.3f && i <1)
        {
            iTween.ScaleTo(girl.gameObject, iTween.Hash("x", 0.4230038f, "y", 0.4230038f, "z", 0.4230038f, "time", 0.7f, "easeType", "easeInOutExpo"));
            iTween.ScaleTo(brother.gameObject, iTween.Hash("x", 0.4230038f, "y", 0.4230038f, "z", 0.4230038f, "time", 0.7f, "easeType", "easeInOutExpo"));
            i++;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.name == "Girl")
            {
                //if(girlText.activeSelf== false)
                //{
                //    girl.Play("Yes");
                //    brother.Rebind();
                //    brother.Play("Idle");
                //    girlText.SetActive(true);
                //    broText.SetActive(false);
                //    // �ƿ����� �ѱ�
                //    girl.gameObject.GetComponent<Outline>().enabled = true;
                //    brother.gameObject.GetComponent<Outline>().enabled = false;
                //}

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.Girl();
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.SelectSis();
                    gameObject.SetActive(false);
                    SH_ChildrenFairyManager.Instance.pages[0].SetActive(true);
                    //preBtn.SetActive(true);
                    //iTween.ScaleTo(preBtn, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 1));
                    //nextBtn.SetActive(true);
                    clickSound.OnClickStartSound();
                }
            }

            else if(hitInfo.transform.name == "Brother")
            {
                //if(broText.activeSelf == false)
                //{
                //    girl.Rebind();
                //    girl.Play("Idle");
                //    brother.Play("Yes");
                //    broText.SetActive(true);
                //    girlText.SetActive(false);
                //    girl.gameObject.GetComponent<Outline>().enabled = false;
                //    brother.gameObject.GetComponent<Outline>().enabled = true;
                //}

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.Boy();
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.SelectBro();
                    gameObject.SetActive(false);
                    SH_ChildrenFairyManager.Instance.pages[0].SetActive(true);
                    //preBtn.SetActive(true);
                    //nextBtn.SetActive(true);
                    clickSound.OnClickStartSound();

                }
            }
            
            //else
            //{
            //    girl.Rebind();
            //    brother.Rebind();
            //    girl.Play("Idle");
            //    brother.Play("Idle");
            //    girlText.SetActive(false);
            //    broText.SetActive(false);
            //    girl.gameObject.GetComponent<Outline>().enabled = false;
            //    brother.gameObject.GetComponent<Outline>().enabled = false;
            //}
        }

        //else
        //{
        //    girl.Rebind();
        //    brother.Rebind();
        //    girl.Play("Idle");
        //    brother.Play("Idle");
        //    girlText.SetActive(false);
        //    broText.SetActive(false);
        //    girl.gameObject.GetComponent<Outline>().enabled = false;
        //    brother.gameObject.GetComponent<Outline>().enabled = false;
        //}
    }
}
