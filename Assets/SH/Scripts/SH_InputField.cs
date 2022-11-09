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
        TextInfo activeInfo = SH_EditorManager.Instance.active_InputField.info;

        if (NK_BookCover.instance != null)
        {
            NK_BookCover.instance.SetInfo(activeInfo.txtDropdown, activeInfo.txtSize, activeInfo.txtColor);
        }
        else
        {
            SH_BtnManager.Instance.SetInfo(activeInfo.txtDropdown, activeInfo.txtSize, activeInfo.txtColor);
        }
    }
    public void UnClick()
    {
        isClicked = false;
    }

    void Awake()
    {
        inputF = GetComponent<InputField>();
        rect = GetComponent<RectTransform>();
        tool = transform.GetChild(0).gameObject;
        tool.SetActive(false);
        info = new TextInfo();
    }
    private void Start()
    {

    }

    public void Initialize(Transform parent, int idx, Vector3 pos)
    {
        gameObject.name = "Text" + idx;
        transform.SetParent(parent);
        transform.localPosition = pos;
    }

    public void SetInfo(int txtDropdown, int txtSize, Color txtColor)
    {

        info.inputs = inputF.text;
        info.txtDropdown = txtDropdown;
        info.txtSize = txtSize;
        info.txtColor = txtColor;
    }

    public void SetFontSize(int fontSize)
    {
        inputF.textComponent.fontSize = fontSize;
        info.txtSize = fontSize;
    }

    public void SetFontColor(Color color)
    {
        inputF.textComponent.color = color;
        info.txtColor = color;
    }

    public void SetFontType(int type)
    {
        inputF.textComponent.font = SH_EditorManager.Instance.fonts[type];
        info.txtDropdown = type;
    }


    GameObject tool;

    void Update()
    {
        // inputField�� text���̵� ���缭 �þ���Ѵ�.
        // ��Ʈ ũ�⿡ ���缭�� �þ���Ѵ�
        // ���𰡰� ������ ������ ����
        //print("���� ���� : " + inputF.text.Length);
        if (inputF.text.Length > 5)
        {
            // ������ ���̶��
            if(NK_BookCover.instance == null)
                rect.sizeDelta = new Vector2(inputF.preferredWidth + 50, inputF.preferredHeight + 10);
            else
            {
                if(inputF.preferredWidth != rect.sizeDelta.x)
                {
                    inputF.textComponent.rectTransform.sizeDelta = new Vector2(rect.sizeDelta.x, inputF.preferredHeight);
                }
            }
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
            if (currentTime > moveTime)
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
            for (int j = 0; j < raycastResults.Count; j++)
            {
                // �˻����� �� Ŭ���� �κп� Tool�� �ִٸ�
                if (raycastResults[j].gameObject.name.Contains("Tool"))
                {
                    print("���� �����Ƿ� �Ȳ�!");
                    // ����Ʈ �ʱ�ȭ ���Ѿ���
                    raycastResults.Clear();

                    b = true;
                    break;
                }

            }

            if (b == false)
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
