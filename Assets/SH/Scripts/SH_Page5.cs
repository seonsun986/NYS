using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Page5 : MonoBehaviour
{
    void Start()
    {
        
    }

    public Animation box;
    public Animation ball;
    public Animator book;
    RaycastHit hitInfo;

    bool boxB;
    bool ballB;
    bool bookB;

    public GameObject ballText;
    public GameObject boxText;
    public GameObject bookText;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // �ڽ��� ���콺 �÷����� ��
            if(hitInfo.transform.name == "BoxBtn")
            {
                if(boxB == false)
                {
                    box.Play("BoxSizeUp");
                    boxB = true;
                    boxText.SetActive(true);
                }

                // �̶� ���콺 ������ �Լ� ����
                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }

                // å, �� �۰� ���ֱ�
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                bookB = false;
                ballB = false;
                bookText.SetActive(false);
                ballText.SetActive(false);
                
            }

            // å�� ���콺 �÷����� ��
            else if (hitInfo.transform.name == "BookBtn")
            {
                
                if(bookB == false)
                {
                    book.Rebind();
                    book.enabled = true;
                    bookB = true;
                    bookText.SetActive(true);
                }


                // �̶� ���콺 ������ �Լ� ����
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }
                // �ڽ�, �� �۰� ���ֱ�
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                ballB = false;
                boxB = false;
                ballText.SetActive(false);
                boxText.SetActive(false);
            }

            // ���� ���콺 �÷����� ��
            else if(hitInfo.transform.name == "BallBtn")
            {
                if(ballB == false)
                {
                    ball.Play("BallSizeUp");
                    ballB = true;
                    ballText.SetActive(true);
                }

                // �̶� ���콺 ������ �Լ� ����
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnPassPopUp();
                }

                // �ڽ�, å �۰� ���ֱ�
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                boxB = false;
                bookB = false;
                boxText.SetActive(false);
                bookText.SetActive(false);
            }

            // �ٸ� �ſ� �ε����� �� �� �۰� ���ش�
            else
            {
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                ballB = false;
                boxB = false;
                bookB = false;
                ballText.SetActive(false);
                boxText.SetActive(false);
                bookText.SetActive(false);
            }    

        }

        // �ƹ��͵� �ε����� �ʾ��� ���� �� �۰� ���ش�
        else
        {
            ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
            book.enabled = false;
            book.transform.localScale = new Vector3(1, 1, 1);
            ballB = false;
            boxB = false;
            bookB = false;
            ballText.SetActive(false);
            boxText.SetActive(false);
            bookText.SetActive(false);
        }
    }


}
