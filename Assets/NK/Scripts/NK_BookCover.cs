using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NK_BookCover : MonoBehaviour
{
    public static NK_BookCover instance;

    public Transform bookCover;
    public InputField inputField;       // inputField 프리팹
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // 현재 선택되어있는 드롭다운과 텍스트 사이즈, 텍스트 컬러
    public Dropdown txtDropdown;
    public InputField InputtxtSize;
    // 텍스트 컬러 반영된 이미지
    public Image txtcolorImage;
    // 텍스트 컬러 hex Color List
    public List<string> hexColor;
    // Object Instantiate를 위한 List
    public GameObject[] obj;
    int text;

    private void Awake()
    {
        instance = this;
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

    public void InstantiateObj()
    {
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        GameObject createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        createObj.GetComponent <RectTransform>().localScale = new Vector3(1, 1, 1);
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
    }
}
