using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static NK_BookShelfManager;


// 폰트변경, 책표지 꾸미기 (생성, 삭제)
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
    // 삭제될 스티커
    public GameObject delSticker;

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
        // 폰트 드롭 박스, 폰트 사이즈 관련 리스너 등록
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

    #region PaletteOnOff // 표지 텍스트 팔레트 ON/OFF
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
    #endregion

    #region BGPaletteOnOff // 표지 배경 팔레트 ON/OFF
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
    #endregion

    GameObject createObj;
    List<string> stickerList = new List<string>();
    List<string> stickerListPosX = new List<string>();
    List<string> stickerListPosY = new List<string>();

    #region InstantiateObj // 표지 수정하기에서 스티커 생성
    public void InstantiateObj()
    {
        // 스티커 선택하면
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // 스티커에서 버튼 기능을 뺀 같은 객체가 책 위에 생성됨
        createObj = Instantiate(clickBtn);
        stickerList.Add(clickBtn.name);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //createObj.GetComponent <RectTransform>().localScale = new Vector3(1, 1, 1);
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }
    #endregion

    #region InstantiateObj // JSON에서 기존에 저장된 스티커 불러와서 표지 수정할 때 다시 보여주기
    public void InstantiateObj(string btnName, string x, string y)
    {
        // Json에서 저장된 스티커 이름
        btnName = btnName.Replace("(Clone) (UnityEngine.GameObject)", "");
        print(btnName);
        // 스티커 이름으로 오브젝트 찾기
        GameObject clickBtn = GameObject.Find(btnName);
        // 스티커에서 버튼 기능을 뺀 같은 객체가 책 위에 생성됨
        createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        // JSON에서 받아온 스티커의 위치대로 배치
        createObj.GetComponent<RectTransform>().localPosition = new Vector2(float.Parse(x), float.Parse(y));
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }
    #endregion

    #region DeleteObj // 클릭한 스티커 오브젝트 삭제
    public void DeleteObj()
    {
        // NK_DragAndDrop에서 OnPointerDown일 때 delSticker 저장해줌
        // 삭제 버튼 눌러서 마지막에 클릭된 스티커 삭제
        if (delSticker != null)
        {
            // 스티커 목록에서 해당 스티커 삭제
            for (int i = 0; i < stickerList.Count; i++)
            {
                if (stickerList[i] == delSticker.name)
                {
                    stickerList.RemoveAt(i);
                }
                
            }
            Destroy(delSticker);
        }
    }
    #endregion

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

    string stickerListSet = "";
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

        // 스티커 목록 넣기
        taleInfo.sticker = createObj.ToString();
        // 스티커 위치값 넣기
        taleInfo.stickerPositionX = createObj.GetComponent<RectTransform>().localPosition.x.ToString();
        taleInfo.stickerPositionY = createObj.GetComponent<RectTransform>().localPosition.y.ToString();
        taleInfo.coverColor = ColorUtility.ToHtmlStringRGBA(bgColor);
    }

    public void SaveListSet()
    {
        // 리스트에있는 항목 전체 한개의 string으로 만들기
        for (int i = 0; i < stickerList.Count; i++)
        {
            if (i == stickerList.Count - 1)
            {
                stickerListSet += stickerList[i];
                break;
            }

            stickerListSet += stickerList[i] + ",";
        }


    }
}
