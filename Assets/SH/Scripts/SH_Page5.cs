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
            // 박스에 마우스 올려뒀을 때
            if(hitInfo.transform.name == "BoxBtn")
            {
                if(boxB == false)
                {
                    box.Play("BoxSizeUp");
                    boxB = true;
                    boxText.SetActive(true);
                }

                // 이때 마우스 누르면 함수 실행
                if(Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }

                // 책, 공 작게 해주기
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1.4773f, 1.4773f, 1.4773f);
                ball.gameObject.transform.localScale = new Vector3(0.16541f, 0.16541f, 0.16541f);
                bookB = false;
                ballB = false;
                bookText.SetActive(false);
                ballText.SetActive(false);
                
            }

            // 책에 마우스 올려뒀을 때
            else if (hitInfo.transform.name == "BookBtn")
            {
                
                if(bookB == false)
                {
                    book.Rebind();
                    book.enabled = true;
                    bookB = true;
                    bookText.SetActive(true);
                }


                // 이때 마우스 누르면 함수 실행
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnFailPopUp();
                }
                // 박스, 공 작게 해주기
                ball.gameObject.transform.localScale = new Vector3(0.16541f, 0.16541f, 0.16541f);
                box.gameObject.transform.localScale = new Vector3(0.0001363301f, 0.0001363301f, 0.0001363301f);
                ballB = false;
                boxB = false;
                ballText.SetActive(false);
                boxText.SetActive(false);
            }

            // 공에 마우스 올려뒀을 때
            else if(hitInfo.transform.name == "BallBtn")
            {
                if(ballB == false)
                {
                    ball.Play("BallSizeUp");
                    ballB = true;
                    ballText.SetActive(true);
                }

                // 이때 마우스 누르면 함수 실행
                if (Input.GetMouseButtonDown(0))
                {
                    SH_ChildrenFairyManager.Instance.OnPassPopUp();
                }

                // 박스, 책 작게 해주기
                box.gameObject.transform.localScale = new Vector3(0.0001363301f, 0.0001363301f, 0.0001363301f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1.4773f, 1.4773f, 1.4773f);
                boxB = false;
                bookB = false;
                boxText.SetActive(false);
                bookText.SetActive(false);
            }

            // 다른 거에 부딪혔을 때 다 작게 해준다
            else
            {
                ball.gameObject.transform.localScale = new Vector3(0.16541f, 0.16541f, 0.16541f);
                box.gameObject.transform.localScale = new Vector3(0.0001363301f, 0.0001363301f, 0.0001363301f);
                book.enabled = false;
                book.gameObject.transform.localScale = new Vector3(1.4773f, 1.4773f, 1.4773f);
                ballB = false;
                boxB = false;
                bookB = false;
                ballText.SetActive(false);
                boxText.SetActive(false);
                bookText.SetActive(false);
            }    

        }

        // 아무것도 부딪히지 않았을 때도 다 작게 해준다
        else
        {
            ball.gameObject.transform.localScale = new Vector3(0.16541f, 0.16541f, 0.16541f);
            box.gameObject.transform.localScale = new Vector3(0.0001363301f, 0.0001363301f, 0.0001363301f);
            book.transform.localScale = new Vector3(1.4773f, 1.4773f, 1.4773f);
            book.enabled = false;
            ballB = false;
            boxB = false;
            bookB = false;
            ballText.SetActive(false);
            boxText.SetActive(false);
            bookText.SetActive(false);
        }
    }


}
