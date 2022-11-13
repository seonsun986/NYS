using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        palette.SetActive(false);
        bgPalette.SetActive(false);
        Initialization();
    }

    public void Initialization()
    {
        // 색, 폰트, 크기 초기화
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

    public void SetBookCoverFont(string fontStyle, string fontColor, string fontSize, string fontPositionX, string fontPositionY)
    {
        // 색깔 적용
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + fontColor, out colorInfo);
        Int32.TryParse(fontSize, out int size);
        Int32.TryParse(fontStyle, out int dropboxNum);
        // 색, 폰트, 크기 초기화
        SetInfo(dropboxNum, size, colorInfo);
        inputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(float.Parse(fontPositionX), float.Parse(fontPositionY));
        // 글씨색
        inputField.textComponent.color = colorInfo;
    }

    public void SetBookCover(string coverColor, string sticker, string stickerPositionX, string stickerPositionY)
    {
        // 색깔 적용
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + coverColor, out colorInfo);
        bgColorImage.color = colorInfo;
        bookCoverColor.color = colorInfo;

        // 스티커 생성
        InstantiateObj(sticker, stickerPositionX, stickerPositionY);
    }

    private void Start()
    {
        txtDropdown.onValueChanged.AddListener(ChangeTextFont);
        InputtxtSize.onValueChanged.AddListener(ChangeFontSize);

        SH_InputField inputText = inputField.GetComponent<SH_InputField>();

        SH_EditorManager.Instance.active_InputField = inputText;
        inputText.SetInfo(txtDropdown.value, int.Parse(InputtxtSize.text), txtcolorImage.color);
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
    }

    public void ChangeTextColor()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        Color color;
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out color);
        txtcolorImage.color = color;
        SH_EditorManager.Instance.active_InputField.SetFontColor(color);
    }

    void ChangeTextFont(int index)
    {
        SH_EditorManager.Instance.active_InputField.SetFontType(index);
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

    GameObject createObj;
    public void InstantiateObj()
    {
        // 스티커 선택하면
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // 스티커에서 버튼 기능을 뺀 같은 객체가 책 위에 생성됨
        createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //createObj.GetComponent <RectTransform>().localScale = new Vector3(1, 1, 1);
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }

    public void InstantiateObj(string btnName, string x, string y)
    {
        btnName = btnName.Replace("(Clone) (UnityEngine.GameObject)", "");
        print(btnName);
        // 스티커 선택하면
        GameObject clickBtn = GameObject.Find(btnName);
        // 스티커에서 버튼 기능을 뺀 같은 객체가 책 위에 생성됨
        createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = new Vector2(float.Parse(x), float.Parse(y));
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }

    Color bgColor;
    public void ChangeBookColor()
    {
        // 팔레트에서 색 선택하면
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        // 책 표지 색상 변경
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out bgColor);
        bgColorImage.color = bgColor;
        bookCoverColor.color = bgColor;
    }

    public void SaveTaleInfo()
    {
        // 팔레트 끄기
        palette.SetActive(false);
        bgPalette.SetActive(false);
        // 정보 저장
        TxtInfo txtInfo = new TxtInfo();
        SH_SceneObj txt = transform.GetComponentInChildren<SH_SceneObj>();
        SH_InputField txt2 = transform.GetComponentInChildren<SH_InputField>();
        taleInfo.fontPositionX = txt.gameObject.GetComponent<RectTransform>().anchoredPosition.x.ToString();
        taleInfo.fontPositionY = txt.gameObject.GetComponent<RectTransform>().anchoredPosition.y.ToString();
        taleInfo.fontStyle = txt2.info.txtDropdown.ToString();
        taleInfo.fontSize = txt2.info.txtSize.ToString();
        taleInfo.fontColor = ColorUtility.ToHtmlStringRGBA(txt2.transform.GetChild(3).GetComponent<Text>().color);
        taleInfo.sticker = createObj.ToString();
        taleInfo.stickerPositionX = createObj.GetComponent<RectTransform>().localPosition.x.ToString();
        taleInfo.stickerPositionY = createObj.GetComponent<RectTransform>().localPosition.y.ToString();
        taleInfo.coverColor = ColorUtility.ToHtmlStringRGBA(bgColor);
    }
}
