using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance; 
    public SH_InputField active_InputField;     // 현재 선택된 InputField
    public Dropdown font;                       // 폰트
    public Text fontSize;                       // 폰트 사이즈
    public Font[] fonts;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (active_InputField != null)
        {
            // 현재 선택되어 있는 InputField의 값을 바꿔보자
            active_InputField.info.txtDropdown = font.value;
            active_InputField.transform.GetChild(3).GetComponent<Text>().font = fonts[active_InputField.info.txtDropdown];
            active_InputField.info.txtSize = int.Parse(fontSize.text);
            active_InputField.transform.GetChild(3).GetComponent<Text>().fontSize = active_InputField.info.txtSize;
        }
    }
}
