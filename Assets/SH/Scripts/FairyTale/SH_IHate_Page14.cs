using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_IHate_Page14 : MonoBehaviour
{
    public GameObject girl_Yes;
    public GameObject girl_No;
    public GameObject bro_Yes;
    public GameObject bro_No;
    public GameObject yesText;
    public GameObject noText;

    void Start()
    {
        
    }
    RaycastHit hitInfo;
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo))
        {
            // ���� ��ư ���� ������Ʈ���
            if (hitInfo.transform.name == "Girl_Yes" || hitInfo.transform.name == "Brother_Yes")
            {
#if !UNITY_ANDROID
                if(yesText.activeSelf == false)
                {
                    yesText.SetActive(true);
                    noText.SetActive(false);
                }
#endif              
                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.PassTrue();
                    SH_ChildrenFairyManager.Instance.NextPage();
                    yesText.SetActive(false);
                    noText.SetActive(false);
                }
            }

            else if (hitInfo.transform.name == "Girl_No" || hitInfo.transform.name == "Brother_No")
            {
#if !UNITY_ANDROID
                if (noText.activeSelf == false)
                {
                    noText.SetActive(true);
                    yesText.SetActive(false);
                }
#endif
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.TryNo();
                    yesText.SetActive(false);
                    noText.SetActive(false);
                }

            }
#if !UNITY_ANDROID
            // �ٸ��ſ� �ε����� ��
            else
            {
                yesText.SetActive(false);
                noText.SetActive(false);
            }
#endif
        }
        // �ƹ��͵� �ε����� �ʾ��� ��
#if !UNITY_ANDROID

        else
        {
            yesText.SetActive(false);
            noText.SetActive(false);
        }
#endif
    }
}
