using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SH_InputField : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // UI �ű�� �ʹ�! -> �⺻ ��ġ
    public static Vector3 defaultPos;

    // eventTrigger���� �����͸� ���ٿ��� �� �ð��� ����
    // 1. Interabtable + pointer 
    // 2. �����̱� 
    float currentTime;
    public float pointerTime = 0.5f;
    public float moveTime = 1;
    bool isClicked;
    bool isDragging;
    InputField inputF;
    RectTransform rect;
    int fontSize;
    // �ű�� ���� Transform �����
    public GameObject transform_Tool;
    // �ڱ� �ڽſ� ���� ����
    public TextInfo info;

    public void Click()
    {
        isClicked = true;
        // ������ InputField�� ��
        SH_EditorManager.Instance.active_InputField = this;
        SH_BtnManager.Instance.txtDropdown.value = SH_EditorManager.Instance.active_InputField.info.txtDropdown;
        SH_BtnManager.Instance.txtSize = SH_EditorManager.Instance.active_InputField.info.txtSize.ToString();
        SH_BtnManager.Instance.InputtxtSize.text = SH_EditorManager.Instance.active_InputField.info.txtSize.ToString();
        SH_BtnManager.Instance.txtColor = SH_EditorManager.Instance.active_InputField.info.txtColor;



    }
    public void UnClick()
    {
        isClicked = false;
    }
    
    void Start()
    {
        inputF = GetComponent<InputField>();
        rect = GetComponent<RectTransform>();
        tool = transform.GetChild(0).gameObject;
        tool.SetActive(false);
    }

    GameObject tool;

    void Update()
    {
        // inputField�� text���̵� ���缭 �þ���Ѵ�.
        // ��Ʈ ũ�⿡ ���缭�� �þ���Ѵ�
        // ���𰡰� ������ ������ ����
        print("���� ���� : " + inputF.text.Length);
        if(inputF.text.Length > 5)
        {
            rect.sizeDelta = new Vector2(inputF.preferredWidth + 50, inputF.preferredHeight + 10);
        }

        // ������ ���̶��
        if (isClicked)
        {
            currentTime += Time.deltaTime;
        }
        // ���� �� �� 0.5�� �̻��̶��
        // 1�� �̻��̶��
        else
        {           
            if(currentTime>moveTime)
            {
                {
                    tool.SetActive(true);
                    RectTransform tool_image = tool.GetComponent<Image>().rectTransform;
                    if (inputF.text.Length == 0)
                    {
                        tool_image.sizeDelta = new Vector2(250, 65);
                    }
                    else
                    {
                        tool_image.sizeDelta = new Vector2(inputF.preferredWidth + 50, inputF.preferredHeight + 40);
                    }

                    inputF.interactable = false;
                }
                
            }
            else if (currentTime > pointerTime && currentTime < moveTime)
            {
                print("��Ŀ�� ��Ÿ����!");
                // ������ �� �ְ� �����
                inputF.interactable = true;
                // ��Ŀ�� ��Ÿ���� �ϱ�
                inputF.Select();
                inputF.ActivateInputField();
            }
            currentTime = 0;
        }

        // tool �̹����� �������� �� 
        // ���콺�� Ŭ������ �� ������Ʈ�� InputField�� �ƴ϶��
        // tool�� ����
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            bool b = false;
            for(int j =0;j<raycastResults.Count; j++)
            {
                // �˻����� �� Ŭ���� �κп� Tool�� �ִٸ�
                if(raycastResults[j].gameObject.name.Contains("Tool"))
                {
                    print("���� �����Ƿ� �Ȳ�!");
                    // ����Ʈ �ʱ�ȭ ���Ѿ���
                    raycastResults.Clear();

                    b = true;
                    break;
                }

            }

            if(b == false)
            {
                print("�� ����!!");
                tool.SetActive(false);
                // ����Ʈ �ʱ�ȭ ���Ѿ���
                raycastResults.Clear();
            }
             
        }

   
    }

    #region UI �巡�� �Լ�

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = transform.position;
    }

    // �巡�� ��
    // �巡�� �߿��� tool �̹����� ���� ���� �ʴ´�
    public void OnDrag(PointerEventData eventData)
    {
        if (tool.activeSelf == false) return;
        Vector3 currentPos = Input.mousePosition;
        transform.position = currentPos;
        isDragging = true;
    }

    // �巡�� ������ ��
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = mousePos;
        isDragging = false;
    }

    #endregion

}
