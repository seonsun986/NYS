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
    public GameObject bearText;
    public GameObject tigerText;
    public GameObject rabbitText;
    public GameObject selectPopUp;
    public AudioSource btnClickSound;
    void Update()
    {
        // ¸¶¿ì½º Ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
        if(Physics.Raycast(ray, out hitInfo))
        {
            if (selectPopUp.activeSelf == true) return;
            if(hitInfo.transform.name == "BearBtn")
            {
                if(bear.enabled == false)
                {
                    bear.enabled = true;
                    bear.transform.GetComponent<Outline>().enabled = true;
                    tiger.transform.GetComponent<Outline>().enabled = false;
                    rabbit.transform.GetComponent<Outline>().enabled = false;
                    bearText.SetActive(true);
                    rabbit.Rebind();
                    tiger.Rebind();
                    tigerText.SetActive(false);
                    rabbitText.SetActive(false);
                }

                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                    btnClickSound.Play();

                }
            }
            else if(hitInfo.transform.name == "TigerBtn")
            {
                if (tiger.enabled == false)
                {
                    tiger.enabled = true;
                    tigerText.SetActive(true);
                    bear.Rebind();
                    bear.enabled = false;
                    rabbit.Rebind();
                    rabbit.enabled = false;
                    bearText.SetActive(false);
                    rabbitText.SetActive(false);

                    bear.transform.GetComponent<Outline>().enabled = false;
                    tiger.transform.GetComponent<Outline>().enabled = true;
                    rabbit.transform.GetComponent<Outline>().enabled = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                    btnClickSound.Play();

                }
            }
            else if (hitInfo.transform.name == "RabbitBtn")
            {
                if (rabbit.enabled == false)
                {
                    rabbit.enabled = true;
                    rabbitText.SetActive(true);
                    bear.Rebind();
                    bear.enabled = false;
                    tiger.Rebind();
                    tiger.enabled = false;
                    bearText.SetActive(false);
                    tigerText.SetActive(false);

                    bear.transform.GetComponent<Outline>().enabled = false;
                    tiger.transform.GetComponent<Outline>().enabled = false;
                    rabbit.transform.GetComponent<Outline>().enabled = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnPassPopUp();
                    btnClickSound.Play();

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
                bearText.SetActive(false);
                tigerText.SetActive(false);
                rabbitText.SetActive(false);
                bear.transform.GetComponent<Outline>().enabled = false;
                tiger.transform.GetComponent<Outline>().enabled = false;
                rabbit.transform.GetComponent<Outline>().enabled = false;
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
            bearText.SetActive(false);
            tigerText.SetActive(false);
            rabbitText.SetActive(false);
            bear.transform.GetComponent<Outline>().enabled = false;
            tiger.transform.GetComponent<Outline>().enabled = false;
            rabbit.transform.GetComponent<Outline>().enabled = false;
        }

    }
}
