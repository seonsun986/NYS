using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static NK_BookShelfManager;

public class NK_BookCover : MonoBehaviour
{
    public static NK_BookCover instance;
    public TaleInfo taleInfo;
    public Transform bookCover;
    public InputField inputField;       // inputField 프리팹
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // 현재 선택되어있는 드롭다운과 텍스트 사이즈, 텍스트 컬러
    public Dropdown txtDropdown;
    public InputField InputtxtSize;
    // 텍스트 컬러 반영된 이미지
    public Image txtcolorImage;
    // 배경 컬러 반영된 이미지
    public Image bgColorImage;
    // 텍스트 컬러 hex Color List
    public List<string> hexColor;
    // Object Instantiate를 위한 List
    public GameObject[] obj;
    public Image bookCoverColor;
    int text;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        taleInfo = new TaleInfo();
        // 색, 폰트, 크기, 팔레트 켜짐 초기화
        palette.SetActive(false);
        bgPalette.SetActive(false);
        SetInfo(0, 30, Color.black);
        bgColorImage.color = Color.white;
        bookCoverColor.color = Color.white;
        inputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(51, 277);
        // 글씨색
        inputField.textComponent.color = Color.black;

        // 스티커 초기화
        Transform[] childList = bookCover.GetComponentsInChildren<Transform>();

        if (childList != null && childList.Length > 6)
        {
            for (int i = 6; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }
    }

    private void Start()
    {
        SH_InputField inputText = inputField.GetComponent<SH_InputField>();

        SH_EditorManager.Instance.active_InputField = inputText;
        SetInfo(0, 30, Color.black);
        inputText.SetInfo(txtDropdown.value, int.Parse(InputtxtSize.text), txtcolorImage.color);

        txtDropdown.onValueChanged.AddListener(ChangeTextFont);
        InputtxtSize.onValueChanged.AddListener(ChangeFontSize);
    }

    public void SetInfo(int dropdown, int inputTextSize, Color txtColor)
    {
        txtDropdown.value = dropdown;
        InputtxtSize.text = inputTextSize.ToString();
        txtcolorImage.color = txtColor;
    }

    public void PlusSize()
    {
        int size = int.Parse(InputtxtSize.text);
        size++;
        InputtxtSize.text = size.ToString();
    }

    public void MinusSize()
    {
        int size = int.Parse(InputtxtSize.text);
        size--;
        InputtxtSize.text = size.ToString();
    }

    public void ChangeFontSize(string size)
    {
        SH_EditorManager.Instance.active_InputField.SetFontSize(int.Parse(size));
        taleInfo.fontSize = size;
    }

    public void ChangeTextColor()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        Color color;
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out color);
        txtcolorImage.color = color;
        SH_EditorManager.Instance.active_InputField.SetFontColor(color);
        taleInfo.fontColor = color.ToString();
    }

    void ChangeTextFont(int index)
    {
        SH_EditorManager.Instance.active_InputField.SetFontType(index);
        taleInfo.fontStyle = index.ToString();
    }

    public GameObject palette;
    public void PaletteOnOff()
    {
        if (palette.activeSelf == true)
        {
            palette.SetActive(false);
        }
        else
        {
            palette.SetActive(true);
        }
    }
    
    public GameObject bgPalette;
    public void BGPaletteOnOff()
    {
        if (bgPalette.activeSelf == true)
        {
            bgPalette.SetActive(false);
        }
        else
        {
            bgPalette.SetActive(true);
        }
    }

    public void InstantiateObj()
    {
        // 스티커 선택하면
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // 스티커에서 버튼 기능을 뺀 같은 객체가 책 위에 생성됨
        GameObject createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //createObj.GetComponent <RectTransform>().localScale = new Vector3(1, 1, 1);
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
        taleInfo.sticker = createObj.ToString();
        taleInfo.stickerPositionX = createObj.GetComponent<RectTransform>().localPosition.x.ToString();
        taleInfo.stickerPositionY = createObj.GetComponent<RectTransform>().localPosition.y.ToString();
    }

    public void ChangeBookColor()
    {
        // 팔레트에서 색 선택하면
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        // 책 표지 색상 변경
        Color color;
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out color);
        bgColorImage.color = color;
        bookCoverColor.color = color;
        taleInfo.coverColor = color.ToString();
    }
}
