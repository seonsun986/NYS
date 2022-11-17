using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static NK_BookShelfManager;


// ��Ʈ����, åǥ�� �ٹ̱� (����, ����)
public class NK_BookCover : MonoBehaviour
{
    public static NK_BookCover instance;
    public TaleInfo taleInfo;
    public Transform bookCover;
    public InputField inputField;       // inputField ������
    public List<SH_InputField> inputFields = new List<SH_InputField>();
    // ���� ���õǾ��ִ� ��Ӵٿ�� �ؽ�Ʈ ������, �ؽ�Ʈ �÷�
    public Dropdown txtDropdown;
    public InputField InputtxtSize;
    // �ؽ�Ʈ �÷� �ݿ��� �̹���
    public Image txtcolorImage;
    // ��� �÷� �ݿ��� �̹���
    public Image bgColorImage;
    // �ؽ�Ʈ �÷� hex Color List
    public List<string> hexColor;
    // Object Instantiate�� ���� List
    public GameObject[] obj;
    public Image bookCoverColor;
    int text;
    // ������ ��ƼĿ
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
        // ��, ��Ʈ, ũ�� �ʱ�ȭ
        SetInfo(0, 30, Color.black);
        bgColorImage.color = Color.white;
        bookCoverColor.color = Color.white;
        inputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(51, 277);
        // �۾���
        inputField.textComponent.color = Color.black;

        // ��ƼĿ �ʱ�ȭ
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
        // ���� ����
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + fontColor, out colorInfo);
        Int32.TryParse(fontSize, out int size);
        Int32.TryParse(fontStyle, out int dropboxNum);
        // ��, ��Ʈ, ũ�� �ʱ�ȭ
        SetInfo(dropboxNum, size, colorInfo);
        inputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(float.Parse(fontPositionX), float.Parse(fontPositionY));
        // �۾���
        inputField.textComponent.color = colorInfo;
    }

    public void SetBookCover(string coverColor, string sticker, string stickerPositionX, string stickerPositionY)
    {
        // ���� ����
        Color colorInfo;
        ColorUtility.TryParseHtmlString("#" + coverColor, out colorInfo);
        bgColorImage.color = colorInfo;
        bookCoverColor.color = colorInfo;

        // ��ƼĿ ����
        InstantiateObj(sticker, stickerPositionX, stickerPositionY);
    }

    private void Start()
    {
        // ��Ʈ ��� �ڽ�, ��Ʈ ������ ���� ������ ���
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

    #region PaletteOnOff // ǥ�� �ؽ�Ʈ �ȷ�Ʈ ON/OFF
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

    #region BGPaletteOnOff // ǥ�� ��� �ȷ�Ʈ ON/OFF
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

    #region InstantiateObj // ǥ�� �����ϱ⿡�� ��ƼĿ ����
    public void InstantiateObj()
    {
        // ��ƼĿ �����ϸ�
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        // ��ƼĿ���� ��ư ����� �� ���� ��ü�� å ���� ������
        createObj = Instantiate(clickBtn);
        stickerList.Add(clickBtn.name);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        createObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //createObj.GetComponent <RectTransform>().localScale = new Vector3(1, 1, 1);
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }
    #endregion

    #region InstantiateObj // JSON���� ������ ����� ��ƼĿ �ҷ��ͼ� ǥ�� ������ �� �ٽ� �����ֱ�
    public void InstantiateObj(string btnName, string x, string y)
    {
        // Json���� ����� ��ƼĿ �̸�
        btnName = btnName.Replace("(Clone) (UnityEngine.GameObject)", "");
        print(btnName);
        // ��ƼĿ �̸����� ������Ʈ ã��
        GameObject clickBtn = GameObject.Find(btnName);
        // ��ƼĿ���� ��ư ����� �� ���� ��ü�� å ���� ������
        createObj = Instantiate(clickBtn);
        createObj.transform.SetParent(bookCover);
        createObj.GetComponent<Button>().enabled = false;
        // JSON���� �޾ƿ� ��ƼĿ�� ��ġ��� ��ġ
        createObj.GetComponent<RectTransform>().localPosition = new Vector2(float.Parse(x), float.Parse(y));
        createObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
    }
    #endregion

    #region DeleteObj // Ŭ���� ��ƼĿ ������Ʈ ����
    public void DeleteObj()
    {
        // NK_DragAndDrop���� OnPointerDown�� �� delSticker ��������
        // ���� ��ư ������ �������� Ŭ���� ��ƼĿ ����
        if (delSticker != null)
        {
            // ��ƼĿ ��Ͽ��� �ش� ��ƼĿ ����
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
        // �ȷ�Ʈ���� �� �����ϸ�
        string name = EventSystem.current.currentSelectedGameObject.name;
        int btnNum = int.Parse(name.Substring(3));
        // å ǥ�� ���� ����
        ColorUtility.TryParseHtmlString(hexColor[btnNum], out bgColor);
        bgColorImage.color = bgColor;
        bookCoverColor.color = bgColor;
    }

    string stickerListSet = "";
    public void SaveTaleInfo()
    {
        // �ȷ�Ʈ ����
        palette.SetActive(false);
        bgPalette.SetActive(false);
        // ���� ����
        TxtInfo txtInfo = new TxtInfo();
        SH_SceneObj txt = transform.GetComponentInChildren<SH_SceneObj>();
        SH_InputField txt2 = transform.GetComponentInChildren<SH_InputField>();
        taleInfo.fontPositionX = txt.gameObject.GetComponent<RectTransform>().anchoredPosition.x.ToString();
        taleInfo.fontPositionY = txt.gameObject.GetComponent<RectTransform>().anchoredPosition.y.ToString();
        taleInfo.fontStyle = txt2.info.txtDropdown.ToString();
        taleInfo.fontSize = txt2.info.txtSize.ToString();
        taleInfo.fontColor = ColorUtility.ToHtmlStringRGBA(txt2.transform.GetChild(3).GetComponent<Text>().color);

        // ��ƼĿ ��� �ֱ�
        taleInfo.sticker = createObj.ToString();
        // ��ƼĿ ��ġ�� �ֱ�
        taleInfo.stickerPositionX = createObj.GetComponent<RectTransform>().localPosition.x.ToString();
        taleInfo.stickerPositionY = createObj.GetComponent<RectTransform>().localPosition.y.ToString();
        taleInfo.coverColor = ColorUtility.ToHtmlStringRGBA(bgColor);
    }

    public void SaveListSet()
    {
        // ����Ʈ���ִ� �׸� ��ü �Ѱ��� string���� �����
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
