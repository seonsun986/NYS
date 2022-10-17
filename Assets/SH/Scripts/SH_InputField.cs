using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_InputField : MonoBehaviour
{
    // eventTrigger에서 포인터를 업다운을 한 시간에 따라
    // 1. Interabtable + pointer 
    // 2. 움직이기 
    float currentTime;
    public float pointerTime = 0.5f;
    public float moveTime = 1;
    bool isClicked;
    InputField inputF;

    // 옮기기 위한 Transform 만들기
    public GameObject transform_Tool;

    public void Click()
    {
        isClicked = true;
    }
    public void UnClick()
    {
        isClicked = false;
    }
    
    void Start()
    {
        inputF = GetComponent<InputField>();
    }

    int i = 0;      // 이미지 하나만 만들기 위한 변수
    GameObject tool;

    void Update()
    {
        // 누르는 중이라면
        if(isClicked)
        {
            currentTime += Time.deltaTime;
        }
        // 뗐을 때 → 0.5초 이상이라면
        // 1초 이상이라면
        else
        {           

            if(currentTime>moveTime)
            {
                if (i < 1)
                {
                    // 이미지 inputField안에 자식으로 넣기
                    tool = Instantiate(transform_Tool);
                    tool.transform.SetParent(gameObject.transform);
                    tool.transform.localPosition = new Vector3(-420, 20, 0);
                    RectTransform tool_image = tool.GetComponent<Image>().rectTransform;
                    tool_image.sizeDelta = new Vector2(inputF.preferredWidth + 50, 65);
                    i++;
                }

                // 이미 한번 만들었다면 그걸 사용하자
                else
                {
                    tool.SetActive(true);
                    RectTransform tool_image = tool.GetComponent<Image>().rectTransform;
                    tool_image.sizeDelta = new Vector2(inputF.preferredWidth + 50, 65);
                }
                
            }
            else if (currentTime > pointerTime)
            {
                // 선택할 수 있게 만들기
                inputF.interactable = true;
                // 포커싱 나타나게 하기
                inputF.Select();
                inputF.ActivateInputField();
            }
            currentTime = 0;
        }

        // 포커싱을 잃었다면 트랜스폼 이미지를 끄자
        if(inputF.isFocused == false)
        {
            if(i>0 && tool.activeSelf == true)
            {
                tool.SetActive(false);
            }
        }
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition;
    }

}
