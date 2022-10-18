using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_InputField : MonoBehaviour
{

    // eventTrigger���� �����͸� ���ٿ��� �� �ð��� ����
    // 1. Interabtable + pointer 
    // 2. �����̱� 
    float currentTime;
    public float pointerTime = 0.5f;
    public float moveTime = 1;
    bool isClicked;
    InputField inputF;

    // �ű�� ���� Transform �����
    public GameObject transform_Tool;
    // �ڱ� �ڽſ� ���� ����
    public TextInfo info;
    public void Click()
    {
        isClicked = true;
        // ������ InputField�� ��
        SH_EditorManager.Instance.active_InputField = this;
    }
    public void UnClick()
    {
        isClicked = false;
    }
    
    void Start()
    {
        inputF = GetComponent<InputField>();
    }

    int i = 0;      // �̹��� �ϳ��� ����� ���� ����
    GameObject tool;

    // tool�� �������� �� 
    // ���콺�� Ŭ���ϴ°� InputField�� �ƴ϶��
    // 
    void Update()
    {
        // ������ ���̶��
        if(isClicked)
        {
            currentTime += Time.deltaTime;
        }
        // ���� �� �� 0.5�� �̻��̶��
        // 1�� �̻��̶��
        else
        {           

            if(currentTime>moveTime)
            {
                if (i < 1)
                {
                    // �̹��� inputField�ȿ� �ڽ����� �ֱ�
                    tool = Instantiate(transform_Tool);
                    tool.transform.SetParent(gameObject.transform);
                    tool.transform.localPosition = new Vector3(-420, 20, 0);
                    RectTransform tool_image = tool.GetComponent<Image>().rectTransform;
                    tool_image.sizeDelta = new Vector2(inputF.preferredWidth + 50, 65);
                    i++;
                }

                // �̹� �ѹ� ������ٸ� �װ� �������
                else
                {
                    tool.SetActive(true);
                    RectTransform tool_image = tool.GetComponent<Image>().rectTransform;
                    tool_image.sizeDelta = new Vector2(inputF.preferredWidth + 50, 65);
                }

                inputF.interactable = false;
                
            }
            else if (currentTime > pointerTime)
            {
                // ������ �� �ְ� �����
                inputF.interactable = true;
                // ��Ŀ�� ��Ÿ���� �ϱ�
                inputF.Select();
                inputF.ActivateInputField();
            }
            currentTime = 0;
        }

        // ��Ŀ���� �Ҿ��ٸ� Ʈ������ �̹����� ����
        //if(inputF.isFocused == false)
        //{
        //    if(i>0 && tool.activeSelf == true)
        //    {
        //        tool.SetActive(false);
        //    }
        //}

        // 
    }

    public void OnDrag()
    {
        print("�巡�� ���̴�!");
        transform.position = Input.mousePosition;
    }

}
