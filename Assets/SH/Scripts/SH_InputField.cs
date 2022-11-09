using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SH_InputField : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // UI 옮기고 싶다! -> 기본 위치
    public static Vector3 defaultPos;

    // eventTrigger에서 포인터를 업다운을 한 시간에 따라
    // 1. Interabtable + pointer 
    // 2. 움직이기 
    float currentTime;
    public float pointerTime = 0.5f;
    public float moveTime = 1;
    bool isClicked;
    bool isDragging;
    InputField inputF;
    RectTransform rect;
    int fontSize;
    // 옮기기 위한 Transform 만들기
    public GameObject transform_Tool;
    // 자기 자신에 대한 정보
    public TextInfo info;

    public void Click()
    {
        isClicked = true;
        // 선택한 InputField가 들어감

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
        // inputField의 text길이도 맞춰서 늘어나야한다.
        // 폰트 크기에 맞춰서도 늘어나야한다
        // 무언가가 적히기 시작한 순간
        //print("문자 길이 : " + inputF.text.Length);
        if (inputF.text.Length > 5)
        {
            // 에디터 씬이라면
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

        // 누르는 중이라면
        if (isClicked)
        {
            currentTime += Time.deltaTime;
        }
        // 뗐을 때 → 0.5초 이상이라면
        // 1초 이상이라면
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
                print("포커싱 나타난다!");
                // 선택할 수 있게 만들기
                inputF.interactable = true;
                // 포커싱 나타나게 하기
                inputF.Select();
                inputF.ActivateInputField();
            }
            currentTime = 0;
        }

        // tool 이미지가 켜져있을 때 
        // 마우스를 클릭했을 때 오브젝트가 InputField가 아니라면
        // tool을 끈다
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            bool b = false;
            for (int j = 0; j < raycastResults.Count; j++)
            {
                // 검사했을 때 클릭한 부분에 Tool이 있다면
                if (raycastResults[j].gameObject.name.Contains("Tool"))
                {
                    print("툴이 있으므로 안끔!");
                    // 리스트 초기화 시켜야함
                    raycastResults.Clear();

                    b = true;
                    break;
                }

            }

            if (b == false)
            {
                print("툴 꺼짐!!");
                tool.SetActive(false);
                // 리스트 초기화 시켜야함
                raycastResults.Clear();
            }

        }


    }

    #region UI 드래그 함수

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = transform.position;
    }

    // 드래그 중
    // 드래그 중에는 tool 이미지를 끄게 하지 않는다
    public void OnDrag(PointerEventData eventData)
    {
        if (tool.activeSelf == false) return;
        Vector3 currentPos = Input.mousePosition;
        transform.position = currentPos;
        isDragging = true;
    }

    // 드래그 끝났을 때
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = mousePos;
        isDragging = false;
    }

    #endregion

}
