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
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // 박스에 마우스 올려뒀을 때
            if(hitInfo.transform.name == "BoxBtn")
            {
                if(boxB == false)
                {
                    box.Play("BoxSizeUp");
                    boxB = true;
                }

                // 이때 마우스 누르면 함수 실행
                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }

                // 책, 공 작게 해주기
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                bookB = false;
                ballB = false;
                
            }

            // 책에 마우스 올려뒀을 때
            else if (hitInfo.transform.name == "BookBtn")
            {
                
                if(bookB == false)
                {
                    book.Rebind();
                    book.enabled = true;
                    bookB = true;
                }
                else
                {
                    return;
                }

                // 이때 마우스 누르면 함수 실행
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }
                // 박스, 공 작게 해주기
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                ballB = false;
                boxB = false;
            }

            // 공에 마우스 올려뒀을 때
            else if(hitInfo.transform.name == "BallBtn")
            {
                if(ballB == false)
                {
                    ball.Play("BallSizeUp");
                    ballB = true;
                }

                // 이때 마우스 누르면 함수 실행
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnPassPopUp();
                }

                // 박스, 책 작게 해주기
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                boxB = false;
                bookB = false;
            }

            // 다른 거에 부딪혔을 때 다 작게 해준다
            else
            {
                ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1, 1, 1);
                ballB = false;
                boxB = false;
                bookB = false;
            }    

        }

        // 아무것도 부딪히지 않았을 때도 다 작게 해준다
        else
        {
            ball.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            box.gameObject.transform.localScale = new Vector3(0.0001034214f, 0.0001034214f, 0.0001034214f);
            book.enabled = false;
            book.transform.localScale = new Vector3(1, 1, 1);
            ballB = false;
            boxB = false;
            bookB = false;
        }
    }


}
