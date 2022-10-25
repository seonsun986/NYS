using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_EditorManager : MonoBehaviour
{
    public static SH_EditorManager Instance;

    #region 텍스트 관련 선언 변수
    public SH_InputField active_InputField;     // 현재 선택된 InputField
    public Dropdown font;                       // 폰트
    public Text fontSize;                       // 폰트 사이즈
    public Font[] fonts;
    #endregion

    // 생성된 오브젝트 애니메이션 담아놓는다
    [Header("오브젝트 애니메이션")]
    public List<Animation> anim = new List<Animation>();
    // 딕셔너리 선언해서 Walk -> 걷기
    public Dictionary<string, string> animName;
    
    // 선택한 버튼에 따라 Instantiate되는 친구들이 달라진다
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        animName = new Dictionary<string, string>
        {
            { "Walk", "걷기"},
            { "Run", "뛰기"},
            {"Eat", "먹기" },
            { "Jump", "점프" },
        };
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
